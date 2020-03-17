using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Weapon", menuName = "ItemPickup/Weapon Pickup", order = 0)]
public class WeaponPickup : ItemPickup {
    
//public String ItemName;
    
[Range(1,20)]
public float DamagePerHit;
[Range(0.1f,3)]
public float AttacksPerSecond;


}
