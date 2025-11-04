using AngryBirds.Enums;
using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlackBird : BirdBase
    {
     
        
        [SerializeField] private float _explosionRadius = 5.0F;
        [SerializeField] private float _explosionPower = 10.0F;
        
        private void Start()
        {
            BirdType = BirdType.Black;
        }
        public override void UseSpecialAbility()
        {
            Debug.Log("BlackBird PlaySoundEffect");
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius, LayerMask.GetMask("Destructable"));
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 3.0F);
                }
            }
        
            Runner.Despawn(GetComponent<NetworkObject>());
        }
    }
}
