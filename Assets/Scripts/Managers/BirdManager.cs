using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static event Action<GameObject> ChangeCurrentProjectile;
    public Transform Slingshot;
    public List<GameObject> spawnedBirds;
    public BirdsAmmoSO BirdsList;

    
    
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
    }
}