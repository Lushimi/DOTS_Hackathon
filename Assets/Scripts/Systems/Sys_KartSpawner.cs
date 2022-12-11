using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;


public partial class SpawnSysBase : SystemBase
{

    int spawnInterval = 1;
    int cnt = 0;
    protected override void OnUpdate()
    {



        EntityQuery npKarts = EntityManager.CreateEntityQuery( typeof( Cmpt_NpKart ) );


        Cmpt_KartSpawner kartSpawn = SystemAPI.GetSingleton<Cmpt_KartSpawner>();
        float popCap = kartSpawn.popCap;


        EntityCommandBuffer cmdBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer( World.Unmanaged );




        if ( npKarts.CalculateEntityCount() < popCap )
        {
            cnt = ( cnt <= 1000 ) ? cnt + 1 : 0;


            Entity spawnedEntity = cmdBuffer.Instantiate( kartSpawn.prefab );



        }

    }
}