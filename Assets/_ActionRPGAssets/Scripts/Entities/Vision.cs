using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
  public bool isInVision;    

    

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("IN VISION");
            Debug.Log(isInVision);
            isInVision = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("I'm NOT IN VISION");
            Debug.Log(isInVision);
            isInVision = false;
        }
    }

    



}
