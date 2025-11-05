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
        public override void UseSpecialAbility()
        {
            Vector3 explosionPos = transform.position;
            _colliders = Physics.OverlapSphere(explosionPos, _explosionRadius, LayerMask.GetMask("Destructable"));
            Debug.Log("Explosion count: " + _colliders.Length);

            if (HasStateAuthority)
            {
                ApplyExplosion(explosionPos);
            }
            else
            {
                Debug.Log("Rpc for black bird!");
                ApplyExplosionRpc(explosionPos);
            }
        
            Runner.Despawn(GetComponent<NetworkObject>());
        }
        
        private void ApplyExplosion(Vector3 explosionPos)
        {
            foreach (Collider hit in _colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(_explosionPower, explosionPos, _explosionRadius, 3.0F);
                    Debug.Log("Applying force!");
                    
                }
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void ApplyExplosionRpc(Vector3 explosionPos)
        {
            ApplyExplosion(explosionPos);
        }
    }
}



