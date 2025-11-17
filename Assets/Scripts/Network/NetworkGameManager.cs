using System;
using System.Collections;
using AngryBirds.Birds;
using AngryBirds.Blocks;
using AngryBirds.Levels;
using AngryBirds.Managers;
using AngryBirds.SO.Scripts;
using Fusion;
using UnityEngine;

namespace AngryBirds.Network
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static event Action<int> OnScoreChanged;
        public static event Action<bool> OnGameOver;

        [Header("Score")] private int _score = 0;
        private int _enemyCount;
        private int _birdCount;

        private const int POINTS_PER_UNUSED_BIRD = 10000;

        [SerializeField] private NetworkSlingshot[] _players = new NetworkSlingshot[2];
        
        private void Start()
        {
            Time.timeScale = 1f;
        }
        

        private void OnEnable()
        {
            // NetworkSlingshot.OnShotFired += RemoveBird;
            // NetworkSlingshot.OnLevelEnter += SetStartBirdCount;
            
            Enemy.Enemy.AddEnemyCount += ChangeEnemyCount;
            Enemy.Enemy.OnEnemyDeath += UpdateScore;
            Enemy.Enemy.OnEnemyDeath += DecreaseEnemyCount;
            Enemy.Enemy.OnHealthChange += UpdateScore;

            Block.OnBlockDestroyed += UpdateScore;
            Block.OnHealthChanged += UpdateScore;

        }

        private void OnDisable()
        {
            
            // NetworkSlingshot.OnShotFired -= RemoveBird;
            // NetworkSlingshot.OnLevelEnter -= SetStartBirdCount;
            Enemy.Enemy.AddEnemyCount -= ChangeEnemyCount;
            Enemy.Enemy.OnEnemyDeath -= UpdateScore;
            Enemy.Enemy.OnEnemyDeath -= DecreaseEnemyCount;
            Enemy.Enemy.OnHealthChange -= UpdateScore;

            Block.OnBlockDestroyed -= UpdateScore;
            Block.OnHealthChanged -= UpdateScore;
        }

        private void SetStartBirdCount(int startCount)
        {
            _birdCount += startCount;
        }

        private void RemoveBird(int obj)
        {
            _birdCount--;
            if (_birdCount == 0)
            {
                CheckGameStatus();
            }
        }
        private void SetPlayer(int index, NetworkSlingshot slingshot)
        {
            _players[index] = slingshot;
        }

        public bool IsPlayerSpawned(int index)
        {
            if (_players[index] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public NetworkSlingshot GetPlayer(int index)
        {
            return _players[index];
        }
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void SetPlayerRpc(int index, NetworkSlingshot slingshot)
        {
            SetPlayer(index, slingshot);
        }
        private void ChangeEnemyCount()
        {
            _enemyCount += 1;
        }

        private void CheckGameStatus()
        {
            StartCoroutine(WaitForEndLevel());
        }

        private IEnumerator WaitForEndLevel()
        {
            yield return new WaitForSeconds(7f);
            if (_enemyCount > 0)
            {
                AudioManager.Instance.PlayEndLevel(false);
                OnGameOver?.Invoke(false);
            }
        }

        private void UpdateScore(int points)
        {
            _score += points;
            OnScoreChanged?.Invoke(_score);
        }

        private void DecreaseEnemyCount(int count)
        {
            _enemyCount--;
            if (_enemyCount == 0)
            {
                AudioManager.Instance.PlayEndLevel(true);
                _score += _birdCount * POINTS_PER_UNUSED_BIRD;
                OnScoreChanged?.Invoke(_score);
                OnGameOver?.Invoke(true);
            }
        }
    }
}
