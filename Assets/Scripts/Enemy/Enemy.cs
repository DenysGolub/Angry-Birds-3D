using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 20f;
    public float DamageMultiplier = 2f;
    
    private float _currentHealth;
    
    public static event Action<int> OnEnemyDeath;
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
        
       
        //Debug.Log($"CurrentHealth: {CurrentHealth}");
        if (_currentHealth <= 0)
        {
            if (OnEnemyDeath != null)
            {
                OnEnemyDeath.Invoke(1000);
            }
            Destroy(gameObject);
        }
    }

   // private void OnDestroy()
    //{
      //  Debug.Log("Pig destroyed");
   // }
}
