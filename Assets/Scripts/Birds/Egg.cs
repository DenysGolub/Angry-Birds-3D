using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class Egg: NetworkBehaviour
    {
        [SerializeField] private float _radius = 15f;
        [SerializeField] private float _power = 100f;

        private Collider[] _colliders;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                Vector3 explosionPos = transform.position;
                _colliders = Physics.OverlapSphere(explosionPos, _radius, LayerMask.GetMask("Destructable"));
                if (HasStateAuthority)
                {
                    ApplyExplosion(explosionPos);
                }
                else
                {
                    ApplyExplosionRpc(explosionPos);
                }
               
                Runner.Despawn(GetComponent<NetworkObject>());
            }
           
        }

        private void ApplyExplosion(Vector3 explosionPos)
        {
            foreach (Collider hit in _colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(_power, explosionPos, _radius, 3.0F);
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