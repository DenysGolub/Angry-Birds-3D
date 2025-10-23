using UnityEngine;

public class YellowBird : BirdBase
{
    public float ForceMultiplier = 40;
    
    void Start()
    {
        _birdType =  BirdType.Yellow;
    }
    
    public override void UseSpecialAbility()
    {
        Debug.Log($"use ability for YellowBird");
        rb.AddForce(transform.forward * ForceMultiplier, ForceMode.VelocityChange);
    }
}
