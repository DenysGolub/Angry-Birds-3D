using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 20f;
    public float DamageMultiplier = 30f;
    
    private float _currentHealth;
    
    public static event Action<int> OnEnemyDeath;
    public static event Action<int> OnHealthChange;
    private void Start()
    {
        _currentHealth = MaxHealth;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_currentHealth <= 0)
        {
            return;
        }

        _currentHealth -= other.relativeVelocity.magnitude * DamageMultiplier;
        Debug.Log($"Impact from enter: {other.relativeVelocity.magnitude * DamageMultiplier}");
        Debug.Log($"Impulse from explosion: {other.impulse.magnitude * DamageMultiplier}");
        if (_currentHealth <= 0)
        {
            if (OnEnemyDeath != null)
            {
                OnEnemyDeath.Invoke(1000);
            }
            AudioManager.Instance.PlayPigDeath();
            Destroy(gameObject);
            
        }
        else
        {
            if (OnHealthChange != null)
            {
                OnHealthChange.Invoke((int)Math.Round(other.relativeVelocity.magnitude *  100f));
                Debug.Log($"Points: {(int)Math.Round(other.relativeVelocity.magnitude * 100f)}");
            }
        }
    }
    
}
