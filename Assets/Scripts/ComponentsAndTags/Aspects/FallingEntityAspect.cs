using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UIElements;

public readonly partial struct FallingEntityAspect : IAspect
{
    public readonly Entity Entity;
    private readonly TransformAspect _transformAspect;
    private readonly RefRO<FallingEntityProperties> _fallingEntityProperties;
    private readonly RefRW<FallingEntityRandom> _fallingEntityRandom;

    public int NumberEntitiesToSpawn => _fallingEntityProperties.ValueRO.NumberEntitiesToSpawn;
    public Entity FallingEntityPrefab => _fallingEntityProperties.ValueRO.FallingEntityPrefab;

    
    public TransformAspect GetRandomEntityTransform()
    {
        return new TransformAspect
        {
            LocalPosition = GetRandomPosition(),
            LocalRotation = GetRandomRotation(),
            LocalScale = GetRandomScale(0.5f)
        };
    }
    
    
    public float3 GetRandomPosition()
    {
        float3 randomPosition;
        do
        {
            randomPosition = _fallingEntityRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        }
        while (math.distancesq(_transformAspect.LocalPosition, randomPosition) <= FALLING_ENTITY_RADIUS_SQ);

        return randomPosition;
    }

    public float3 MinCorner => _transformAspect.LocalPosition - HalfDimensions;
    public float3 MaxCorner => _transformAspect.LocalPosition + HalfDimensions;
    public float3 HalfDimensions => new()
    {
        x = _fallingEntityProperties.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = _fallingEntityProperties.ValueRO.FieldDimensions.y * 0.5f

    };
    public const float FALLING_ENTITY_RADIUS_SQ = 70;

    public quaternion GetRandomRotation() => quaternion.RotateY(_fallingEntityRandom.ValueRW.Value.NextFloat(-0.5f, 0.5f));
    public float GetRandomScale(float min) => _fallingEntityRandom.ValueRW.Value.NextFloat(min, 1f);

    public float2 GetRandomOffset()
    {
        return _fallingEntityRandom.ValueRW.Value.NextFloat2();
    }
}
