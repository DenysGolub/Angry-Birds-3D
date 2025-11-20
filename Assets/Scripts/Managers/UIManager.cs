using System.Threading.Tasks;
using AngryBirds.Network;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AngryBirds.Managers
{
    public class UIManager : MonoBehaviour
    {
        //TODO: sync points between players
        private int _score;
    
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _gameOverMenu;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private GameObject _pauseMenu;

        [SerializeField] private Button _pauseBtn;
        [SerializeField] private Button _closePauseBtn;
        [SerializeField] private Button _changeCameraBtn;

        [SerializeField] private Button _toMainMenuBtn;
        [SerializeField] private Button _replayBtn;
        
        [SerializeField] CameraManager _cameraManager;
        [SerializeField] SceneLoader _sceneLoader;

        private void OnEnable()
        {
            
            NetworkGameManager.OnScoreChanged += UpdateScore;
            NetworkGameManager.OnGameOver += ShowGameOverMenu;
            
            _pauseBtn.onClick.AddListener(() => SetActivePauseMenu(true));
            _closePauseBtn.onClick.AddListener(() => SetActivePauseMenu(false));
            _changeCameraBtn.onClick.AddListener(_cameraManager.ChangeCamera);
            _toMainMenuBtn.onClick.AddListener(_sceneLoader.LoadMainMenu);
            _replayBtn.onClick.AddListener(_sceneLoader.ReplayLevel);

        }
    
        private void OnDisable()
        {
            NetworkGameManager.OnScoreChanged -= UpdateScore;
            NetworkGameManager.OnGameOver -= ShowGameOverMenu;
            
            _pauseBtn.onClick.RemoveAllListeners();
            _closePauseBtn.onClick.RemoveAllListeners();
            _changeCameraBtn.onClick.RemoveAllListeners();
            _toMainMenuBtn.onClick.RemoveAllListeners();
            _replayBtn.onClick.RemoveAllListeners();
        }
        
       
        private void ShowGameOverMenu(bool isWin)
        {
            
            Debug.Log(isWin);
            _gameOverMenu.SetActive(true);
        
            _gameOverText.text = isWin ? "You win!" : "You lose!";
            LevelScores.SetHighScore(SceneManager.GetActiveScene().name, _score);
        }

        private void SetActivePauseMenu(bool isActive)
        {
            _pauseMenu.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;
        }
        
        private void UpdateScore(int newPoints)
        {
            _scoreText.text = $"Score: {newPoints}";
        }
    }
}
