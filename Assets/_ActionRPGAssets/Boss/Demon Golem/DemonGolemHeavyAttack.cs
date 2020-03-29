using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGolemHeavyAttack : MonoBehaviour
{

    public DemonGolem bossComponent;

    private void Awake()
    {
        this.gameObject.GetComponentInParent<DemonGolem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (bossComponent.BossActive && other.GetComponent<Idamageable>() != null)
        {
            bossComponent.DealDamage(bossComponent.HeavyAttackDamage, other.GetComponent<Idamageable>());
        }
    }
}
