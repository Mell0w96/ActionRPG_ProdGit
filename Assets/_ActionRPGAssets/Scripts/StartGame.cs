using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void SetScene()
    {
        SceneManager.LoadScene("Level2(Final)");
    }
}
