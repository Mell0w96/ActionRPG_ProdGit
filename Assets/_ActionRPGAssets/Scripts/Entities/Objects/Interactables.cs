
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public float interactionRadius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}
