using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementy : MonoBehaviour
{

    ScoreBoard _scoreBoard;

    void Start()
    {
        _scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    public static int getCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        _scoreBoard.IncreaseLevel(nextSceneIndex);
        PlayerMovement.isCheer = false;
        PlayerMovement.isOnEndLine = false;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
    public void InvokeRestartScene(float secondssToInvoke)
    {
        Invoke("RestartScene", secondssToInvoke);
    }
    public void InvokeNextLevel(float secondsToInvoke)
    {
        Invoke("NextLevel", secondsToInvoke);
    }
}
