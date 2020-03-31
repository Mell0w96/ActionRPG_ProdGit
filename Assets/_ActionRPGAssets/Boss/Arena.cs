using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Arena : MonoBehaviour
{

    public BossBehaviour TargetBoss;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TargetBoss.playerTarget = other.gameObject;
            TargetBoss.ActivateBoss(this.gameObject);
            AnalyticsEvent();
        }
    }

    void AnalyticsEvent()
    {
        Analytics.CustomEvent("EnteringBossArena", new Dictionary<string, object>
        {
            {"BossType",TargetBoss.gameObject.name },
            {"EntryTime", Time.timeSinceLevelLoad}
        });
    }

}
