using Unity.Entities;
using Unity.Mathematics;

namespace Team3
{
    public struct C_TrackProperties : IComponentData
    {
        public float3 MinSpawnPosition;
        public float3 MaxSpawnPosition;
        public int MaxSpawnAmount;
        public Entity ItemToSpawn;
        public float SpawnRate;
    }

    public struct C_ItemSpawnTimer : IComponentData
    {
        public float Value;
    }
}