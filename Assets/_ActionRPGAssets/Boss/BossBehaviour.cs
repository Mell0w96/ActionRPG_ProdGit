using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBehaviour : MonoBehaviour
{
    [Tooltip("Make Sure The Arena Has A Trigger Collider on The Arena Entrance")]
  

    [HideInInspector] public bool BossActive = false;
    [HideInInspector] public GameObject playerTarget;





    public void ActivateBoss(GameObject ArenaTrigger)
    {
        if(BossActive == false)
        {
            BossActive = true;
            ArenaTrigger.gameObject.SetActive(false);
        }
    }
}
