using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    private int _score = 0;
    
    public static event Action<int> OnScoreChanged;
    
    public static event Action OnGameOver;
    public static event Action OnGameStarted;

    public static event Action OnNextBirdChanged;
    public static event Action<GameObject> SetNextBirdToSlingshotAction;

    private int _enemyCount;

    private void Awake()
    {
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log(_enemyCount);
    }

    void OnEnable()
    {
        Slingshot.OnShotFired += RequestNextBird;
        BirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
        Enemy.OnEnemyDeath += UpdateScore;
    }

    private void UpdateScore(int points)
    {
        
        _score += points;
        if (OnScoreChanged != null)
        {
            OnScoreChanged.Invoke(_score);
        }
    }

    void OnDisable()
    {
        Slingshot.OnShotFired -= RequestNextBird;
        BirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
        Enemy.OnEnemyDeath -= UpdateScore;
    }


    void RequestNextBird()
    {
        if (OnNextBirdChanged != null)
        {
            OnNextBirdChanged.Invoke();
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
