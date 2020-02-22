using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float weaponDmg;
    EnemyHealth enemy;

    private void Start()
    {
        enemy = FindObjectOfType<EnemyHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            DealDamage(weaponDmg);
        }
    }

    public void DealDamage(float damageAmmount)
    {
        enemy.TakeDamage(damageAmmount);

    }
}
