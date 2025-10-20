using System;
using UnityEngine;

namespace Birds
{
    public class Egg: MonoBehaviour
    {
        private float _radius = 15f;
        private float _power = 100f;
      
        void OnCollisionEnter(Collision collision)
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
            
                Destroy(gameObject);
            }
           
        }
    }
}