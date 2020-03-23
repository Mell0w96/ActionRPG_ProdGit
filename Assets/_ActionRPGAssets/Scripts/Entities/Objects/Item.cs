using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Inventory / Item")]
public class Item : ScriptableObject    
{
    public string itemName = "New Name";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public GameObject itemModel;    
    public enum ItemType { InteractiveObject, Consumable, Weapon, Equipment }
    public ItemType typeOfItem;  
    
    

}
