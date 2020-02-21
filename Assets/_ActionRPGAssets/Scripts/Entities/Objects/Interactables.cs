
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public float interactionRadius;
    [SerializeField]
    CharacterController Player;


    private void Start()
    {
        Player = FindObjectOfType<CharacterController>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (distanceFromPlayer <= interactionRadius) 
        {
            Player.canPickUp = true;
        
        }
        else 
        {
            Player.canPickUp = false;
        
        }

        print("distanceFromPlayer" + distanceFromPlayer);
        print("Can pick up" + Player.canPickUp);
    }



}
