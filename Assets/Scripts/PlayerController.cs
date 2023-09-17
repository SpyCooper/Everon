using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // controls used by the player that shouldn't be in the start asset menu

    // general
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    [SerializeField] private GameObject player;
    private CharacterData characterData;

    // pause menu
    [SerializeField] private GameObject pauseMenuObject;
    private PauseMenu pauseMenu;
    private bool pauseMenuOpen;

    // attack
    private bool canAttack = true;
    private Animator animator;
    private string attackOneTriggerName = "Attack 1";
    private float attackCooldown = 0.8f;
    [SerializeField] private AudioClip attackOneSound;
    [SerializeField] private AudioClip attackTwoSound;
    [SerializeField] private AudioClip attackThreeSound;
    [SerializeField] private GameObject meleeHitBox;
    [SerializeField] private GameObject attackTrail;

    // spawn point
    [SerializeField] private GameObject spawnPoint;

    // death and alive states
    private bool inDeathAnimation = false;
    private string deathTriggerName = "Death";
    private string aliveTriggerName = "Alive";

    // timers
    private float outOfCombatTimerMax = 7f;
    private float outOfCombatTimer;
    private float oneSecondTimer = 1f;
    private bool inCombat = false;

    // on Awake
    private void Awake()
    {
        // sets up components on the same object
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        pauseMenu = pauseMenuObject.GetComponent<PauseMenu>();
        animator = GetComponent<Animator>();

        // disables the pause menu
        pauseMenuOpen = false;

        // sets the out of combat timer to max
        outOfCombatTimer = outOfCombatTimerMax;
    }

    // on Start
    public void Start()
    {
        // sets the character data to the same as the PlayerManager
        characterData = PlayerManager.Instance.currentCharacterData;
        // listens to the required events
        starterAssetsInputs.togglePauseMenu += StarterAssetsInputs_TogglePauseMenu;
        pauseMenu.closePauseMenu += PauseMenu_ClosePauseMenu;
        characterData.PlayerTookDamage += CharacterData_PlayerTookDamage;

        // sets the attack trail as off
        attackTrail.SetActive(false);

        // respawns the player
        RespawnPlayer();

        // sets the game data's spawn point to the same as spawn point used to respawn the player
        PlayerManager.Instance.currentGameData.spawnPoint = spawnPoint;
    }

    // on Update
    private void Update()
    {
        // if(starterAssetsInputs.pauseMenuActive)
        // {
        //     if(!pauseMenuOpen)
        //     {
        //         OpenPauseMenu();
        //         starterAssetsInputs.pauseMenuActive = false;
        //     }
        //     else
        //     {
        //         ClosePauseMenu();
        //         starterAssetsInputs.pauseMenuActive = false;
        //     }
        // }

        // decreases timers by Time.deltaTime
        oneSecondTimer -= Time.deltaTime;
        outOfCombatTimer -= Time.deltaTime;

        // if the player is not in combat
        if(inCombat)
        {
            // if the out of combat timer is 0
            if(outOfCombatTimer <= 0f)
            {
                // resets the one second timer and the in combat timer
                oneSecondTimer = 1f;
                inCombat = false;
            }
        }

        // regens mana and health every second
        if(oneSecondTimer <= 0f)
        {
            oneSecondTimer = 1f;
            characterData.RegenMP();
            // if the player is not in combat (not being hit), regen HP
            if(!inCombat)
            {
                characterData.RegenHP();
            }
        }

        // if the player is not in the death animation
        if(!inDeathAnimation)
        {
            // checks to see if the player is below or at 0 hp
            if(characterData.GetCurrentHP() <= 0 )
            {
                // resets the attack triggers
                ResetTriggersOfType(attackOneTriggerName);

                // starts the coroutine of the player dying
                StartCoroutine(PlayerHasDied());
            }

            // if the player is trying to attack
            if(starterAssetsInputs.attacking)
            {
                starterAssetsInputs.attacking = false;

                // if there is no UI elements under the mouse, the player can attack
                if(PlayerManager.Instance.IsCursorOverUI() || PlayerManager.Instance.currentGameData.cameraState == 2)
                {
                    // if the player can attack
                    if(canAttack && PlayerManager.Instance.currentCharacterData.currentWeapon != null)
                    {
                        // activate a melee attack
                        MeleeAttack();
                    }
                }
            }
        }
    }

    // opens the pause menu
    private void OpenPauseMenu()
    {
        // unlocks the mouse
        starterAssetsInputs.setMouseLock(false);

        // locks camera position
        thirdPersonController.SetLockCameraPosition(true);

        // shows the pause menu
        pauseMenu.Show();
        pauseMenuOpen = true;

        // turns the time scale to 0, stopping the game
        Time.timeScale = 0f;
    }

    // closes the pause menu
    public void ClosePauseMenu()
    {
        if(PlayerManager.Instance.currentGameData.cameraState == 2)
        {
            // locks the mouse
            starterAssetsInputs.setMouseLock(true);
        }

        // unlocks camera position
        thirdPersonController.SetLockCameraPosition(false);

        // closes the pause menu
        pauseMenu.Hide();
        pauseMenuOpen = false;

        // turns the time scale to 1, starting the game
        Time.timeScale = 1f;
    }

    // toggles the pause menu
    private void StarterAssetsInputs_TogglePauseMenu(object sender, System.EventArgs e)
    {
        if(!pauseMenuOpen)
        {
            OpenPauseMenu();
            starterAssetsInputs.pauseMenuActive = false;
        }
        else
        {
            ClosePauseMenu();
            starterAssetsInputs.pauseMenuActive = false;
        }
    }

    // closes the pause menu
    private void PauseMenu_ClosePauseMenu(object sender, System.EventArgs e)
    {
        ClosePauseMenu();
    }

    // activates a melee attack
    private void MeleeAttack()
    {
        // TODO - add back when inventory is set up
        // if(PlayerManager.Instance.currentCharacterData.currentWeapon == null)
        // {
        //     return;
        // }

        // player can no longer attack until the attack timer is up
        canAttack = false;

        // starts the attack animation and plays the sound
        animator.SetTrigger(attackOneTriggerName);
        SoundManager.Instance.PlaySound(attackOneSound, transform.position);
        
        // starts the cooldown for an attack
        // when using daggers or staves, you can make a different trail for them
        StartCoroutine(AttackOneSlashTimer()); 
    }


    // goes through the attack animation and hits enemies
    private IEnumerator AttackOneSlashTimer()
    {
        // waits to line up the animation with the damage calculation
        yield return new WaitForSeconds(0.3f);

        // activates the melee hitboc
        meleeHitBox.GetComponent<WeaponCollisionDetection>().Show();

        // activates the attack trail
        attackTrail.SetActive(true);
        
        // waits for a bit to deactivate the hitbox
        yield return new WaitForSeconds(0.3f);
        meleeHitBox.GetComponent<WeaponCollisionDetection>().Hide();

        // waits for the cooldown of the attack to allow another attack
        // 0.6f is the time for the other yield return statements
        yield return new WaitForSeconds(attackCooldown - 0.6f);
        canAttack = true;

        // deactivates the attack trail
        attackTrail.SetActive(false);
    }

    // resets triggers of a type
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

    // plays the death animation and then respawns the player
    private IEnumerator PlayerHasDied()
    {
        inDeathAnimation = true;
        animator.SetTrigger(deathTriggerName);
        yield return new WaitForSeconds(2.5f);
        animator.SetTrigger(aliveTriggerName);
        RespawnPlayer();
    }

    // respawns the player with full HP at the spawn point
    private void RespawnPlayer()
    {
        PlayerManager.Instance.currentCharacterData.RestoreHPToFull();
        player.transform.position = spawnPoint.transform.position;
        inDeathAnimation = false;
    }

    // the player is in combat and has to wait until they are out of combat to regen HP
    public void InCombat()
    {
        inCombat = true;
        outOfCombatTimer = outOfCombatTimerMax;
    }

    // when PlayerTookDamage, InCombate() is run
    private void CharacterData_PlayerTookDamage(object sender, System.EventArgs e)
    {
        InCombat();
    }

    // when a collider enters the player's hitbox
    private void OnTriggerEnter(Collider collider)
    {
        // if the collider has the itemWorld component (i.e. is an item in the world), the player picks up the item
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            bool collected = true;
            // player touched an item in the world
            if(itemWorld.GetItem().type == ItemType.Coin)
            {
                PlayerManager.Instance.currentCharacterData.AddPlayerCurrency(itemWorld.GetStackAmount());
            }
            else
            {
               collected = InventoryManager.Instance.AddItem(itemWorld.GetItem(), itemWorld.GetStackAmount());
                Debug.Log(collected);
            }

            // destroys the item in the world
            if(collected)
            {
                itemWorld.DestroySelf();
            }
            
        }
    }

    public void LockPlayerMovement()
    {
        thirdPersonController.playerLock = true;
    }

    public void UnlockPlayerMovement()
    {
        thirdPersonController.playerLock = false;
    }
}
