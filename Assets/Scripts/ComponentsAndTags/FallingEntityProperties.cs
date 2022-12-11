using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct FallingEntityProperties : IComponentData
{
    public float2 FieldDimensions;
    public int NumberEntitiesToSpawn;
    public Entity FallingEntityPrefab;
    //public Rigidbody Rb;
    //public BoxCollider Bc;

}
