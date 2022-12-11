using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

public class MonoScript : MonoBehaviour
{
    public float2 fieldDimensions;
    public int numberEntitiesToSpawn;
    public GameObject fallingEntityPrefab;
    public uint RandomSeed;
  
}

public class FallingEntityBaker : Baker<MonoScript>
{
    public override void Bake(MonoScript authoring)
    {
        AddComponent(new FallingEntityProperties
        {
            FieldDimensions = authoring.fieldDimensions,
            NumberEntitiesToSpawn = authoring.numberEntitiesToSpawn,
            FallingEntityPrefab = GetEntity(authoring.fallingEntityPrefab),
            
        });
        AddComponent(new FallingEntityRandom
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed)
        });
    }       
    
}
