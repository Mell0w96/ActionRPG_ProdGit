
using UnityEngine;

public class HealthPickup : Interactable
{
    
    public float healthRegenAmnt;
    

    private void Start()
    {
       
    }

    public override void Interact()
    {
        Pickup();
    }

    void Pickup()
    {
        if (Player.currentHealth < Player.totalHealth)
        {
            Player.currentHealth += healthRegenAmnt;

            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("full health");
        }
        
      
     
     
    }

}
