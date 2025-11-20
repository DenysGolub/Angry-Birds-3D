using AngryBirds.Enums;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlueBirdClone : BirdBase
    {
        private void Start()
        {
            BirdType =  BirdType.Blue;
        }

        protected override void UseSpecialAbility() {}
        
    }
}
