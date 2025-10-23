using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int _points;
    
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverMenu;
    public TextMeshProUGUI GameOverText;
    public GameObject PauseMenu;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnGameOver += ShowGameOverMenu;
    }
    
    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnGameOver -= ShowGameOverMenu;
    }

    void UpdateScore(int newPoints)
    {
        ScoreText.text = $"Score: {newPoints}";
        _points = newPoints;
    }

    public void ShowGameOverMenu(bool isWin)
    {
        GameOverMenu.SetActive(true);
        
        if (isWin)
        {
            GameOverText.text = "You win!";
        }
        else
        {
            GameOverText.text = "You lose!";
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        LevelScores.SetHighScore(SceneManager.GetActiveScene().name.ToString(), _points);
    }

    public void SetActivePauseMenu(bool isActive)
    {
        PauseMenu.SetActive(isActive);
        if(isActive)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
