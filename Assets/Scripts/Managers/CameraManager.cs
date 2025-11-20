using System;
using AngryBirds.Levels;
using Unity.Cinemachine;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static event Action<bool> EnableSlingshot;
        
        [SerializeField] private CinemachineCamera _levelCamera;
        [SerializeField] private CinemachineCamera _shotCamera;
    
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
                EnableSlingshot?.Invoke(_shotCamera.isActiveAndEnabled);
            }
        }

    }
}