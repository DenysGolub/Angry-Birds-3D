using System;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds.EnemySystem
{
    public class Enemy : NetworkBehaviour
    {
        public static event Action<int> OnEnemyDeath;
        public static event Action<int> OnHealthChanged;
        public static event Action AddEnemyCount;
        
        [SerializeField] private float _maxHealth = 150f;
        [SerializeField] private float _damageMultiplier = 15f;
        
        private float _currentHealth;
        
        private void Start()
        {
            _currentHealth = _maxHealth;
            AddEnemyCount?.Invoke();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            _currentHealth -= other.relativeVelocity.magnitude * _damageMultiplier;
            if (_currentHealth <= 0)
            {
                OnEnemyDeath?.Invoke(1000);
                AudioManager.Instance.PlayPigDeath();
                
                Runner.Despawn(GetComponent<NetworkObject>());
            }
            else
            {
                OnHealthChanged?.Invoke((int)Math.Round(other.relativeVelocity.magnitude *  100f));
            }
        }
    }
}
