using System;
using System.Collections;
using AngryBirds.Enums;
using AngryBirds.Managers;
using UnityEngine;

namespace AngryBirds.Birds
{
    public abstract class BirdBase : MonoBehaviour
    {
        private protected Rigidbody _rb;
        private protected bool isFlying = false;
        private protected bool _hasPowerUsed = false;
        private protected BirdType _birdType;
    
        public Action OnShoot;
        public BirdType BirdType => _birdType;

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
            if (Input.GetMouseButtonDown(0) && !_hasPowerUsed && isFlying)
            {
                Debug.Log(isFlying);
                UseSpecialAbility();
                _hasPowerUsed = true;
                isFlying = false;
            }

            if (_hasPowerUsed && !isFlying)
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
            AudioManager.Instance.PlayBirdLaunch(_birdType);
        }
    
        private void SetFlying()
        {
            PlayFlyingSoundEffect();
            Debug.Log("Got call to set flying");
            isFlying = true;
        }
    }
}
