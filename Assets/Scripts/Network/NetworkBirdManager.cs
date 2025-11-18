using System.Collections.Generic;
using AngryBirds.Birds;
using AngryBirds.Levels;
using AngryBirds.Network;
using AngryBirds.SO.Scripts;
using Fusion;
using UnityEngine;

namespace AngryBirds.Managers
{
    public class NetworkBirdManager : NetworkBehaviour
    {
        public Dictionary<NetworkSlingshot, BirdsAmmoSO> _playersAmmo 
            = new Dictionary<NetworkSlingshot, BirdsAmmoSO>();

        public Dictionary<NetworkSlingshot, Queue<NetworkObject>> _spawnedBirds 
            = new Dictionary<NetworkSlingshot, Queue<NetworkObject>>();


        public List<NetworkObject> birdsForDisplayDebug1 = new List<NetworkObject>();
        public List<NetworkObject> birdsForDisplayDebug2 = new List<NetworkObject>();

        
        public void SetPlayerSlingshot(BirdsAmmoSO ammo, NetworkSlingshot slingshot)
        {
            Debug.Log("Set Player!");
            _playersAmmo.Add(slingshot, ammo);
            _spawnedBirds.Add(slingshot, new Queue<NetworkObject>());
            Debug.Log("Exit the method");
        }

        private void OnEnable()
        {
            NetworkSlingshot.OnShotFired += SetNextBirdAsProjectile;
        }
        private void OnDisable()
        {
            NetworkSlingshot.OnShotFired -= SetNextBirdAsProjectile;
        }
        
        
        public void SpawnBirds()
        {
            Debug.Log("Enter to spawn birds method!");
            try
            {
                int index = 0;
                Debug.Log("Spawn birds for");
                foreach (var entry in _playersAmmo)
                {
                    NetworkSlingshot slingshot = entry.Key;
                    BirdsAmmoSO ammo = entry.Value;

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

                        if (index == 0)
                        {
                            birdsForDisplayDebug1.Add(newBird);
                        }
                        else
                        {
                            birdsForDisplayDebug2.Add(newBird);
                        }
                        _spawnedBirds[slingshot].Enqueue(newBird);

                        padding = 0.8f;
                    }
                    index++;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
          
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
            Debug.Log($"Set {bird} for {slingshot}!!!");
            slingshot.GetProjectileRpc(slingshot.Object.Id, bird.Id);
            // slingshot.CreateProjectileRpc(bird.Id, slingshot.Object.Id);
        }
        
        public void SetNextBirdAsProjectile(NetworkSlingshot slingshot)
        {
            GetBirdRpc(slingshot);
        }

        public bool HasBirds(NetworkSlingshot slingshot)
        {
            return _spawnedBirds.ContainsKey(slingshot) &&
                   _spawnedBirds[slingshot].Count > 0;
        }
    }
}
