using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    private int _score = 0;
    
    public static event Action<int> OnScoreChanged;
    
    public static event Action<bool> OnGameOver;
    public static event Action OnGameStarted;

    public static event Action OnNextBirdChanged;
    public static event Action<GameObject> SetNextBirdToSlingshotAction;

    private int _enemyCount;
    private int _birdCount;

    private void Start()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _birdCount = GameObject.FindGameObjectsWithTag("Player").Length;
        Debug.Log($"Bird count: {_birdCount}");
    }

    void OnEnable()
    {
        Slingshot.OnShotFired += RequestNextBird;
        BirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
        Enemy.OnEnemyDeath += UpdateScore;
        Enemy.OnEnemyDeath += DecreaseEnemyCount;
        Enemy.OnHealthChange += UpdateScore;

        Block.OnBlockDestroyed += UpdateScore;
        Block.OnHealthChanged += UpdateScore;
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
                    _score += _birdCount * 10000;
                    OnScoreChanged.Invoke(_score);
                }
                OnGameOver.Invoke(true);
            } 
        }
    }
    
    void OnDisable()
    {
        Slingshot.OnShotFired -= RequestNextBird;
        BirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
        Enemy.OnEnemyDeath -= UpdateScore;
        Enemy.OnHealthChange -= UpdateScore;
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
