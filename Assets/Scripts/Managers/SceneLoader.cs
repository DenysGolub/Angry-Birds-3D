using AngryBirds.Network;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngryBirds.Managers
{
    public class SceneLoader : NetworkBehaviour
    {
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        

        public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        public void ReplayLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
