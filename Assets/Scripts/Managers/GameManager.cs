using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    private int _score = 0;
    
    public static event Action OnScoreChanged;
    
    public static event Action OnGameOver;
    public static event Action OnGameStarted;

    public static event Action OnNextBirdChanged;
    public static event Action<GameObject> SetNextBirdToSlingshotAction;
    
    
    void OnEnable()
    {
        Slingshot.OnShotFired += RequestNextBird;
        BirdManager.ChangeCurrentProjectile += SetNextBirdToSlingshot;
    }

    void OnDisable()
    {
        Slingshot.OnShotFired -= RequestNextBird;
        BirdManager.ChangeCurrentProjectile -= SetNextBirdToSlingshot;
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
