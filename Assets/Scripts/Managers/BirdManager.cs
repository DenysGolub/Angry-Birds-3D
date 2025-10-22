using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static event Action<GameObject> ChangeCurrentProjectile;
    public Transform Slingshot;
    private List<GameObject> _spawnedBirds;
    public BirdsAmmoSO BirdsList;
    public static Action OnEmptyAmmo;

    void Start()
    {
        _spawnedBirds = new List<GameObject>();
        float padding = 1.5f;
        Vector3 slingshotPosition = Slingshot.position;
        Debug.Log(slingshotPosition);
        for(int i = 1; i < BirdsList.Birds.Count; i++)
        {
            slingshotPosition.x  -= padding;
            _spawnedBirds.Add(Instantiate(BirdsList.Birds[i], slingshotPosition, BirdsList.Birds[i].transform.rotation));

            padding = 0.8f;
        }
    }
    
    void OnEnable()
    {
        GameManager.OnNextBirdChanged += SetUpCurrentProjectile;
    }

    void OnDisable()
    {
        GameManager.OnNextBirdChanged -= SetUpCurrentProjectile;
    }
    
    void SetUpCurrentProjectile()
    {
        if (ChangeCurrentProjectile != null && _spawnedBirds.Count > 0)
        {
            GameObject bird = _spawnedBirds[0].gameObject;
            ChangeCurrentProjectile(bird);
            _spawnedBirds.RemoveAt(0);
        }
        else
        {
            if (OnEmptyAmmo != null)
            {
                OnEmptyAmmo.Invoke();
            }
        }
    }
    
    
}