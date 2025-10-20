using UnityEngine;

public class BlackBird : BirdBase
{
    public float radius = 5.0F;
    public float power = 10.0F;
   
    public override void PlaySoundEffect()
    {
        Debug.Log("BlackBird PlaySoundEffect");
    }
   
    public override void UseSpecialAbility()
    {
        Debug.Log("BlackBird PlaySoundEffect");
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, LayerMask.GetMask("Destructable"));
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
