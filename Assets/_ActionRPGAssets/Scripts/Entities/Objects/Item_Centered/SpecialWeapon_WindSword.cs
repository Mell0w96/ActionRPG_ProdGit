using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory / Weapon / SpecialWeapon / WindSword")]
public class SpecialWeapon_WindSword : SpecialWeaponBase
{
    public override void MainAbility()
    {
        
        Instantiate(abilityGFX, weaponItemRef.AbilityPoint.position, weaponItemRef.AbilityPoint.rotation);
    }
}
