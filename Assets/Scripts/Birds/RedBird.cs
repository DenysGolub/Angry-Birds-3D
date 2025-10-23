using System;
using UnityEngine;

public class RedBird : BirdBase
{
    void Start()
    {
        _birdType =  BirdType.Red;
    }
    public override void UseSpecialAbility() { }
}
