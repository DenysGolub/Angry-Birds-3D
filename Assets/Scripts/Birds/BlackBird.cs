using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlackBird : BirdBase
    {
        private void Start()
        {
            BirdType = BirdType.Black;
        }
        
        [SerializeField] private float _explosionRadius = 5.0F;
        [SerializeField] private float _explosionPower = 10.0F;
        public override void UseSpecialAbility()
        {
            Debug.Log("BlackBird PlaySoundEffect");
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius, LayerMask.GetMask("Destructable"));
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 3.0F);
            }
        
            Destroy(gameObject);
        }
    }
}
