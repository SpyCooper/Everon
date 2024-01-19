using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionDetection : MonoBehaviour
{
    // Detects enemies in a melee collision

    // on start, deactivates the hitbox
    private void Start()
    {
        Hide();
    }

    private void OnTriggerEnter(Collider other)
    {
        // replace with the current weapons
        WeaponSO currentWeapon = PlayerManager.Instance.currentCharacterData.getCurrentWeapon();

        // if the collider has the component enemy, they take damage
        if(other.GetComponent<Enemy>())
        {
            int attackDealt = currentWeapon.attack * (PlayerManager.Instance.currentCharacterData.GetSTR()+1);
            Debug.Log("Attack dealt = " + attackDealt);
            Debug.Log(attackDealt);
            other.GetComponent<Enemy>().TakeDamage(attackDealt);
        }
    }

    // shows the hit box, which will trigger the on trigger enter
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    // deactivates the hitbox
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
