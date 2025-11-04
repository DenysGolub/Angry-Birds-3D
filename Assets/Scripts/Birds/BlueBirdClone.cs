using AngryBirds.Enums;
using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlueBirdClone : BirdBase
    {
        private void Start()
        {
            BirdType =  BirdType.Blue;
        }

        public override void UseSpecialAbility() {}
    }
}
