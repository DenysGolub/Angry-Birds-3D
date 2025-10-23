using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public abstract class BirdBase : MonoBehaviour
{
   
    private protected Rigidbody rb;
    private protected bool isFlying = false;
    private protected bool hasPowerUsed = false;
    private protected BirdType _birdType;
    
    public Action OnShoot;
    
    public abstract void UseSpecialAbility();

    public CinemachineCamera FlightCamera;
    public BirdType BirdType => _birdType;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        OnShoot += SetFlying;
    }

    private void OnDisable()
    {
        OnShoot -= SetFlying;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasPowerUsed && isFlying)
        {
            Debug.Log(isFlying);
            UseSpecialAbility();
            hasPowerUsed = true;
            isFlying = false;
        }

        if (hasPowerUsed && !isFlying)
        {
            StartCoroutine(DestroyBird());
        }
    }
    
    void SetFlying()
    {
        PlayFlyingSoundEffect();
        Debug.Log("Got call to set flying");
        isFlying = true;
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
