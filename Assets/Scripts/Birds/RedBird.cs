using System;
using UnityEngine;

public class RedBird : BirdBase
{
  
    public override void PlaySoundEffect()
    {
        AudioManager.Instance.PlayBirdLaunch(Bird.Red);
    }

    public override void UseSpecialAbility()
    {
        
    }
}
