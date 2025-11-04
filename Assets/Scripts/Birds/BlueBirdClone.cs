using AngryBirds.Enums;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlueBirdClone : BirdBase
    {
        public Vector3 Position { get; set; }

        private void Start()
        {
            BirdType =  BirdType.Blue;
        }

        public override void UseSpecialAbility() {}
        
    }
}
