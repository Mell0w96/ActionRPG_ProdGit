using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("FOUND MORE THAN ONE INVENTORY INSTANCE");
            return;
        }
        instance = this;
    }

    #endregion

    public int invSpace = 3;

    public List<Item> items = new List<Item>();
   

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= invSpace)
            {
                Debug.Log("Inventory Full!!");
                return false;                
            }
           
            items.Add(item);
            
            
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
