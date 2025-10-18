using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 20f;
    public float DamageMultiplier = 2f;
    
    private float CurrentHealth;
    
    public static event Action<float> OnEnemyDeath;
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void OnCollisionEnter(Collision other)
    {

        if (CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth -= other.relativeVelocity.magnitude * DamageMultiplier;
        //Debug.Log($"CurrentHealth: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(0);
            Destroy(gameObject);
        }
    }

   // private void OnDestroy()
    //{
      //  Debug.Log("Pig destroyed");
   // }
}
