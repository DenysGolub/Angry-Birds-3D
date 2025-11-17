using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds.Network
{
    public class NetworkSlingshotManager: NetworkBehaviour
    {
        public bool IsSpawned = false;
		
        public override void Spawned()
        {
            IsSpawned = true;
        }
        
        
        
    }
}