
using UnityEngine;

public class HealthPickup : Interactable
{
    CharacterMovement player;
    public float healthRegenAmnt;
    

    private void Start()
    {
        player = FindObjectOfType < CharacterMovement >();
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
