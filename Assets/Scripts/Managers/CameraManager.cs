using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera LevelCamera;
    public CinemachineCamera ShotCamera;
    public GameObject Slingshot;

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
            ShotCamera.gameObject.SetActive(!ShotCamera.isActiveAndEnabled);
            LevelCamera.gameObject.SetActive(!LevelCamera.isActiveAndEnabled);
            Slingshot.GetComponent<Slingshot>().enabled = ShotCamera.isActiveAndEnabled;
        }
    }

}
