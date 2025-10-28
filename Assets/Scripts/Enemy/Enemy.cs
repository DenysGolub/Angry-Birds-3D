using System;
using AngryBirds.Managers;
using UnityEngine;

namespace AngryBirds.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private float _currentHealth;
    
        public float MaxHealth = 20f;
        public float DamageMultiplier = 30f;
    
        public static event Action<int> OnEnemyDeath;
        public static event Action<int> OnHealthChange;
        public static event Action AddEnemyCount;
        private void Start()
        {
            _currentHealth = MaxHealth;
            AddEnemyCount?.Invoke();
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
                OnEnemyDeath?.Invoke(1000);
                AudioManager.Instance.PlayPigDeath();
                Destroy(gameObject);
            }
            else
            {
                OnHealthChange?.Invoke((int)Math.Round(other.relativeVelocity.magnitude *  100f));
                Debug.Log($"Points: {(int)Math.Round(other.relativeVelocity.magnitude * 100f)}");
            }
        }
    }
}
