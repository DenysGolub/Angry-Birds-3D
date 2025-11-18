using System;
using System.Threading.Tasks;
using AngryBirds.Levels;
using AngryBirds.Managers;
using AngryBirds.SO.Scripts;
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
        [SerializeField] private NetworkBirdManager _birdManagerPrefab;
        [SerializeField] private NetworkRunner _runner;

        [SerializeField] private NetworkSlingshotManager _networkSlingshotManager;
        [SerializeField] private SpawnPointManager _spawnPointManagerInstance;
        [SerializeField] private CameraManager cameraManager;

        [SerializeField] private GameObject structurePrefab;
        [SerializeField] private Transform structureSpawnPoint;
        [SerializeField] private NetworkGameManager _gameManager;

        [SerializeField] private BirdsAmmoSO[] _playersAmmo = new BirdsAmmoSO[2];

        private NetworkObject[] _spawnedSlingshots = new NetworkObject[2];
        private NetworkBirdManager _birdManagerInstance;


        private void OnEnable()
        {
            _networkEventsHandler.OnPlayerJoinedEvent += PlayerJoined;
        }

        private void OnDisable()
        {
            _networkEventsHandler.OnPlayerJoinedEvent -= PlayerJoined;
        }


        public void PlayerJoined(PlayerRef player)
        {
            _ = PlayerJoinedAsync(player);
        }


        public async Task PlayerJoinedAsync(PlayerRef player)
        {
            if (_spawnPointManagerInstance == null)
            {
                Debug.Log("Spawn point manager is null!");
                return;
            }

            while (!_spawnPointManagerInstance.IsSpawned)
                await Task.Yield();

            while (!_networkSlingshotManager.IsSpawned)
                await Task.Yield();

            int index = _spawnPointManagerInstance.IsFree(0) ? 0 : 1;
            Transform spawnPos = index == 0 ? _firstPlayerSlingshot : _secondPlayerSlingshot;

            if (_spawnedSlingshots[index] == null)
            {
                if (player == _runner.LocalPlayer)
                {
                    _spawnedSlingshots[index] = _runner.Spawn(
                        _playerPrefab,
                        spawnPos.position,
                        _playerPrefab.transform.rotation,
                        player
                    );
                }

                if (index == 0 && _spawnPointManagerInstance.IsFree(0))
                {
                    Debug.Log("Structure with pigs is spawned!");
                    _runner.Spawn(structurePrefab, structureSpawnPoint.position, structurePrefab.transform.rotation);
                }

                _spawnedSlingshots[index]
                    .GetComponent<NetworkSlingshot>()
                    .SetCamera(_cinemachineCamera, player);

                _spawnPointManagerInstance.SetSpawnPointUsedRpc(index, true);

                _gameManager.SetPlayerRpc(index,
                    _spawnedSlingshots[index].GetComponent<NetworkSlingshot>());
            }

            if (_runner.IsSharedModeMasterClient)
            {
                await TrySpawnBirdsAfterPlayersReady();
            }
        }


        private async Task TrySpawnBirdsAfterPlayersReady()
        {
            Debug.Log("Master entered!");

            while (!_gameManager.IsPlayerSpawned(1))
            {
                Debug.Log("Waiting for second slingshot to spawn...");
                await Task.Yield();
            }
            
            if (_birdManagerInstance == null)
            {
                Debug.Log("Spawning BirdManager now");
                _birdManagerInstance = _runner.Spawn(_birdManagerPrefab);
            }

            try
            {
                Debug.Log(_gameManager.GetPlayer(0) +"_"+_gameManager.GetPlayer(1));
                SetupBirdManagerRpc(_birdManagerInstance.Object, _gameManager.GetPlayer(0).Object, _gameManager.GetPlayer(1).Object);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            _birdManagerInstance.SpawnBirds();
            SharedModeMasterClientTracker.LocalInstance.BirdManager = _birdManagerInstance;
        }
        
        private void SetupBirdManagerRpc(NetworkObject birdManager, NetworkObject slingshot1, NetworkObject slingshot2)
        {
            var manager = birdManager.GetComponent<NetworkBirdManager>();
            slingshot1.GetComponent<NetworkSlingshot>().SetAmmo(_playersAmmo[0], slingshot1.Id);
            manager.SetPlayerSlingshot(_playersAmmo[0], slingshot1.GetComponent<NetworkSlingshot>());
            slingshot2.GetComponent<NetworkSlingshot>().SetAmmo(_playersAmmo[1], slingshot2.Id);
            manager.SetPlayerSlingshot(_playersAmmo[1], slingshot2.GetComponent<NetworkSlingshot>());
        }

    }
}
