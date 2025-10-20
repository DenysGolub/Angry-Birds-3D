using UnityEngine;

public class WhiteBird : BirdBase
{
    public GameObject EggPrefab;
    
    public override void PlaySoundEffect()
    {
        Debug.Log("WhiteBird");
    }

    public override void UseSpecialAbility()
    {
        Rigidbody eggRb = Instantiate(EggPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }
}
