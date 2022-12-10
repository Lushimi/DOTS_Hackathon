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
        public float3 ExcludeMaxSpawnPos => _trackProperties.ValueRO.ExcludeMaxSpawnPosition;
        public float3 ExcludeMinSpawnPos => _trackProperties.ValueRO.ExcludeMinSpawnPosition;
        public float ItemSpawnRate => _trackProperties.ValueRO.SpawnRate;
        public int MaxSpawnAmount => _trackProperties.ValueRO.MaxSpawnAmount;
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
            bool excludeVolumeCheck = false;
            float3 randomPosition;
            do
            {
                randomPosition = _trackRandom.ValueRW.Value.NextFloat3( MinSpawnPos, MaxSpawnPos );
                bool3 cond1 = ExcludeMinSpawnPos <= randomPosition;                
                bool3 cond2 = randomPosition <= ExcludeMaxSpawnPos;
                excludeVolumeCheck = cond1.x == true && cond1.z == true && cond2.x == true && cond2.z == true;
            }
            while ( excludeVolumeCheck );
            var heightToSpawn = 1f;
            randomPosition.y = heightToSpawn;
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