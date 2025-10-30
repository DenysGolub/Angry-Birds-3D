using AngryBirds.Levels;
using Fusion;
using UnityEngine;

namespace AngryBirds
{
    public class NetworkSlingshot : NetworkBehaviour
    {
        [Networked] public PlayerRef Owner { get; set; }

        public override void Spawned()
        {
            if (Owner != Runner.LocalPlayer)
            {
                Destroy(gameObject.GetComponent<Slingshot>());
            }
        }
        
        
    }
}
