using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float weaponDmg;
    private float newWeaponDmg;
    [SerializeField]
    EnemyHealth enemy;
    CharacterControls playerRef;
    public bool isAbility;

    private void Start()
    {
        playerRef = gameObject.GetComponentInParent<CharacterControls>();
        if (isAbility)
        {
            newWeaponDmg = weaponDmg * 4;
            weaponDmg = newWeaponDmg;
        }
    }

    private void Update()
    {
        print(weaponDmg + "Weapon DAMAGE");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            enemy = other.gameObject.GetComponent<EnemyHealth>();
            DealDamage(weaponDmg);
            playerRef.IncreasePower(playerRef.specialPowerIncreasePerHit);
            //Debug.Log(weaponDmg + "Weapon Damage");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            playerRef.IncreasePower(0);
        }
    }


    public void DealDamage(float damageAmmount)
    {
        enemy.TakeDamage(damageAmmount);

    }
}
