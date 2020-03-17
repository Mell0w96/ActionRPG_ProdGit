
using UnityEngine;

public class HealthPickup : Interactable
{
    CharacterController player;
    public float healthRegenAmnt;
    

    private void Start()
    {
        player = FindObjectOfType < CharacterController >();
    }

    public override void Interact()
    {
        Pickup();
    }

    void Pickup()
    {

        player.currentHealth += healthRegenAmnt;
        
      
        Destroy(this.gameObject);
     
    }

}
