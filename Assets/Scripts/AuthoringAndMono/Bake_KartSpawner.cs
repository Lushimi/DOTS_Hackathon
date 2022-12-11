
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class Bake_KartSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float popCap;

}







public class SpawnBaker: Baker<Bake_KartSpawner>
{
    public override void Bake(Bake_KartSpawner authoring)
    {
 

        AddComponent(new Cmpt_KartSpawner
        {
           prefab= GetEntity(authoring.prefab),
           popCap= authoring.popCap,
        });
    }

}