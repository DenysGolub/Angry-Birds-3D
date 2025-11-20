using System;
using System.Collections;
using AngryBirds.Blocks;
using AngryBirds.EnemySystem;
using AngryBirds.Levels;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds.Network
{
    public class NetworkGameManager : NetworkBehaviour
    {
        public static event Action<int> OnScoreChanged;
        public static event Action<bool> OnGameOver;
        
        [Networked, OnChangedRender(nameof(EndGame))] private GameResult GameStatus { get; set; }
        [Networked, OnChangedRender(nameof(UpdateUI))] private int Score { get; set; }
        
        [SerializeField] private NetworkSlingshot[] _players = new NetworkSlingshot[2];
        
        private int _enemyCount;
        private int _birdCount;

        private const int POINTS_PER_UNUSED_BIRD = 10000;
        
        private void Start()
        {
            Time.timeScale = 1f;
        }
        
        private void OnEnable()
        {
            NetworkBirdManager.UpdateBirdsCount += UpdateBirdsCount;

            Enemy.AddEnemyCount += ChangeEnemyCount;
            Enemy.OnEnemyDeath += UpdateScore;
            Enemy.OnEnemyDeath += DecreaseEnemyCount;
            Enemy.OnHealthChanged += UpdateScore;

            Block.OnBlockDestroyed += UpdateScore;
            Block.OnHealthChanged += UpdateScore;
        }

        private void OnDisable()
        {
            NetworkBirdManager.UpdateBirdsCount -= UpdateBirdsCount;

            Enemy.AddEnemyCount -= ChangeEnemyCount;
            Enemy.OnEnemyDeath -= UpdateScore;
            Enemy.OnEnemyDeath -= DecreaseEnemyCount;
            Enemy.OnHealthChanged -= UpdateScore;

            Block.OnBlockDestroyed -= UpdateScore;
            Block.OnHealthChanged -= UpdateScore;
        }

        public NetworkSlingshot GetPlayer(int index) => _players[index];

        public bool IsPlayerSpawned(int index) => _players[index] != null;
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void SetPlayerRpc(int index, NetworkSlingshot slingshot)
        {
            SetPlayer(index, slingshot);
        }

        private void ChangeEnemyCount() => _enemyCount++;

        private void UpdateBirdsCount(int delta)
        {
            _birdCount += delta;
            if (_birdCount <= 0)
            {
                StartCoroutine(WaitForEndLevel());
            }
        }

        private void SetPlayer(int index, NetworkSlingshot slingshot)
        {
            _players[index] = slingshot;
        }
        
        private IEnumerator WaitForEndLevel()
        {
            yield return new WaitForSeconds(5f);
            if (_enemyCount > 0)
            {
                GameStatus = GameResult.Lose;
            }
        }

        private void EndGame()
        {
            bool isWin = GameStatus == GameResult.Win;
            AudioManager.Instance.PlayEndLevel(isWin);
            OnGameOver?.Invoke(isWin);
            SharedModeMasterClientTracker.LocalInstance.DisconnectPlayers();
        }

        private void UpdateScore(int points)
        {
            Score += points;   
        }

        private void UpdateUI()
        {
            OnScoreChanged?.Invoke(Score);
        }

        private void DecreaseEnemyCount(int _)
        {
            _enemyCount--;

            if (_enemyCount == 0)
            {
                GameStatus = GameResult.Win;
                Score += _birdCount * POINTS_PER_UNUSED_BIRD;
            }
        }
    }
}
