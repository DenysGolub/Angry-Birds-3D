using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BirdsAmmo", menuName = "Scriptable Objects/BirdsAmmo")]
public class BirdsAmmoSO : ScriptableObject
{
    public List<GameObject> Birds = new List<GameObject>();
}
