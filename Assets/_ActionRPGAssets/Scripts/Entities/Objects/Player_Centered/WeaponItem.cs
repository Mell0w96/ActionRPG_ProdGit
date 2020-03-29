using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory / Weapon")]
public class WeaponItem : Item
{    
    public int PlayerState;
    public bool isSpecialWeapon;    
    [Range(1, 100)]
    public float weaponDmg;
    [Range(0f, 5f)]
    public float AttackSpdMultiplier;
    public SpecialWeaponBase SpecialWeapon;
    public Transform AbilityPoint;


    public void PerformAbility()
    {
        if (isSpecialWeapon)
        {
            
            SpecialWeapon.MainAbility();
        }
        
    }

}
