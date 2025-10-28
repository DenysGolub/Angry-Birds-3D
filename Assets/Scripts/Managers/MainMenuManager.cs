using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AngryBirds.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _settingsMenu;
        
        [SerializeField]
        private Button _settingsMenuBtn;
    
        [SerializeField]
        private GameObject _highScoreMenu;
        
        [SerializeField]
        private Button _highScoreBtn;
    
        [SerializeField]
        private GameObject _mainMenu;
        
        [SerializeField]
        private Button _mainMenuBtn;

        [SerializeField]
        private TextMeshProUGUI _highScoreLevelOne;
    
        [SerializeField]
        private TextMeshProUGUI _highScoreLevelTwo;
    
        [SerializeField]
        private TextMeshProUGUI _highScoreLevelThree;

        private void Start()
        {
            _highScoreLevelOne.text = LevelScores.GetHighScore("Level_1").ToString();
            _highScoreLevelTwo.text = LevelScores.GetHighScore("Level_2").ToString();
            _highScoreLevelThree.text = LevelScores.GetHighScore("Level_3").ToString();
        }

        private void OnEnable()
        {
            _mainMenuBtn.onClick.AddListener(ToMainMenu);
            _settingsMenuBtn.onClick.AddListener(ToSettingsMenu);
            _highScoreBtn.onClick.AddListener(ToHighScoreMenu);
        }
        private void OnDisable()
        {
            _mainMenuBtn.onClick.RemoveListener(ToMainMenu);
            _settingsMenuBtn.onClick.RemoveListener(ToSettingsMenu);
            _highScoreBtn.onClick.RemoveListener(ToHighScoreMenu);
        }

        public void ToMainMenu()
        {
            _mainMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _highScoreMenu.SetActive(false);
            _mainMenuBtn.gameObject.SetActive(false);
        }

        public void ToSettingsMenu()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(true);
            _highScoreMenu.SetActive(false);
            _mainMenuBtn.gameObject.SetActive(true);
        }

        public void ToHighScoreMenu()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _highScoreMenu.SetActive(true);
            _mainMenuBtn.gameObject.SetActive(true);
        }
    }
}
