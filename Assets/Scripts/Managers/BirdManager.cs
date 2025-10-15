using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public List<GameObject> BirdsAmmo;
    public static event Action<GameObject> ChangeCurrentProjectile;
    public Transform Slingshot;
    public List<GameObject> spawnedBirds;

    
    
    void Start()
    {
        spawnedBirds = new List<GameObject>();
        float padding = 1.5f;
        Vector3 slingshotPosition = Slingshot.position;
        Debug.Log(slingshotPosition);
        foreach (GameObject bird in BirdsAmmo)
        {
            slingshotPosition.x  -= padding;
            spawnedBirds.Add(Instantiate(bird, slingshotPosition, Slingshot.rotation));

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
        if (ChangeCurrentProjectile != null && BirdsAmmo.Count > 0)
        {
            ChangeCurrentProjectile(BirdsAmmo[0]);
            BirdsAmmo.RemoveAt(0);
            Destroy(spawnedBirds[0]);
            spawnedBirds.RemoveAt(0);
        }
        Debug.Log(BirdsAmmo.Count);
        
    }
}