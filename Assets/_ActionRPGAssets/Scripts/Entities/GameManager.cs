using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    public Canvas PauseScreenCanvas;
    public CameraController cameraControllerComponent;
    //public Scene TitleScene;
    bool PauseScreenActive = false;

    public void EnemyDied()
    {
        numberOfEnemies++;
    }

    IEnumerator WaitToUpdateKillCount()
    {
        while (this.gameObject.activeSelf)
        {
            KillCounter();
            yield return new WaitForSeconds(30);
        }
        
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseScreenActive)
            {
                PauseScreenCanvas.gameObject.SetActive(true);
                cameraControllerComponent.IsActive = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseScreenActive = true;
            } else if (PauseScreenActive)
            {
                PauseScreenCanvas.gameObject.SetActive(false);
                cameraControllerComponent.IsActive = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PauseScreenActive = false;
            }
            
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

    public void quitGame()
    {
        Application.Quit();
    }
    
    public void restartGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
