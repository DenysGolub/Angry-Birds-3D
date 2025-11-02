using System;
using System.Collections.Generic;
using AngryBirds.SO.Scripts;
using Fusion;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class NetworkBirdManager : NetworkBehaviour
    {
        public static event Action OnEmptyAmmo;
        public static event Action<GameObject> ChangeCurrentProjectile;
        public static event Action<BirdsAmmoSO> SetAmmo;
        
        private Queue<GameObject> _spawnedBirds = new Queue<GameObject>();
    
        [SerializeField] private Transform _slingshot;
        [SerializeField] private BirdsAmmoSO _birdsList;
        
        public void SetNewAmmoAndSlingshot(BirdsAmmoSO ammo, Transform slingshot)
        {
            _birdsList = ammo;
            _slingshot = slingshot;
            SetAmmo?.Invoke(_birdsList);

            float padding = 1.5f;
            Vector3 slingshotPosition = _slingshot.position;
            slingshotPosition.y += 0.15f;

            for (int i = 1; i < _birdsList.Birds.Count; i++)
            {
                slingshotPosition.x -= padding;
                GameObject newBird = Runner.Spawn(_birdsList.Birds[i], slingshotPosition, _birdsList.Birds[i].transform.rotation, Object.InputAuthority).gameObject;
                _spawnedBirds.Enqueue(newBird);
                padding = 0.8f;
                Debug.Log("Spawned bird!");
            }

            Debug.Log($"Spawned {_spawnedBirds.Count} birds for new ammo");
        }
    
        private void OnEnable()
        {
            GameManager.OnNextBirdChanged += SetUpCurrentProjectile;
        }

        private void OnDisable()
        {
            GameManager.OnNextBirdChanged -= SetUpCurrentProjectile;
        }
    
        private void SetUpCurrentProjectile()
        {
            Debug.Log("Sended bird!");
            if (ChangeCurrentProjectile != null && _spawnedBirds.Count > 0)
            {
                GameObject bird = _spawnedBirds.Dequeue().gameObject;
                ChangeCurrentProjectile(bird);
            }
            else
            {
                Debug.Log("Empty!");
                OnEmptyAmmo?.Invoke();
            }
        }
    }
}