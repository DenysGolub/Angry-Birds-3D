using Fusion;
namespace AngryBirds.Network
{
    public class SpawnPointManager : NetworkBehaviour
    {
       [Networked, Capacity(2)]
        public NetworkArray<bool> SpawnPointsFree { get; }

        public bool IsSpawned = false;
        
        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                for (int i = 0; i < SpawnPointsFree.Length; i++)
                {
                    SpawnPointsFree.Set(i, false);
                }
            }
            IsSpawned = true;
        }

        public bool IsFree(int index)
        {
            return !SpawnPointsFree[index];
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void SetSpawnPointUsedRpc(int index, bool used)
        {
            SetSpawnPointUsed(index, used);
        }
        
        private void SetSpawnPointUsed(int index, bool used)
        {
            SpawnPointsFree.Set(index, used);
        }
    }
}