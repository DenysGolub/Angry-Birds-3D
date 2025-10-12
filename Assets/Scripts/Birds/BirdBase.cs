using UnityEngine;

public abstract class BirdBase : MonoBehaviour
{
    public int PointsForNotUsing;
    public abstract void PlaySoundEffect();
    public abstract void UseSpecialAbility();
    
}
