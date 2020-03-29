using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGolemLightAttack : MonoBehaviour
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
            bossComponent.DealDamage(bossComponent.LightAttackDamage, other.GetComponent<Idamageable>());
        }
    }
}
