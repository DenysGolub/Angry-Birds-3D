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
        [Header("Network")]
        [SerializeField] private NetworkEventsHandler _networkEventsHandler;
        [SerializeField] private NetworkRunner _runner;

        [Header("Spawn points")]
        [SerializeField] private Transform _firstPlayerSlingshot;
        [SerializeField] private Transform _secondPlayerSlingshot;

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private SpawnPointManager _spawnPointManagerPrefab;
        [SerializeField] private NetworkBirdManager _birdManagerPrefab;
        [SerializeField] private GameObject structurePrefab;
        
        [Header("References")]        
        [SerializeField] private NetworkSlingshotManager _networkSlingshotManager;
        [SerializeField] private SpawnPointManager _spawnPointManagerInstance;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        [SerializeField] private Transform structureSpawnPoint;
        [SerializeField] private NetworkGameManager _gameManager;
        
        [Header("Ammo")]
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


        private void PlayerJoined(PlayerRef player)
        {
            _ = PlayerJoinedAsync(player);
        }


        private async Task PlayerJoinedAsync(PlayerRef player)
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
            while (!_gameManager.IsPlayerSpawned(1))
            {
                await Task.Yield();
            }
            
            if (_birdManagerInstance == null)
            {
                _birdManagerInstance = _runner.Spawn(_birdManagerPrefab);
            }

            SetupAmmoForSlingshots(_gameManager.GetPlayer(0), _gameManager.GetPlayer(1));

            _birdManagerInstance.SpawnBirds();
            SharedModeMasterClientTracker.LocalInstance.BirdManager = _birdManagerInstance;
        }
        
        private void SetupAmmoForSlingshots(NetworkSlingshot firstPlayerSlingshot, NetworkSlingshot secondPlayerSlingshot)
        {
            firstPlayerSlingshot.SetAmmo(_playersAmmo[0], firstPlayerSlingshot.Object.Id);
            _birdManagerInstance.SetPlayerSlingshot(_playersAmmo[0], firstPlayerSlingshot.GetComponent<NetworkSlingshot>());
            
            secondPlayerSlingshot.SetAmmo(_playersAmmo[1], secondPlayerSlingshot.Object.Id);
            _birdManagerInstance.SetPlayerSlingshot(_playersAmmo[1], secondPlayerSlingshot.GetComponent<NetworkSlingshot>());
        }

    }
}
