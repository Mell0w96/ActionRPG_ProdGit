﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float weaponDmg;
    [SerializeField]
    EnemyHealth enemy;
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            enemy = other.gameObject.GetComponent<EnemyHealth>();
            DealDamage(weaponDmg);
        }
    }
   

    public void DealDamage(float damageAmmount)
    {
        enemy.TakeDamage(damageAmmount);

    }
}