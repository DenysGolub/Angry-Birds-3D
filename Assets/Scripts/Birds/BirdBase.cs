using System;
using System.Collections;
using AngryBirds.Enums;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public abstract class BirdBase : NetworkBehaviour
    {
        public Action OnShoot;
        public BirdType BirdType { get; protected set; }
        
        private protected Rigidbody _rb;
        private protected bool _IsFlying = false;
        private protected bool _hasPowerUsed = false;
        
        protected abstract void UseSpecialAbility();
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    
        private void OnEnable()
        {
            OnShoot += SetFlying;
        }

        private void OnDisable()
        {
            OnShoot -= SetFlying;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !HasStateAuthority)
            {
                ApplyAbilityRpc(); 
            }
            else if(Input.GetMouseButtonDown(0) && HasStateAuthority)
            {
                ApplyAbility();
            }
        }
        public IEnumerator DestroyBird()
        {
            yield return new WaitForSeconds(5f);
            AudioManager.Instance.PlayBirdDeath();
            Runner.Despawn(GetComponent<NetworkObject>());
        }

        private void PlayFlyingSoundEffect()
        {
            AudioManager.Instance.PlayBirdLaunch(BirdType);
        }
        private void ApplyAbility()
        {
            if (!_hasPowerUsed && _IsFlying)
            {
                AudioManager.Instance.PlaySpecialAbility(BirdType);
                UseSpecialAbility();
                _hasPowerUsed = true;
                _IsFlying = false;
            }

            if (_hasPowerUsed && !_IsFlying)
            {
                StartCoroutine(DestroyBird());
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void ApplyAbilityRpc()
        {
            ApplyAbility();
        }
        
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            _IsFlying = true;
        }
    }
}
