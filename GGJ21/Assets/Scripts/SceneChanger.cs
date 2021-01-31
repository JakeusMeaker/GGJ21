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
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void WinScene()
    {
        SceneManager.LoadScene("SuccessScene", LoadSceneMode.Single);
    }

    public void FailScene()
    {
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

}
