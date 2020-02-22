
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactionRadius;
    [SerializeField]
   // CharacterController Player;
    


    private void Start()
    {
       // Player = FindObjectOfType<CharacterController>();
    }


    public virtual void Interact()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Update()
    {
       // float distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);

       // if (distanceFromPlayer <= interactionRadius) 
        //{
         //   Player.canPickUp = true;
        
       // }
      //  else 
       // {
       //     Player.canPickUp = false;
        
       // }

       
       // print("Can pick up" + Player.canPickUp);
    }



}
