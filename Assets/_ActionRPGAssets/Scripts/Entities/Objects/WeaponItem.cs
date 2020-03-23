using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory / Weapon")]
public class WeaponItem : Item
{    
    public int PlayerState;
    public bool isSpecialWeapon;
    public GameObject abilityGFX;
    [Range(1, 20)]
    public float weaponDmg;
    [Range(0f, 5f)]
    public float AttackSpdMultiplier;
}
