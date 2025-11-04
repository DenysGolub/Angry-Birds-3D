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
        
        protected Rigidbody Rb;
        protected bool IsFlying = false;
        protected bool HasPowerUsed = false;

       
        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
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
            if (HasStateAuthority)
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
        public void PlayFlyingSoundEffect()
        {
            AudioManager.Instance.PlayBirdLaunch(BirdType);
        }
        private void ApplyAbility()
        {
            if (Input.GetMouseButtonDown(0) && !HasPowerUsed && IsFlying)
            {
                AudioManager.Instance.PlaySpecialAbility(BirdType);
                UseSpecialAbility();
                HasPowerUsed = true;
                IsFlying = false;
            }

            if (HasPowerUsed && !IsFlying)
            {
                StartCoroutine(DestroyBird());
            }
        }
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            IsFlying = true;
        }
    }
}
