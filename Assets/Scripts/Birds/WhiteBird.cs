using UnityEngine;

public class WhiteBird : BirdBase
{
    public GameObject EggPrefab;
    void Start()
    {
        _birdType =  BirdType.White;
    }
    
    public override void UseSpecialAbility()
    {
        Rigidbody eggRb = Instantiate(EggPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }
}
