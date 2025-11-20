using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class YellowBird : BirdBase
    {
        [SerializeField] private float _forceMultiplier = 40;
    
        private void Start()
        {
            BirdType =  BirdType.Yellow;
        }

        protected override void UseSpecialAbility()
        {
            _rb.AddForce(transform.forward * _forceMultiplier, ForceMode.VelocityChange);
        }
    }
}
