using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CinemachineCamera LevelCamera;
    public CinemachineCamera ShotCamera;

    public GameObject Slingshot;

    public void ChangeCamera()
    {
        ShotCamera.gameObject.SetActive(!ShotCamera.isActiveAndEnabled);
        LevelCamera.gameObject.SetActive(!LevelCamera.isActiveAndEnabled);
        Slingshot.GetComponent<Slingshot>().enabled = ShotCamera.isActiveAndEnabled;
    }
}
