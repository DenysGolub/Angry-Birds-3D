using System.Linq;
using System.Threading.Tasks;
using AngryBirds.Levels;
using AngryBirds.Managers;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

namespace AngryBirds.Network
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
        [SerializeField]
        private Transform structureSpawnPoint;

        private NetworkObject[] sling = new NetworkObject[2];        
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
            Debug.Log($"{player.AsIndex}, {_runner.LocalPlayer.AsIndex}");


            //if (player != _runner.LocalPlayer)
            //{
            //  return;
            //}

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
            Transform spawnPos = index == 0 ? _firstPlayerSlingshot : _secondPlayerSlingshot;


         
            if (sling[index] == null)
            {
                if (player == _runner.LocalPlayer)
                {
                    sling[index] = _runner.Spawn(_playerPrefab, spawnPos.position, _playerPrefab.transform.rotation, player);
                }
                
                sling[index].GetComponent<NetworkSlingshot>().SetCamera(_cinemachineCamera, player);


                if (index == 0 && _spawnPointManagerInstance.IsFree(0))
                {
                    Debug.Log("Structure with pigs is spawned!");
                                   
                    _runner.Spawn(structurePrefab, structureSpawnPoint.position, structurePrefab.transform.rotation);
                }

                if (_runner.LocalPlayer.AsIndex == 1)
                {
                    _networkSlingshotManager.SpawnBirds(index, sling[index], player);
                }
                _spawnPointManagerInstance.SetSpawnPointUsedRpc(index, true);
            }
            
            
        }
    }
}
