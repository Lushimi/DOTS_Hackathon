using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;


namespace Team3
{

    public class TrackMono : MonoBehaviour
    {
        public GameObject ItemToSpawn;
        public Vector3 TrackBoundsMin => GetTrackBounds.TrackBoundsMin;
        public Vector3 TrackBoundsMax => GetTrackBounds.TrackBoundsMax;
        public Vector3 ExcludeBoundsMax => GetTrackBounds.ExcludeBoundsMax;
        public Vector3 ExcludeBoundsMin => GetTrackBounds.ExcludeBoundsMin;
        public int MaxSpawnAmount;
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
                MinSpawnPosition = authoring.TrackBoundsMin,
                MaxSpawnPosition = authoring.TrackBoundsMax,
                ExcludeMinSpawnPosition = authoring.ExcludeBoundsMin,
                ExcludeMaxSpawnPosition = authoring.ExcludeBoundsMax,
                SpawnRate = authoring.SpawnRate
            } );
            //Debug.Log( authoring.TrackBoundsMin + " Max: " + authoring.TrackBoundsMax );
            AddComponent( new C_TrackRandom
            {
                Value = Random.CreateFromIndex( authoring.RandomSeed )
            } );
            AddComponent<C_ItemSpawnTimer>();
        }
    }
}

