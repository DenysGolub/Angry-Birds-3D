using System;
using System.Collections.Generic;
using AngryBirds.SO.Scripts;
using Fusion;
using UnityEngine;

namespace AngryBirds.Network
{
    public class NetworkBirdManager : NetworkBehaviour
    {
        public Dictionary<NetworkSlingshot, BirdsAmmoSO> _playersAmmo 
            = new Dictionary<NetworkSlingshot, BirdsAmmoSO>();

        public Dictionary<NetworkSlingshot, Queue<NetworkObject>> _spawnedBirds 
            = new Dictionary<NetworkSlingshot, Queue<NetworkObject>>();

        public static Action<int> UpdateBirdsCount;

        private void OnEnable()
        {
            NetworkSlingshot.OnShotFired += SetNextBirdAsProjectile;
        }
        private void OnDisable()
        {
            NetworkSlingshot.OnShotFired -= SetNextBirdAsProjectile;
        }
        
        public void SetPlayerSlingshot(BirdsAmmoSO ammo, NetworkSlingshot slingshot)
        {
            _playersAmmo.Add(slingshot, ammo);
            _spawnedBirds.Add(slingshot, new Queue<NetworkObject>());
        }
        
        public void SpawnBirds()
        {
            foreach (var entry in _playersAmmo)
            {
                NetworkSlingshot slingshot = entry.Key;
                BirdsAmmoSO ammo = entry.Value;
                UpdateBirdsCount?.Invoke(ammo.Birds.Count);

                float padding = 1.2f;
                Vector3 pos = slingshot.transform.position;
                pos.y += 0.15f;

                for (int i = 1; i < ammo.Birds.Count; i++)
                {
                    pos.x -= padding;

                    NetworkObject newBird = Runner.Spawn(
                        ammo.Birds[i],
                        pos,
                        ammo.Birds[i].transform.rotation
                    );

                    _spawnedBirds[slingshot].Enqueue(newBird);

                    padding = 0.8f;
                }
            }
        }
        
        public void SetNextBirdAsProjectile(NetworkSlingshot slingshot)
        {
            UpdateBirdsCount?.Invoke(-1);
            GetBirdRpc(slingshot);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void GetBirdRpc(NetworkSlingshot slingshot)
        {
            if (!_spawnedBirds.ContainsKey(slingshot))
            {
                return;
            }
            if (_spawnedBirds[slingshot].Count == 0)
            {
                return;
            }
            NetworkObject bird = _spawnedBirds[slingshot].Dequeue();
            slingshot.GetProjectileRpc(slingshot.Object.Id, bird.Id);
        }
    }
}
