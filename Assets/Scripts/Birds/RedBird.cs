using AngryBirds.Enums;

namespace AngryBirds.Birds
{
    public class RedBird : BirdBase
    {
        private void Start()
        {
            _birdType =  BirdType.Red;
        }
        public override void UseSpecialAbility() { }
    }
}
