using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void WinScene()
    {
        SceneManager.LoadScene("SucessScene");
    }

    public void FailScene()
    {
        SceneManager.LoadScene("EndScene");
    }

}
