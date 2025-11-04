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
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private NetworkBirdManager _birdManager;

        [SerializeField] private BirdsAmmoSO firstPlayerAmmo;
        [SerializeField] private BirdsAmmoSO secondPlayerAmmo;

        public BirdsAmmoSO GetAmmoForIndex(int index)
        {
            return index == 0 ? firstPlayerAmmo : secondPlayerAmmo;
        }
        
        public void AssignSlingshot(PlayerRef targetPlayer, int index, NetworkObject slingshot)
        {
            Debug.Log($"Assign slingshot: {Runner.LocalPlayer}, {targetPlayer}");

            if (targetPlayer == Runner.LocalPlayer)
            {
                if (_cameraManager != null && slingshot != null)
                {
                    Debug.Log($"[LOCAL] Assigned slingshot {index} to local player {targetPlayer}.");
                    slingshot.GetComponent<NetworkSlingshot>().SetAmmo(GetAmmoForIndex(index), targetPlayer);
                    _birdManager.SetNewAmmoAndSlingshot(GetAmmoForIndex(index), slingshot.transform);
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
