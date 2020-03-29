using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialWeaponBase : ScriptableObject
{
    public GameObject abilityGFX;
    public WeaponItem weaponItemRef;
    public abstract void MainAbility();

}
