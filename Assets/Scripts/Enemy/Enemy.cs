using System;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds.Enemy
{
    public class Enemy : NetworkBehaviour
    {
        public float MaxHealth = 20f;
        public float DamageMultiplier = 30f;
    
        public static event Action<int> OnEnemyDeath;
        public static event Action<int> OnHealthChange;
        public static event Action AddEnemyCount;
        
        private float _currentHealth;
        
        private void Start()
        {
            _currentHealth = MaxHealth;
            AddEnemyCount?.Invoke();
        }
        //TODO: make rpc for impact for enemy
        private void OnCollisionEnter(Collision other)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            _currentHealth -= other.relativeVelocity.magnitude * DamageMultiplier;
  //          Debug.Log($"Impact from enter: {other.relativeVelocity.magnitude * DamageMultiplier}");
//            Debug.Log($"Impulse from explosion: {other.impulse.magnitude * DamageMultiplier}");
            if (_currentHealth <= 0)
            {
                OnEnemyDeath?.Invoke(1000);
                AudioManager.Instance.PlayPigDeath();
                
                Runner.Despawn(GetComponent<NetworkObject>());
            }
            else
            {
                OnHealthChange?.Invoke((int)Math.Round(other.relativeVelocity.magnitude *  100f));
                //Debug.Log($"Points: {(int)Math.Round(other.relativeVelocity.magnitude * 100f)}");
            }
        }
    }
}
