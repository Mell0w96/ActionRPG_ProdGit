using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour , IdamageDealer
{
    private CharacterControls player;
    public float damageAmmount;

    private void Start()
    {
        player = CharacterControls.FindObjectOfType <CharacterControls>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("HIT");
            DealDamage(damageAmmount);

        }
    }

    public void DealDamage(float damageAmmount) 
    {
        player.TakeDamage(damageAmmount);
    
    }
}
