using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : Interactable
{
    public int setPlayerState;
    public bool setAsSpecial;
    public GameObject ability;
    public float setAttackDmg;
    public float setAttackSpeed;
    public WeaponItem weapon;
    public WeaponDamage WeaponHitBox;


    private void Start()
    {
        setPlayerState = weapon.PlayerState;
        setAsSpecial = weapon.isSpecialWeapon;
        setAttackDmg = weapon.weaponDmg;
        setAttackSpeed = weapon.AttackSpdMultiplier;
        WeaponHitBox = GetComponentInChildren<WeaponDamage>();
        WeaponHitBox.weaponDmg = setAttackDmg;
    }
}
