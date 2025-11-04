using System;
using AngryBirds.Enums;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;
namespace AngryBirds.Blocks
{
    public class Block : NetworkBehaviour
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

            float impact = other.relativeVelocity.magnitude * _damageMultiplier;

            if (HasStateAuthority)
            {
                ApplyImpact(impact);
            }
            else
            {
                ApplyImpactRpc(impact);
            }
        }

        private void ApplyImpact(float impact)
        {
            _currentHealth -= impact;

            if (_currentHealth <= 0)
            {
                OnBlockDestroyed?.Invoke(500);
                AudioManager.Instance.PlayDestroyedBlock(_blockType);

                Runner.Despawn(GetComponent<NetworkObject>());

            }
            else
            {
                OnHealthChanged?.Invoke((int)Math.Round(impact * 100f));
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void ApplyImpactRpc(float impact)
        {
            ApplyImpact(impact);
        }
    }
}
