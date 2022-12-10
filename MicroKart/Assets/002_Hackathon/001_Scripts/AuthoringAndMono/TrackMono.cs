using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Team3
{
    public class TrackMono : MonoBehaviour
    {
        public GameObject ItemToSpawn;
        public int MaxSpawnAmount;
        public Vector3 MinSpawnPosition;
        public Vector3 MaxSpawnPosition;
        public float SpawnRate;
        public uint RandomSeed;
    }

    public class TrackBaker : Baker<TrackMono>
    {
        public override void Bake( TrackMono authoring )
        {
            AddComponent( new C_TrackProperties
            {
                ItemToSpawn = GetEntity( authoring.ItemToSpawn ),
                MaxSpawnAmount = authoring.MaxSpawnAmount,
                MinSpawnPosition = authoring.MinSpawnPosition,
                MaxSpawnPosition = authoring.MaxSpawnPosition,
                SpawnRate = authoring.SpawnRate
            } );

            AddComponent( new C_TrackRandom
            {
                Value = Random.CreateFromIndex( authoring.RandomSeed )
            } );

            AddComponent<C_ItemSpawnTimer>();
        }
    }
}