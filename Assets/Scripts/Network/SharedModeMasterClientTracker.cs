using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace AngryBirds.Network
{
    public class SharedModeMasterClientTracker : NetworkBehaviour
    {
        public NetworkBirdManager BirdManager {get => _birdManager; set => _birdManager = value; }
        public static SharedModeMasterClientTracker LocalInstance;
        
        private NetworkBirdManager _birdManager;

        public override void Spawned()
        {
            LocalInstance = this;
        }

        public static void RequestBird(NetworkSlingshot slingshot, PlayerRef sender)
        {
            if (LocalInstance == null) return;
            if (LocalInstance.Object.StateAuthority == PlayerRef.None) return;

            LocalInstance.RequestBirdRpc(slingshot, sender);
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RequestBirdRpc(NetworkSlingshot slingshot, PlayerRef sender)
        {
            Debug.Log(sender + " called for bird!");
            _birdManager.SetNextBirdAsProjectile(slingshot);
        }

        public void DisconnectPlayers()
        {
            /*IEnumerable<PlayerRef> players = Runner.ActivePlayers;
            foreach (PlayerRef player in players)
            {
                Runner.Disconnect(player);
                
                Debug.Log(player + " disconnected!");
            }
            */
           
        }

        public void DisconnectPlayer(PlayerRef sender)
        {
            // Runner.Disconnect(sender);
        }
    }

}
