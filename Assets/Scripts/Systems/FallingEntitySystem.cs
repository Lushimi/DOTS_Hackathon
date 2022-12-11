using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.VisualScripting;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnFallingEntitySystem : ISystem
{
    public readonly Entity Entity;
    private readonly TransformAspect _transformAspect;
    private readonly RefRO<FallingEntityProperties> _fallingEntityProperties;
    private readonly RefRW<FallingEntityRandom> _fallingEntityRandom;

   

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<FallingEntityProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //state.Enabled = false;
        
            var fallingEntity = SystemAPI.GetSingletonEntity<FallingEntityProperties>();
            var falling = SystemAPI.GetAspectRW<FallingEntityAspect>(fallingEntity);

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

            for (var i = 0; i < falling.NumberEntitiesToSpawn; i++)
            {
                var newFallingEntity = ecb.Instantiate(falling.FallingEntityPrefab);
                //var newFallingEntityTransform = falling.GetRandomEntityTransform();
                ecb.SetComponent(newFallingEntity, new LocalTransform { Position = falling.GetRandomPosition(), Rotation = falling.GetRandomRotation(), Scale = falling.GetRandomScale(0.5f) }); ;

            }
            ecb.Playback(state.EntityManager);
        
    }

    /*
    private float3 GetRandomPosition()
    {
        float3 randomPosition;
        do
        {
            randomPosition = _fallingEntityRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        }
        while (math.distancesq(_transformAspect.LocalPosition, randomPosition) <= FALLING_ENTITY_RADIUS_SQ);

        return randomPosition;
    }

    private float3 MinCorner => _transformAspect.LocalPosition - HalfDimensions;
    private float3 MaxCorner => _transformAspect.LocalPosition + HalfDimensions;
    private float3 HalfDimensions => new()
    {
        x = _fallingEntityProperties.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = _fallingEntityProperties.ValueRO.FieldDimensions.y * 0.5f

    };
    private const float FALLING_ENTITY_RADIUS_SQ = 70;
    
    private quaternion GetRandomRotation() => quaternion.RotateY(_fallingEntityRandom.ValueRW.Value.NextFloat(-0.5f, 0.5f));
    private float GetRandomScale(float min) => _fallingEntityRandom.ValueRW.Value.NextFloat(min, 1f);
*/
}
