using System;
using UnityEngine;

public abstract class BirdBase : MonoBehaviour
{
    public int PointsForNotUsing;
   
    private protected Rigidbody _rb;

    private protected bool _isFlying = false;
 
    private protected bool _hasPowerUsed = false;

    public abstract void PlaySoundEffect();
    public abstract void UseSpecialAbility();
    public Action OnShoot;

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
    }

}
