using System;
using Enums;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockSO BlockConfiguration;
    private float _maxHealth;
    private float _damageMultiplier;
    private Rigidbody _rigidbody;
    private float _currentHealth;
    private BlockType _blockType;
    public static Action<int> OnHealthChanged;
    public static Action<int> OnBlockDestroyed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _maxHealth = BlockConfiguration.MaxHealth;
        _damageMultiplier = BlockConfiguration.DamageMultiplier;
        _blockType = BlockConfiguration.Type;
        _currentHealth = _maxHealth;
        
        _rigidbody.mass = BlockConfiguration.Mass;
        _rigidbody.linearDamping = BlockConfiguration.LinearDamping;
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (_currentHealth <= 0)
        {
            return;
        }

        _currentHealth -= other.relativeVelocity.magnitude * _damageMultiplier;
        Debug.Log($"Impact from enter: {other.relativeVelocity.magnitude * _damageMultiplier}");
        Debug.Log($"Impulse from explosion: {other.impulse.magnitude * _damageMultiplier}");
        if (_currentHealth <= 0)
        {
            if (OnBlockDestroyed != null)
            {
                OnBlockDestroyed.Invoke(500);
            }
            AudioManager.Instance.PlayDestroyedBlock(_blockType);
            Destroy(gameObject);
            
        }
        else
        {
            if (OnHealthChanged != null)
            {
                OnHealthChanged.Invoke((int)Math.Round(other.relativeVelocity.magnitude  * 100f));
                Debug.Log($"Points: {(int)Math.Round(other.relativeVelocity.magnitude * 100f)}");
            }
        }
    }
}
