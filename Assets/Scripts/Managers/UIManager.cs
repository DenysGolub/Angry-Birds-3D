using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngryBirds.Managers
{
    public class UIManager : MonoBehaviour
    {
        private int _points;
    
        [SerializeField] 
        private TextMeshProUGUI _scoreText;
    
        [SerializeField] 
        private GameObject _gameOverMenu;
    
        [SerializeField] 
        private TextMeshProUGUI _gameOverText;
    
        [SerializeField] 
        private GameObject _pauseMenu;

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

        public void ShowGameOverMenu(bool isWin)
        {
            _gameOverMenu.SetActive(true);
        
            _gameOverText.text = isWin ? "You win!" : "You lose!";
            Debug.Log(SceneManager.GetActiveScene().name);
            LevelScores.SetHighScore(SceneManager.GetActiveScene().name, _points);
        }

        public void SetActivePauseMenu(bool isActive)
        {
            _pauseMenu.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;
        }
        
        private void UpdateScore(int newPoints)
        {
            _scoreText.text = $"Score: {newPoints}";
            _points = newPoints;
        }


    }
}
