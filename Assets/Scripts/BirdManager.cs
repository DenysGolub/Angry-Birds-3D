using System;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public List<GameObject> BirdsAmmo;
    public static event Action<GameObject> ChangeCurrentProjectile;

    void OnEnable()
    {
        Slingshot.OnShotFired += SetUpCurrentProjectile;
    }

    void OnDisable()
    {
        Slingshot.OnShotFired -= SetUpCurrentProjectile;
    }
    
    void SetUpCurrentProjectile()
    {
        if (ChangeCurrentProjectile != null && BirdsAmmo.Count > 0)
        {
            ChangeCurrentProjectile(BirdsAmmo[0]);
            BirdsAmmo.RemoveAt(0);
        }
        Debug.Log(BirdsAmmo.Count);
        
    }
}