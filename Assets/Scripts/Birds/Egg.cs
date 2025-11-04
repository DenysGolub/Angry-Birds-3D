using Fusion;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class Egg: NetworkBehaviour
    {
        [SerializeField] private float _radius = 15f;
        [SerializeField] private float _power = 100f;
        public bool IsMultiplayer = false;
      
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius, LayerMask.GetMask("Destructable"));
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(_power, explosionPos, _radius, 3.0F);
                }
            
                if (!IsMultiplayer)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Runner.Despawn(GetComponent<NetworkObject>());
                }
                
            }
           
        }
    }
}