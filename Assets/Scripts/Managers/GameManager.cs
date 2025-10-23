using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    private int _score = 0;
    private int _enemyCount;
    private int _birdCount;
    private const int POINTS_PER_UNUSED_BIRD = 10000;
    
    public MoveCamera Camera;
    public Slingshot Slingshot;
    
    public static event Action<int> OnScoreChanged;
    public static event Action<bool> OnGameOver;
    public static event Action OnNextBirdChanged;
    public static event Action<GameObject> SetNextBirdToSlingshotAction;
  
    void OnEnable()
    {
        Slingshot.OnShotFired += RequestNextBird;
        
        BirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
        BirdManager.SetAmmo += GetStartingBirdsCount;
        BirdManager.OnEmptyAmmo += CheckGameStatus;
        
        Enemy.AddEnemyCount += ChangeEnemyCount;
        Enemy.OnEnemyDeath += UpdateScore;
        Enemy.OnEnemyDeath += DecreaseEnemyCount;
        Enemy.OnHealthChange += UpdateScore;
        
        Block.OnBlockDestroyed += UpdateScore;
        Block.OnHealthChanged += UpdateScore;

    }
    
    void OnDisable()
    {
        Slingshot.OnShotFired -= RequestNextBird;
        
        BirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
        BirdManager.OnEmptyAmmo -= CheckGameStatus;
        BirdManager.SetAmmo -= GetStartingBirdsCount;
        
        Enemy.AddEnemyCount -= ChangeEnemyCount;
        Enemy.OnEnemyDeath -= UpdateScore;
        Enemy.OnEnemyDeath -= DecreaseEnemyCount;
        Enemy.OnHealthChange -= UpdateScore;

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
            if (OnGameOver != null)
            {
                AudioManager.Instance.PlayEndLevel(false);
                OnGameOver.Invoke(false);
            }
        }
    }
    

    private void UpdateScore(int points)
    {
        _score += points;
        if (OnScoreChanged != null)
        {
            OnScoreChanged.Invoke(_score);
        }
    }

    private void DecreaseEnemyCount(int count)
    {
        _enemyCount--;
        if (_enemyCount == 0)
        {
            if (OnGameOver != null)
            {
                AudioManager.Instance.PlayEndLevel(true);
                if (OnScoreChanged != null)
                {
                    _score += _birdCount * POINTS_PER_UNUSED_BIRD;
                    OnScoreChanged.Invoke(_score);
                }
                OnGameOver.Invoke(true);
            } 
        }
    }

    void RequestNextBird()
    {
        if (OnNextBirdChanged != null)
        {
            OnNextBirdChanged.Invoke();
            _birdCount--;
        }
    }

    void SetNextBirdToSlingshot(GameObject bird)
    {
        if (SetNextBirdToSlingshotAction != null)
        {
            SetNextBirdToSlingshotAction.Invoke(bird);
        }
    }
}
