using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{

    public BossBehaviour TargetBoss;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TargetBoss.playerTarget = other.gameObject;
            TargetBoss.ActivateBoss(this.gameObject);
        }
    }

}
