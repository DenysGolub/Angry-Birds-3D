using System;
using System.Collections;
using AngryBirds.Enums;
using AngryBirds.Managers;
using UnityEngine;

namespace AngryBirds.Birds
{
    public abstract class BirdBase : MonoBehaviour
    {
        public Action OnShoot;
        
        protected Rigidbody Rb;
        protected bool IsFlying = false;
        protected bool HasPowerUsed = false;


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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !HasPowerUsed && IsFlying)
            {
                Debug.Log(IsFlying);
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
        public abstract void UseSpecialAbility();
        
        public IEnumerator DestroyBird()
        {
            yield return new WaitForSeconds(3f);
            AudioManager.Instance.PlayBirdDeath();
            Destroy(gameObject);
        }
        public void PlayFlyingSoundEffect()
        {
            AudioManager.Instance.PlayBirdLaunch(BirdType);
        }
    
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            Debug.Log("Got call to set flying");
            IsFlying = true;
        }
    }
}
