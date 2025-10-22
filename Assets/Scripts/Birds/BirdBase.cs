using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public abstract class BirdBase : MonoBehaviour
{
    const int PointsForNotUsing = 10000;
   
    private protected Rigidbody _rb;

    private protected bool _isFlying = false;
 
    private protected bool _hasPowerUsed = false;

    public abstract void UseSpecialAbility();
    public Action OnShoot;
    private protected BirdType _birdType;
    [Header("Flight Camera Setup")] public CinemachineCamera FlightCamera;

    public BirdType BirdType => _birdType;

    private void OnEnable()
    {
        OnShoot += SetFlying;
    }

    private void OnDisable()
    {
        OnShoot -= SetFlying;
    }
    
    void SetFlying()
    {
        PlayFlyingSoundEffect();
        Debug.Log("Got call to set flying");
        _isFlying = true;
    }
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_hasPowerUsed && _isFlying)
        {
            Debug.Log(_isFlying);
            UseSpecialAbility();
            _hasPowerUsed = true;
            _isFlying = false;
        }

        if (_hasPowerUsed && !_isFlying)
        {
            StartCoroutine(DestroyBird());
        }
    }

    public IEnumerator DestroyBird()
    {
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlayBirdDeath();
        Destroy(gameObject);
    }
    public void PlayFlyingSoundEffect()
    {
        AudioManager.Instance.PlayBirdLaunch(_birdType);
    }
}
