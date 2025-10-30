using AngryBirds.Levels;
using AngryBirds.Managers;
using AngryBirds.SO.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

namespace AngryBirds
{
    public class NetworkSlingshotManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _firstSlingshot;
        [SerializeField] private GameObject _secondSlingshot;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private BirdManager _birdManager;

        [SerializeField] private BirdsAmmoSO firstPlayerAmmo;
        [SerializeField] private BirdsAmmoSO secondPlayerAmmo;

        public GameObject GetSlingshotForIndex(int index)
        {
            return index == 0 ? _firstSlingshot : _secondSlingshot;
        }
        
        
        public void AssignSlingshot(PlayerRef targetPlayer, int index)
        {
            Debug.Log($"Assign slingshot: {Runner.LocalPlayer}, {targetPlayer}");

            if (targetPlayer == Runner.LocalPlayer)
            {
                var slingshot = GetSlingshotForIndex(index);
                if (_cameraManager != null && slingshot != null)
                {
                    _cameraManager._slingshot = slingshot;
                    Debug.Log($"[LOCAL] Assigned slingshot {index} to local player {targetPlayer}.");
                    slingshot.GetComponent<Slingshot>().SetAmmo(index == 0 ? firstPlayerAmmo : secondPlayerAmmo);
                    _birdManager.SetNewAmmoAndSlingshot(index == 0 ? firstPlayerAmmo : secondPlayerAmmo, slingshot.transform);
                }
            }
           
            
            
        }
        
        public bool IsSpawned = false;
        public override void Spawned()
        {
            IsSpawned = true;
        }
        

    }
}
