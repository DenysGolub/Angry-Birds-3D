using AngryBirds.Levels;
using AngryBirds.Managers;
using Fusion;
using UnityEngine;

namespace AngryBirds
{
    public class SharedModeMasterClientTracker : NetworkBehaviour
    {
        public static SharedModeMasterClientTracker LocalInstance;
        private NetworkBirdManager _birdManager;

        public override void Spawned()
        {
            LocalInstance = this;
        }
        
        public NetworkBirdManager BirdManager {get => _birdManager; set => _birdManager = value; }
     
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RequestBirdRpc(NetworkSlingshot slinsghot, PlayerRef sender)
        {
            Debug.Log(sender + " called for bird!");
            _birdManager.SetNextBirdAsProjectile(slinsghot);
        }

        public static void RequestBird(NetworkSlingshot slingshot, PlayerRef sender)
        {
            Debug.Log($"Local instance_{LocalInstance}_" );
            if (LocalInstance == null) return;
            if (LocalInstance.Object.StateAuthority == PlayerRef.None) return;

            LocalInstance.RequestBirdRpc(slingshot, sender);
        }
        
        
        
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void SetBirdForSlingshotRpc(NetworkSlingshot slinsghot, PlayerRef sender)
        {
            Debug.Log(sender + " called for bird!");
            _birdManager.SetNextBirdAsProjectile(slinsghot);
        }

        public static void SetBirdForSlingshot(NetworkSlingshot slingshot, PlayerRef sender)
        {
            Debug.Log($"Local instance_{LocalInstance}_" );
            if (LocalInstance == null) return;
            if (LocalInstance.Object.StateAuthority == PlayerRef.None) return;

            LocalInstance.RequestBirdRpc(slingshot, sender);
        }
        

       
    }

}
