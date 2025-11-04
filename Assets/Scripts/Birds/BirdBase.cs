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
        
        protected Rigidbody Rb;
        protected bool IsFlying = false;
        protected bool HasPowerUsed = false;
        [SerializeField] private protected bool _isMultiplayer = false;
        

        public BirdType BirdType { get; protected set; }
       

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
        
        private void Update()
        {
            //TODO: fix update so it can be called in that player who owns the stateauthority
            if (HasStateAuthority)
            {
                ApplyAbility();
            }
        }
        public abstract void UseSpecialAbility();
        
        public IEnumerator DestroyBird()
        {
            yield return new WaitForSeconds(5f);
            AudioManager.Instance.PlayBirdDeath();
            if (!_isMultiplayer)
            {
                Destroy(gameObject);
            }
            else
            {
                Runner.Despawn(GetComponent<NetworkObject>());
            }
        }
        public void PlayFlyingSoundEffect()
        {
            AudioManager.Instance.PlayBirdLaunch(BirdType);
        }
    
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            IsFlying = true;
        }
    }
}
