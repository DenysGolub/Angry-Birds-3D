using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverMenu;
    public TextMeshProUGUI GameOverText;
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

    void UpdateScore(int points)
    {
        ScoreText.text = $"Score: {points}";
    }

    void PauseButton()
    {
        
    }

    void ResumeButton()
    {
        
    }

    void ShowGameOverMenu(bool isWin)
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
    }
    
    
}
