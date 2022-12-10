#if DOTS_PRIMER
/* Copyright 2022
 * Authors: Dimension X, Inc. && Turbo Makes Games
 * Date of last edit: 2022-12-09
 * 
 * License Info:
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the Software 
 * without restriction, including without limitation the rights to use, copy, modify, merge, 
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
 * to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * 1) The above copyright notice and this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * 2) THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 */

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace MC.DOTS_Primer
{
    [BurstCompile]
    public partial struct S_SpawnEntitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<C_SpawnData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            new SpawnEntityJob { ECB = ecb }.Run();
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [BurstCompile]
    public partial struct SpawnEntityJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        
        [BurstCompile]
        private void Execute(ref C_SpawnerRandom random, in C_SpawnData spawnData)
        {
            for (var i = 0; i < spawnData.NumberToSpawn; i++)
            {
                var newEntity = ECB.Instantiate(spawnData.EntityPrefab);
                var newPosition = random.Value.NextFloat3(spawnData.MinSpawnPosition, spawnData.MaxSpawnPosition);
                var newSpeed = random.Value.NextFloat(spawnData.MinSpeed, spawnData.MaxSpeed);

                ECB.SetComponent(newEntity, new LocalTransform()
                {
                    Position = newPosition,
                    Rotation = quaternion.identity,
                    Scale = spawnData.Scale
                });
                ECB.AddComponent(newEntity, new C_Speed { Value = newSpeed });
                ECB.AddComponent<T_FollowPlayer>(newEntity);
            }
        }
    }
}
#endif