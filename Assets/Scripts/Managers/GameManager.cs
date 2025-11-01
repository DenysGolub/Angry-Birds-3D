using System;
using System.Collections;
using AngryBirds.Blocks;
using AngryBirds.Levels;
using AngryBirds.SO.Scripts;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static event Action<int> OnScoreChanged;
        public static event Action<bool> OnGameOver;
        public static event Action OnNextBirdChanged;
        public static event Action<GameObject> SetNextBirdToSlingshotAction;
        
        [Header("Score")]
        private int _score = 0;
        private int _enemyCount;
        private int _birdCount;
        
        [SerializeField] private MoveCamera _camera;

        private const int POINTS_PER_UNUSED_BIRD = 10000;
        
        private void Start() 
        {
            Time.timeScale = 1f;
        }
    
        private void OnEnable()
        {
            Slingshot.OnShotFired += RequestNextBird;
            NetworkSlingshot.OnShotFired += RequestNextBird;

            BirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
            BirdManager.SetAmmo += GetStartingBirdsCount;
            BirdManager.OnEmptyAmmo += CheckGameStatus;
        
            NetworkBirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
            NetworkBirdManager.SetAmmo += GetStartingBirdsCount;
            NetworkBirdManager.OnEmptyAmmo += CheckGameStatus;

            
            Enemy.Enemy.AddEnemyCount += ChangeEnemyCount;
            Enemy.Enemy.OnEnemyDeath += UpdateScore;
            Enemy.Enemy.OnEnemyDeath += DecreaseEnemyCount;
            Enemy.Enemy.OnHealthChange += UpdateScore;
        
            Block.OnBlockDestroyed += UpdateScore;
            Block.OnHealthChanged += UpdateScore;
        }
    
        private void OnDisable()
        {
            Slingshot.OnShotFired -= RequestNextBird;
            NetworkSlingshot.OnShotFired -= RequestNextBird;
            BirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
            BirdManager.OnEmptyAmmo -= CheckGameStatus;
            BirdManager.SetAmmo -= GetStartingBirdsCount;
            
            NetworkBirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
            NetworkBirdManager.OnEmptyAmmo -= CheckGameStatus;
            NetworkBirdManager.SetAmmo -= GetStartingBirdsCount;
        
            Enemy.Enemy.AddEnemyCount -= ChangeEnemyCount;
            Enemy.Enemy.OnEnemyDeath -= UpdateScore;
            Enemy.Enemy.OnEnemyDeath -= DecreaseEnemyCount;
            Enemy.Enemy.OnHealthChange -= UpdateScore;

            Block.OnBlockDestroyed -= UpdateScore;
            Block.OnHealthChanged -= UpdateScore;
        }
    
        private void GetStartingBirdsCount(BirdsAmmoSO obj)
        {
            _birdCount = obj.Birds.Count;
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

        private void RequestNextBird()
        {
            OnNextBirdChanged?.Invoke();
            _birdCount--;
        }

        private void SetNextBirdToSlingshot(GameObject bird)
        {
            SetNextBirdToSlingshotAction?.Invoke(bird);
        }
    }
}
