using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class GreenSlimeAI : MonoBehaviour
{
    //used for the green slime but should theoretically work for any slime models that follow the same attack patterns

    // references set when the enemy is created
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private Enemy enemyScript;

    // reference to the entire object (used to destroy the enemy when killed)
    [SerializeField] private GameObject wholeEnemy;

    // idle wandering
    private float idleMovementTimer;
    private float idleMovementTimerMax = 5f;
    private bool idleMovementHappening;
    private bool walkPointSet;
    private Vector3 walkPoint;
    private float walkPointRange = 5f;
    private bool animated;
    private float animationMoveTimer;
    private float animationMoveTimerMax = 0.25f;
    private string idleMovementAnimationTriggerName = "Move";

    // chasing the player
    private float visionRange = 8f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private bool playerIsInVisionRange;
    private string jumpingMovementAnimationTriggerName = "Jump";

    // attacking
    private float attackRange = 1.5f;
    private bool playerIsInAttackRange;
    private bool alreadyAttacked;
    private float attackCooldown = 2.5f;
    private string attackAnimationTriggerName = "Attack";

    // death
    private string deathAnimationTriggerName = "Death";
    private bool dead = false;
    private string deadStateName = "Dead";

    // on Awake
    private void Awake()
    {
        // sets up things that need to be set before the program runs
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<Enemy>();
        idleMovementTimer = idleMovementTimerMax;
        walkPointSet = false;
    }

    // on Start
    private void Start()
    {
        // gets the reference to the player transform (has to be in start since awake is when player manager is set)
        player = PlayerManager.Instance.player.transform;
    }

    // on Update
    private void Update()
    {
        // makes sure there is HP left
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(deadStateName))
        {
            // if the enemy is in the DeadState animation, the enemy is destroyed
            DestroyEnemy();
        }
        // if the enemy is not dead
        else if(!dead)
        {
            // checks to see if there is HP left
            if(enemyScript.GetCurrentHP() <= 0)
            {
                // if HP is at 0, it runs IsKilled(), which starts the death
                IsKilled();
            }
            // if the HP is above 0
            else
            {
                // checks to see if the player is in vision or attack range
                // ranges are set in the variables section
                playerIsInVisionRange = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
                playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
                
                // if the player is in the attack range
                if(playerIsInAttackRange)
                {
                    // stands still and attack the player

                    // resets all the trigger or "queued" animations
                    ResetTriggersOfType(idleMovementAnimationTriggerName);
                    ResetTriggersOfType(jumpingMovementAnimationTriggerName);

                    // resets the idle movement timer, the timer between each idle movement
                    idleMovementTimer = idleMovementTimerMax;

                    // if the enemy has not already attacked
                    if(!alreadyAttacked)
                    {
                        // idleMovementHappening is set to false
                        idleMovementHappening = false;

                        // look direction is set to look at the player position
                        SetLookDirection(player.position);

                        // start the coroutine attack, which handles how the animation and damage calculations are timed
                        StartCoroutine(Attack());
                    }

                    // resets the walkpoint of a slime
                    walkPointSet = false;
                }
                // if the player is in the vision range but not the attack range
                else if(playerIsInVisionRange)
                {
                    // move toward the player

                    // resets any queued idle movement animation triggers
                    ResetTriggersOfType(idleMovementAnimationTriggerName);

                    // resets the idle movement timer and idleMovementHappening is set to false
                    idleMovementTimer = idleMovementTimerMax;
                    idleMovementHappening = false;

                    // set the trigger for the movement
                    animator.SetTrigger(jumpingMovementAnimationTriggerName);

                    // sets the destination of the enemy to be the player's position
                    agent.SetDestination(player.position);

                    // resets the walkpoint of the snemy
                    walkPointSet = false;
                }
                // if the player is not in the vision range or attack range
                else if(!playerIsInVisionRange && !playerIsInAttackRange)
                {
                    // move around every few seconds

                    // if idle movement is not currently happening
                    if(!idleMovementHappening)
                    {
                        // idleMovement timer is decreased
                        idleMovementTimer -= Time.deltaTime;
                    }
                    // if the timer is less than or equal to 0, patrol() is called which starts the idle movement
                    if(idleMovementTimer <= 0)
                    {
                        Patrol();
                    }

                    // if the enemy is in an idle movement, it should call patrol again and again until the idle movement is done or the player is spotted
                    // the timer should not go down until the idle movement is done
                }
            }
        }
    }

    // controls the idle movement of the enemy
    private void Patrol()
    {
        // if there is no walkpoint set, one is set
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }

        // if a walkpoint is set
        if(walkPointSet)
        {
            // checks to see if the animation for movement is running for the movement
            animated = animator.GetCurrentAnimatorStateInfo(0).IsName("GreenSlimeMove");

            // if the animation is not running
            if(!animated)
            {
                // the animation is started
                animator.SetTrigger(idleMovementAnimationTriggerName);
                // the timer between when charge up of the move is set
                animationMoveTimer = animationMoveTimerMax;
                // the enemy is considered animated
                animated = true;
            }
            // if the enemy is already animated
            else if(animated)
            {
                // if the timer is below or at 0
                if(animationMoveTimer <= 0f)
                {
                    // idleMovement is happening
                    idleMovementHappening = true;
                    // the walkpoint is set
                    agent.SetDestination(walkPoint);
                    // the direction of the enemy is set to the walkpoint
                    SetLookDirection(walkPoint);
                }
                // if the timer is above 0
                else
                {
                    // the timer is decreased by Time.deltaTime
                    animationMoveTimer -= Time.deltaTime;
                }
            }

            // if the enemy is within 1.6 units of the walkpoint
            if(Vector3.Distance(transform.position, walkPoint) <= 1.6f)
            {
                // the movement is done and everything is reset
                walkPointSet = false;
                idleMovementTimer = idleMovementTimerMax;
                idleMovementHappening = false;
                animated = false;
                ResetTriggersOfType(idleMovementAnimationTriggerName);
            }
        }

    }

    // searched for an available walkpoint withing the walkpoint range
    private void SearchWalkPoint()
    {
        // find a random point in range
        // walkpoint range is set in the variables section
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        // NOTE: there was a check but was messing up the code
        walkPointSet = true;
    }

    // sets the enemy's direction to face the target
    private void SetLookDirection(Vector3 target)
    {
        Vector3 lookDirection = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Attempts to line up the animation and the time attack damage is taken
    private IEnumerator Attack()
    {
        // considered to be already attack at when this is called
        alreadyAttacked = true;

        // starts the attack animation
        animator.SetTrigger(attackAnimationTriggerName);

        // delay in animation
        yield return new WaitForSeconds(0.5f);

        // attack damage calculation
        if(enemyScript.GetCurrentHP() != 0)
        {
            enemyScript.Attack();
        }

        // waits until the attack cooldown is done
        yield return new WaitForSeconds(attackCooldown);

        // is considered to not be attacking
        alreadyAttacked = false;
    }

    // resets the trigger of whatever name is sent to it
    private void ResetTriggersOfType(string triggerName)
    {
        foreach(var trigger in animator.parameters)
        {
            if(trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(triggerName);
            }
        }
    }

    // Starts the death of the enemy
    private void IsKilled()
    {
        // is considered dead
        dead = true;

        // resets all of the "queued" triggers
        ResetTriggersOfType(idleMovementAnimationTriggerName);
        ResetTriggersOfType(jumpingMovementAnimationTriggerName);
        ResetTriggersOfType(attackAnimationTriggerName);
        // starts the death animation
        animator.SetTrigger(deathAnimationTriggerName);

        // finds the collider of the enemy
        Collider enemyCollider = GetComponent<Collider>();
        // send the collider to be removed from the list of enemys in the hitbox
        // PlayerManager.Instance.playerMeleeHitBox.GetComponent<WeaponCollisionDetection>().EnemyHasDied(enemyCollider);

        //adds XP to the player based on the enemy's SO
        PlayerManager.Instance.currentCharacterData.AddPlayerXP(enemyScript.GetXP());


        // TODO - drop items
    }

    // destroys the enemy's game object
    private void DestroyEnemy()
    {
        Destroy(wholeEnemy);
    }
}
