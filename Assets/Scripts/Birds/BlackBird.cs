using UnityEngine;

public class BlackBird : BirdBase
{
    public float ExplosionRadius = 5.0F;
    public float ExplosionPower = 10.0F;
    public override void UseSpecialAbility()
    {
        Debug.Log("BlackBird PlaySoundEffect");
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius, LayerMask.GetMask("Destructable"));
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(ExplosionPower, explosionPos, ExplosionRadius, 3.0F);
        }
        
        Destroy(gameObject);
    }
}
