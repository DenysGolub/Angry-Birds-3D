using AngryBirds.Enums;
using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlackBird : BirdBase
    {
        [SerializeField] private float _explosionRadius = 5.0F;
        [SerializeField] private float _explosionPower = 10.0F;

        private Collider[] _colliders;
        
        private void Start()
        {
            BirdType = BirdType.Black;
        }

        protected override void UseSpecialAbility()
        {
            ApplyExplosionRpc(transform.position);
            Runner.Despawn(GetComponent<NetworkObject>());
        }
        
        private void ApplyExplosion(Vector3 birdPos)
        {
            Vector3 explosionPos = birdPos;
            _colliders = Physics.OverlapSphere(explosionPos, _explosionRadius, LayerMask.GetMask("Destructable"));
            foreach (Collider hit in _colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 3.0F);
                }
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void ApplyExplosionRpc(Vector3 birdPos)
        {
            ApplyExplosion(birdPos);
        }
    }
}



