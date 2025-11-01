using System;
using AngryBirds.Enums;
using AngryBirds.Managers;
using UnityEngine;
namespace AngryBirds.Blocks
{
    public class Block : MonoBehaviour
    {
        public static Action<int> OnHealthChanged;
        public static Action<int> OnBlockDestroyed;
        
        [SerializeField] private BlockSO _blockConfiguration;
        
        private float _maxHealth;
        private float _damageMultiplier;
        private Rigidbody _rigidbody;
        private float _currentHealth;
        private BlockType _blockType;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _maxHealth = _blockConfiguration.MaxHealth;
            _damageMultiplier = _blockConfiguration.DamageMultiplier;
            _blockType = _blockConfiguration.Type;
            _currentHealth = _maxHealth;
        
            _rigidbody.mass = _blockConfiguration.Mass;
            _rigidbody.linearDamping = _blockConfiguration.LinearDamping;
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            _currentHealth -= other.relativeVelocity.magnitude * _damageMultiplier; 
            //Debug.Log($"Impact from enter: {other.relativeVelocity.magnitude * _damageMultiplier}");
            //Debug.Log($"Impulse from explosion: {other.impulse.magnitude * _damageMultiplier}");
            if (_currentHealth <= 0)
            {
                OnBlockDestroyed?.Invoke(500);
                AudioManager.Instance.PlayDestroyedBlock(_blockType);
                Destroy(gameObject);
            }
            else
            {
                OnHealthChanged?.Invoke((int)Math.Round(other.relativeVelocity.magnitude  * 100f));
               // Debug.Log($"Points: {(int)Math.Round(other.relativeVelocity.magnitude * 100f)}");
            }
        }
    }
}
