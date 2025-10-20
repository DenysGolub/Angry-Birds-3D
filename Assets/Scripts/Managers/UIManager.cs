using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
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
    
    
}
