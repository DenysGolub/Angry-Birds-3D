using System.Collections;
using AngryBirds.Enums;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlueBird : BirdBase
    {
        private NetworkObject _firstBird;
        private NetworkObject _secondBird;
        
        private Vector3 _basePos;
        private Vector3 _positionUp;
        private Vector3 _positionDown;
        
        [SerializeField] private GameObject _prefab;
        

        private void Start()
        {
            BirdType = BirdType.Blue;
        }
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void SpawnCopiesRpc(Vector3 basePos, Quaternion rot)
        {
            SpawnCopies(basePos, rot);
        }

        private void SpawnCopies(Vector3 basePos, Quaternion rot)
        {
            Vector3 posUp = basePos + Vector3.up;
            Vector3 posDown = basePos + Vector3.down;

            _firstBird = Runner.Spawn(_prefab, posUp, rot);
            _secondBird = Runner.Spawn(_prefab, posDown, rot);

            StartCoroutine(_firstBird.GetComponent<BirdBase>().DestroyBird());
            StartCoroutine(_secondBird.GetComponent<BirdBase>().DestroyBird());
        }
        public override void UseSpecialAbility()
        {
            var currentBird = GetComponent<Rigidbody>(); 
            _basePos = transform.position;
            if (HasStateAuthority)
            {
                SpawnCopies(_basePos, transform.rotation);
                SetVelocity(currentBird.angularVelocity, currentBird.linearVelocity);
            }
            else
            {
                SpawnCopiesRpc(_basePos, transform.rotation);
                SetVelocityRpc(currentBird.angularVelocity, currentBird.linearVelocity);
            }
        }

      
        private void SetVelocity(Vector3 angularVelocity, Vector3 linearVelocity)
        {
            var rb1 = _firstBird.GetComponent<Rigidbody>();
            var rb2 = _secondBird.GetComponent<Rigidbody>();

            rb1.isKinematic = false;
            rb2.isKinematic = false;
                
            rb1.angularVelocity = angularVelocity;
            rb2.angularVelocity = angularVelocity;
        
            rb1.linearVelocity = linearVelocity;
            rb2.linearVelocity = linearVelocity;
            
            // Debug.Log($"Angular for origin: {angularVelocity}");
            // Debug.Log($"Linear for origin: {linearVelocity}");
            //
            // Debug.Log($"Angular for first bird: {rb1.angularVelocity}");
            // Debug.Log($"Linear for first bird: {rb1.linearVelocity}");

        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void SetVelocityRpc(Vector3 angular, Vector3 linear)
        {
            SetVelocity(angular, linear);
        }
    }
}
