using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Checkpoint : MonoBehaviour
{
    public string CheckpointName;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AnalyticsEvent();
        }
    }

    void AnalyticsEvent()
    {
        Analytics.CustomEvent("EnteringCheckpoint", new Dictionary<string, object>
        {
            {"CheckPointName",CheckpointName},
            {"PassTime", Time.timeSinceLevelLoad}
        });
    }
}
