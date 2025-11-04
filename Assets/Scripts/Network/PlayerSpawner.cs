using System.Threading.Tasks;
using AngryBirds.Levels;
using AngryBirds.Managers;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

namespace AngryBirds
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkEventsHandler _networkEventsHandler;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        [Header("Spawn points")]
        [SerializeField] private Transform _firstPlayerSlingshot;
        [SerializeField] private Transform _secondPlayerSlingshot;

        [Header("Prefabs")]
        [SerializeField] private SpawnPointManager _spawnPointManagerPrefab;

        [SerializeField] private NetworkRunner _runner;
        
        [SerializeField] private NetworkSlingshotManager _networkSlingshotManager;
        [SerializeField] private SpawnPointManager _spawnPointManagerInstance;
        [SerializeField] private CameraManager cameraManager;

        [SerializeField] private GameObject structurePrefab;
        [SerializeField] private Transform structureSpawnPoint;
        
        private void OnEnable()
        {
            Debug.Log("Player subscribed!");
            _networkEventsHandler.OnPlayerJoinedEvent += PlayerJoined;
        }

        private void OnDisable()
        {
            _networkEventsHandler.OnPlayerJoinedEvent -= PlayerJoined;
        }
        
        /*public void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                _spawnPointManagerInstance = Runner.Spawn(
                    _spawnPointManagerPrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    Object.InputAuthority);

                Debug.Log("SpawnPointManager spawned by StateAuthority client.");
                
                
            }
        }*/

        public void PlayerJoined(PlayerRef player)
        {
            _ = PlayerJoinedAsync(player);
        }

        
        public async Task PlayerJoinedAsync(PlayerRef player)
        {
            Debug.Log($"{player}, {_runner.LocalPlayer}");
            if (player != _runner.LocalPlayer)
            {
                return;
            }

            if (_spawnPointManagerInstance == null)
            {
                Debug.Log("Spawn point manager is null!");
                return;
            }

            while (!_spawnPointManagerInstance.IsSpawned)
            {
                await Task.Yield();
            }
            
            while (!_networkSlingshotManager.IsSpawned)
            {
                await Task.Yield();
            }
            
            int index = _spawnPointManagerInstance.IsFree(0) ? 0 : 1;
            Transform spawnPos = index == 0 ? 
                _firstPlayerSlingshot : 
                _secondPlayerSlingshot;
            
            Debug.Log(spawnPos);
            var sling = _runner.Spawn(_playerPrefab, spawnPos.position, _playerPrefab.transform.rotation, player);
        
            sling.GetComponent<NetworkSlingshot>().SetCamera(_cinemachineCamera);

            
            _cinemachineCamera.Follow = spawnPos; //TODO: call method from local player
            _networkSlingshotManager.AssignSlingshot(player, index, sling);
            
            if (index == 0)
            {
                Debug.Log("Structure with pigs is spawned!");
                _runner.Spawn(structurePrefab, structureSpawnPoint.position, structurePrefab.transform.rotation);
            }
            
            _spawnPointManagerInstance.SetSpawnPointUsedRpc(index, true);

           

            //RPC call is don't called when assign slingshot is called first???

        }
    }
}
