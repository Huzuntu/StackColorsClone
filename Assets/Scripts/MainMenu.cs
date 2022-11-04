using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("I have to quit");
        Application.Quit();
    }
    /*
    public void OptionsMenu()
    {
        GameObject.Find("MainMenu").SetActive(false);
        GameObject.Find("OptionMenu").SetActive(true);
    }

    public void BackButton()
    {
        GameObject.Find("MainMenu").SetActive(true);
        GameObject.Find("OptionMenu").SetActive(false);
    }
    */
}
