
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
   

    public override void Interact()
    {
        Pickup();
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.itemName);
        // add item to inventory
        bool itemPickedUp = Inventory.instance.Add(item);
        if (itemPickedUp)
        {
            Destroy(this.gameObject);
        }
    }

}
