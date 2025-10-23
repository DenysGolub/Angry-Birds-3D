using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsMenu;
    public GameObject HighScoreMenu;
    public GameObject MainMenu;

    public TextMeshProUGUI HighScoreLevelOne;
    public TextMeshProUGUI HighScoreLevelTwo;
    public TextMeshProUGUI HighScoreLevelThree;

    void Start()
    {
        HighScoreLevelOne.text = LevelScores.GetHighScore("Level_1").ToString();
        HighScoreLevelTwo.text = LevelScores.GetHighScore("Level_2").ToString();
        HighScoreLevelThree.text = LevelScores.GetHighScore("Level_3").ToString();
    }
    
    public void ToMainMenu()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        HighScoreMenu.SetActive(false);
    }

    public void ToSettingsMenu()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        HighScoreMenu.SetActive(false);
    }

    public void ToHighScoreMenu()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        HighScoreMenu.SetActive(true);
    }
    
}
