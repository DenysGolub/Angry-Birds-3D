using AngryBirds.Levels;
using Unity.Cinemachine;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _levelCamera;
        [SerializeField] private CinemachineCamera _shotCamera;
        [SerializeField] private GameObject _slingshot;
    
        private void OnEnable()
        {
            GameManager.OnGameOver += DisableCamera;
        } 
        private void OnDisable()
        {
            GameManager.OnGameOver -= DisableCamera;
        }

        private void DisableCamera(bool obj)
        {
            this.enabled = false;
        }
    
        public void ChangeCamera()
        {
            if (Time.timeScale != 0)
            {
                _shotCamera.gameObject.SetActive(!_shotCamera.isActiveAndEnabled);
                _levelCamera.gameObject.SetActive(!_levelCamera.isActiveAndEnabled);
                _slingshot.GetComponent<Slingshot>().enabled = _shotCamera.isActiveAndEnabled;
            }
        }

    }
}