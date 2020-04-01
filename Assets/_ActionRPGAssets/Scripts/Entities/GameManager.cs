using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    #region GM Singleton
    public static GameManager GMinstance;

    private void Awake()
    {
        if (GMinstance != null)
        {
            Debug.LogWarning("THERE IS MORE THAN ONE GAME MANAGER HERE BRO");
            return;
        }

        GMinstance = this;

        StartCoroutine(WaitToUpdateKillCount());
    }

    #endregion

    int numberOfEnemies;  


    public void EnemyDied()
    {
        numberOfEnemies++;
    }

    IEnumerator WaitToUpdateKillCount()
    {
        while (this.isActiveAndEnabled)
        {
            KillCounter();
            yield return new WaitForSeconds(30);
        }
        
       
    }


    void KillCounter()
    {
        Analytics.CustomEvent("CountingEnemyDeaths", new Dictionary<string, object>
        {

            {"Number of Enemies Dead", numberOfEnemies },
            {"CheckInTime", Time.timeSinceLevelLoad}
            });
    }
}
