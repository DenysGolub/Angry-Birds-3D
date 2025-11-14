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

		[SerializeField]
		private NetworkBirdManager _birdManagerPrefab;

		public BirdsAmmoSO GetAmmoForIndex(int index)
		{
			return index == 0 ? firstPlayerAmmo : secondPlayerAmmo;
		}
        
		// public void AssignSlingshot(PlayerRef targetPlayer, int index, NetworkObject slingshot)
		// {
		// 	Debug.Log($"Assign slingshot: {Runner.LocalPlayer}, {targetPlayer}");
		//
		// 	if (targetPlayer == Runner.LocalPlayer)
		// 	{
		// 		if (_cameraManager != null && slingshot != null)
		// 		{
		// 			Debug.Log($"[LOCAL] Assigned slingshot {index} to local player {targetPlayer}.");
		// 			slingshot.GetComponent<NetworkSlingshot>().SetAmmo(GetAmmoForIndex(index), targetPlayer);
		// 		}
		// 	}
		// }

		public void SpawnManager(NetworkId id, int index, NetworkObject slingshot, PlayerRef targetPlayer)
		{
			_birdManager = Runner.Spawn(_birdManagerPrefab);
		}
		public void SetBirds(NetworkId id, int index, NetworkObject slingshot, PlayerRef targetPlayer)
		{
			_birdManager.SetNewAmmoAndSlingshot(GetAmmoForIndex(index), slingshot.transform, targetPlayer);
		}
		
		public void SpawnBirds(NetworkId id, int index, NetworkObject slingshot, PlayerRef targetPlayer)
		{
			//SetBirds(id, index, slingshot, targetPlayer);
			slingshot.GetComponent<NetworkSlingshot>().SetAmmo(GetAmmoForIndex(index), targetPlayer, id);
		}
		
		
        
		[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
		public void SpawnBirdsRpc(NetworkId id, int index, NetworkObject slingshot, PlayerRef owner)
		{
			SpawnBirds(id, index, slingshot, owner);
		}
        
		public bool IsSpawned = false;
		
		public override void Spawned()
		{
			IsSpawned = true;
		}
        

	}
}