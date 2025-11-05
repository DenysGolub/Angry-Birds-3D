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
        public abstract void UseSpecialAbility();
        
        protected Rigidbody _rb;
        protected bool _IsFlying = false;
        protected bool _hasPowerUsed = false;

       
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
            //TODO: fix update so it can be called in that player who owns the stateauthority
            if (Input.GetMouseButtonDown(0) && HasStateAuthority)
            {
                ApplyAbility();
            }
            else if (Input.GetMouseButtonDown(0) && !HasStateAuthority)
            {
               ApplyAbilityRpc(); 
            }
        }
        public IEnumerator DestroyBird()
        {
            yield return new WaitForSeconds(5f);
            AudioManager.Instance.PlayBirdDeath();
            Runner.Despawn(GetComponent<NetworkObject>());
        }
        public void PlayFlyingSoundEffect()
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
        
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            _IsFlying = true;
        }
    }
}
