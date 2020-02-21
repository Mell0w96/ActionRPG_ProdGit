using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingForInteractables : MonoBehaviour
{
    CharacterController Player;

    private void Start()
    {
        Player = GetComponentInParent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == FindObjectOfType<Interactables>())
        {
            Player.item = other.gameObject.GetComponent<Interactables>();            
        }
    }
}
