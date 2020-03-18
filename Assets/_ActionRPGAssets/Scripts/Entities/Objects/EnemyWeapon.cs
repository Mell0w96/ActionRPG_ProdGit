using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour , IdamageDealer
{
    private CharacterMovement player;
    public float damageAmmount;

    private void Start()
    {
        player = CharacterMovement.FindObjectOfType <CharacterMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("HIT");
            DealDamage(damageAmmount);

        }
    }

    public void DealDamage(float damageAmmount) 
    {
        player.TakeDamage(damageAmmount);
    
    }
}
