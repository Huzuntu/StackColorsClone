using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;
    [SerializeField] TMP_Text m_scoreDisplay;

    [SerializeField] TMP_Text m_levelDisplay;

    int score;

    private void Awake() {
        m_levelDisplay.text = "Level 1";
        DontDestroyOnLoad(this);
        if(instance == null)
        {
            instance = this;
        }
    }

    public void IncreaseLevel(int currentSceneIndex)
    {
        m_levelDisplay.text = "Level " + currentSceneIndex;
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        m_scoreDisplay.text = score.ToString();
    }
}
