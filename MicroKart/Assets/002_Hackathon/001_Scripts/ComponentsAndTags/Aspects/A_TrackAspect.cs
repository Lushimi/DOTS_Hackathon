using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace Team3
{
    public readonly partial struct A_TrackAspect : IAspect
    {
        public readonly Entity Entity;
        private readonly TransformAspect _transformAspect;

        private readonly RefRO<C_TrackProperties> _trackProperties;
        private readonly RefRW<C_TrackRandom> _trackRandom;
        private readonly RefRW<C_ItemSpawnTimer> _itemSpawnTimer;
        public float3 MaxSpawnPos => _trackProperties.ValueRO.MaxSpawnPosition;
        public float3 MinSpawnPos => _trackProperties.ValueRO.MinSpawnPosition;
        public float ItemSpawnRate => _trackProperties.ValueRO.SpawnRate;
        public int MaxSpawn => _trackProperties.ValueRO.MaxSpawnAmount;
        public Entity ItemPrefab => _trackProperties.ValueRO.ItemToSpawn;
        public float ItemSpawnTimer
        {
            get => _itemSpawnTimer.ValueRO.Value;
            set => _itemSpawnTimer.ValueRW.Value = value;
        }

        public LocalTransform GetRandomTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale( 0.5f )
            };
        }

        public float3 GetRandomPosition()
        {
            float3 randomPosition;
            /* SPAWN HERE
             * do
            {

            }*/
            //while ( AREAS TO REJECT );

            randomPosition = _trackRandom.ValueRW.Value.NextFloat3( MinSpawnPos, MaxSpawnPos );
            return randomPosition;
        }

        private quaternion GetRandomRotation() => quaternion.RotateY( _trackRandom.ValueRW.Value.NextFloat( -0.25f, 0.25f ) );
        private float GetRandomScale( float min ) => _trackRandom.ValueRW.Value.NextFloat( min, 1f );

        public bool TimeToSpawnItem => ItemSpawnTimer <= 0f;


        /*    public LocalTransform GetZombieSpawnPoint()
            {
                var position = GetRandomZombieSpawnPoint();
                return new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.RotateY( MathHelpers.GetHeading( position, _transformAspect.WorldPosition ) ),
                    Scale = 1f
                };
            }

            private float3 GetRandomZombieSpawnPoint()
            {
                return ZombieSpawnPoints[_graveyardRandom.ValueRW.Value.NextInt( ZombieSpawnPoints.Length )];
            }*/
    }
}