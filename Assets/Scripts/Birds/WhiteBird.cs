using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class WhiteBird : BirdBase
    {
        [SerializeField] private GameObject _eggPrefab;
        private void Start()
        {
            BirdType =  BirdType.White;
        }

        protected override void UseSpecialAbility()
        {
            Runner.Spawn(_eggPrefab, transform.position, transform.rotation);
            _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
