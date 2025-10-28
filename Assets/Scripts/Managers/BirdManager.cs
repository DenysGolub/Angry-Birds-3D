using System;
using System.Collections.Generic;
using AngryBirds.SO.Scripts;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class BirdManager : MonoBehaviour
    {
        private Queue<GameObject> _spawnedBirds = new Queue<GameObject>();
    
        [SerializeField] private Transform _slingshot;
        [SerializeField] private BirdsAmmoSO _birdsList;

        public static event Action OnEmptyAmmo;
        public static event Action<GameObject> ChangeCurrentProjectile;
        public static event Action<BirdsAmmoSO> SetAmmo;
    
        private void Awake()
        {
            float padding = 1.5f;
            Vector3 slingshotPosition = _slingshot.position;
            slingshotPosition.y += 0.15f;
            Debug.Log(slingshotPosition);
            for(int i = 1; i < _birdsList.Birds.Count; i++)
            {
                slingshotPosition.x  -= padding;
                _spawnedBirds.Enqueue(Instantiate(_birdsList.Birds[i], slingshotPosition, _birdsList.Birds[i].transform.rotation));
                padding = 0.8f;
            }
        }

        private void Start()
        {
            SetAmmo?.Invoke(_birdsList);
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
            if (ChangeCurrentProjectile != null && _spawnedBirds.Count > 0)
            {
                GameObject bird = _spawnedBirds.Dequeue().gameObject;
                ChangeCurrentProjectile(bird);
            }
            else
            {
                OnEmptyAmmo?.Invoke();
            }
        }
    }
}