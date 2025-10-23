using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

    void UpdateScore(int points)
    {
        ScoreText.text = $"Score: {points}";
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
