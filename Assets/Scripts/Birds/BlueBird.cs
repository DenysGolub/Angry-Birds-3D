using System.Collections;
using AngryBirds.Enums;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class BlueBird : BirdBase
    {
        [SerializeField] private GameObject _prefab;
        private NetworkObject firstBird;
        private NetworkObject secondBird;

        private void Start()
        {
            BirdType = BirdType.Blue;
        }
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_SpawnCopies(Vector3 basePos, Quaternion rot)
        {
            SpawnCopiesInternal(basePos, rot);
        }

        private void SpawnCopiesInternal(Vector3 basePos, Quaternion rot)
        {
            Vector3 posUp = basePos + Vector3.up * 0.4f;
            Vector3 posDown = basePos + Vector3.down * 0.4f;

            firstBird = Runner.Spawn(_prefab, posUp, rot);
            secondBird = Runner.Spawn(_prefab, posDown, rot);

            StartCoroutine(firstBird.GetComponent<BirdBase>().DestroyBird());
            StartCoroutine(secondBird.GetComponent<BirdBase>().DestroyBird());
        }

        IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();
        }
        public override void UseSpecialAbility()
        {
            var currentBird = GetComponent<NetworkRigidbody3D>().Rigidbody;
            Vector3 basePos = transform.position;
            Vector3 positionUp = basePos + Vector3.up * 0.4f;
            Vector3 positionDown = basePos + Vector3.down * 0.4f;
            if (HasStateAuthority)
            {
                Debug.Log($"Authority: {HasStateAuthority}, Input: {HasInputAuthority}, RunnerMode: {Runner.GameMode}");

                //TODO: spawn birds with Runner.Spawn!
            
                firstBird = Runner.Spawn(_prefab, positionUp, transform.rotation);
                secondBird = Runner.Spawn(_prefab, positionDown, transform.rotation);

                var netObj1 = firstBird.GetComponent<NetworkObject>();
                var netObj2 = secondBird.GetComponent<NetworkObject>();

                netObj1.transform.position = positionUp;
                netObj2.transform.position = positionDown;

                // StartCoroutine(Wait());
            }
            else
            {
                Rpc_SpawnCopies(basePos, transform.rotation);
            }
            

            
            // if (HasStateAuthority)
            // {
            //     SetVelocity(currentBird.angularVelocity, currentBird.linearVelocity);
            // }
            // else
            // {
            //     SetVelocityRpc(currentBird.angularVelocity, currentBird.linearVelocity);
            // }
           
            // StartCoroutine(firstBird.GetComponent<BirdBase>().DestroyBird());
            // StartCoroutine(secondBird.GetComponent<BirdBase>().DestroyBird());
        }

        private void SetVelocity(Vector3 angularVelocity, Vector3 linearVelocity)
        {
            var rb1 = firstBird.GetComponent<Rigidbody>();
            var rb2 = secondBird.GetComponent<Rigidbody>();

            rb1.isKinematic = false;
            rb2.isKinematic = false;
                
            rb1.angularVelocity = angularVelocity;
            rb2.angularVelocity = angularVelocity;
        
            rb1.linearVelocity = linearVelocity;
            rb2.linearVelocity = linearVelocity;
            
            Debug.Log($"Angular for origin: {angularVelocity}");
            Debug.Log($"Linear for origin: {linearVelocity}");
            
            Debug.Log($"Angular for first bird: {rb1.angularVelocity}");
            Debug.Log($"Linear for first bird: {rb1.linearVelocity}");

        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void SetVelocityRpc(Vector3 angular, Vector3 linear)
        {
            SetVelocity(angular, linear);
        }
    }
}
