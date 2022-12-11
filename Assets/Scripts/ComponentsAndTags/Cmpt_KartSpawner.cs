using Unity.Entities;
using Unity.Mathematics;

//[Serializable]
public struct Cmpt_KartSpawner : IComponentData
{

    public Entity prefab;
    public float popCap;

}