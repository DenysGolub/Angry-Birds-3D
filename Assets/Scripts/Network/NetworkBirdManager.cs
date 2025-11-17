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

                        _spawnedBirds[slingshot].Enqueue(newBird);

                        padding = 0.8f;

                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
          
        }

        
        public void SetNextBirdAsProjectile(NetworkSlingshot slingshot)
        {
            // if (!_spawnedBirds.ContainsKey(slingshot))
            //     return false;
            //
            // if (_spawnedBirds[slingshot].Count == 0)
            //     return false;

            NetworkObject bird = _spawnedBirds[slingshot].Dequeue();
            slingshot.GetProjectile(bird.gameObject);

        }

        public bool HasBirds(NetworkSlingshot slingshot)
        {
            return _spawnedBirds.ContainsKey(slingshot) &&
                   _spawnedBirds[slingshot].Count > 0;
        }
    }
}
