using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class YellowBird : BirdBase
    {
        [SerializeField] private float _forceMultiplier = 40;
    
        private void Start()
        {
            _birdType =  BirdType.Yellow;
        }
    
        public override void UseSpecialAbility()
        {
            Debug.Log($"use ability for YellowBird");
            _rb.AddForce(transform.forward * _forceMultiplier, ForceMode.VelocityChange);
        }
    }
}
