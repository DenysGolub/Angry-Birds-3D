using AngryBirds.Enums;

namespace AngryBirds.Birds
{
    public class RedBird : BirdBase
    {
        private void Start()
        {
            BirdType =  BirdType.Red;
        }

        protected override void UseSpecialAbility() { }
    }
}
