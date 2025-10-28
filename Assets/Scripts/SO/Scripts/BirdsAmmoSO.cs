using System.Collections.Generic;
using UnityEngine;

namespace AngryBirds.SO.Scripts
{
    [CreateAssetMenu(fileName = "BirdsAmmo", menuName = "Scriptable Objects/BirdsAmmo")]
    public class BirdsAmmoSO : ScriptableObject
    {
        public List<GameObject> Birds = new List<GameObject>();
    }
}
