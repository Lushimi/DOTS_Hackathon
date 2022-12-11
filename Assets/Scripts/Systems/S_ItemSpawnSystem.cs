using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Team3
{
    [BurstCompile]
    //[UpdateInGroup( typeof( InitializationSystemGroup ) )]
    public partial struct S_ItemSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate( ref SystemState state )
        {
            state.RequireForUpdate<C_TrackProperties>();
        }

        [BurstCompile]
        public void OnDestroy( ref SystemState state )
        {
        }

        [BurstCompile]
        public void OnUpdate( ref SystemState state )
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new SpawnItemJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer( state.WorldUnmanaged ),
                //maybe this is bad
                //could also increment a counter when spawning in items, but i wanted to try tags out
                CurrentItemAmount = state.GetEntityQuery( ComponentType.ReadOnly<T_SpawnedItem>() ).CalculateEntityCount(),
            }.Run();

        }
    }

    [BurstCompile]
    public partial struct SpawnItemJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        public float CurrentItemAmount;

        [BurstCompile]
        private void Execute( A_TrackAspect track )
        {
            //MaxSpawn currently unused
            track.ItemSpawnTimer -= DeltaTime;
            if ( !track.TimeToSpawnItem ) return;
            if ( CurrentItemAmount >= track.MaxSpawnAmount ) return;

            track.ItemSpawnTimer = track.ItemSpawnRate;

            Entity newItem = ECB.Instantiate( track.ItemPrefab );
            LocalTransform randomTransform = track.GetRandomTransform();
            ECB.SetComponent( newItem, randomTransform );
            ECB.AddComponent<T_SpawnedItem>(newItem);
        }
    }
}
