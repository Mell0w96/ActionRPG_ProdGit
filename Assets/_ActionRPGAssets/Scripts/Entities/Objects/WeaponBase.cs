using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : Interactable
{
    public int setPlayerState;
    public bool setAsSpecial;    
    public float setAttackDmg;
    public float setAttackSpeed;
    public WeaponItem weapon;
    public WeaponDamage WeaponHitBox;
    public Transform ActuationPoint;
    public Transform PlayerItemSlot;
    public Collider WeaponCollider;
    private Rigidbody weaponRB;
    public bool canActivate;
    


    private void Start()
    {
        setPlayerState = weapon.PlayerState;        
        setAsSpecial = weapon.isSpecialWeapon;
        setAttackDmg = weapon.weaponDmg;
        setAttackSpeed = weapon.AttackSpdMultiplier;
        WeaponHitBox = GetComponentInChildren<WeaponDamage>();
        WeaponHitBox.weaponDmg = setAttackDmg;
        WeaponCollider = GetComponent<Collider>();
        weaponRB = GetComponent<Rigidbody>();
        weapon.AbilityPoint = ActuationPoint;

        //PlayerItemSlot = Player.gameObject.transform.Find("WeaponHolder");

    }

    public void Update()
    {
        // this is for debugging
        if (Player.canPickUp)
        {
            if (PlayerItemSlot != null)
            {
                Debug.Log("Item Slot Found");
            }
            else
            {
                Debug.Log("Item Slot Not Found");
            }
        }
        
    }

    public override void Interact()
    {
        SetWeaponPositionToPlayerSlot();
    }

   
    void SetWeaponPositionToPlayerSlot()
    {
        if (Player.Weapon == null)
        {
            transform.SetParent(PlayerItemSlot.transform);
            transform.position = PlayerItemSlot.transform.position;
            transform.rotation = PlayerItemSlot.transform.rotation;
            WeaponCollider.enabled = false;
            weaponRB.isKinematic = true;
            weaponRB.detectCollisions = false;

        }
        else
        {
            return;
        }


    }

    public void DropWeapon()
    {
        WeaponCollider.enabled = true;
        weaponRB.isKinematic = false;
        weaponRB.detectCollisions = true;
    }

    public void PerfromMainAbility()
    {
        weapon.PerformAbility();
    }
}
