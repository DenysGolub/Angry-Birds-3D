using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    private List<GameObject> spawnedBirds;
    
    public Transform Slingshot;
    public BirdsAmmoSO BirdsList;

    public static event Action OnEmptyAmmo;
    public static event Action<GameObject> ChangeCurrentProjectile;
    public static event Action<BirdsAmmoSO> SetAmmo;

    void Start()
    {
        spawnedBirds = new List<GameObject>();
        float padding = 1.5f;
        Vector3 slingshotPosition = Slingshot.position;
        Debug.Log(slingshotPosition);
        for(int i = 1; i < BirdsList.Birds.Count; i++)
        {
            slingshotPosition.x  -= padding;
            spawnedBirds.Add(Instantiate(BirdsList.Birds[i], slingshotPosition, BirdsList.Birds[i].transform.rotation));

            padding = 0.8f;
        }

        if (SetAmmo != null)
        {
            SetAmmo.Invoke(BirdsList);
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
        if (ChangeCurrentProjectile != null && spawnedBirds.Count > 0)
        {
            GameObject bird = spawnedBirds[0].gameObject;
            ChangeCurrentProjectile(bird);
            spawnedBirds.RemoveAt(0);
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