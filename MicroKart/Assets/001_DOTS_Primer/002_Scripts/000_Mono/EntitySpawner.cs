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


using UnityEngine;
using UnityEngine.Rendering;

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

using Random = Unity.Mathematics.Random;

namespace MC.DOTS_Primer
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] private Material EntPrefabMaterial;
        [SerializeField] private Mesh[] EntPrefabMesh;
        [SerializeField] private int EntityCount;
        
        public struct SpawnJob : IJobParallelFor
        {
            public uint seed;
            public Entity Prototype;
            public EntityCommandBuffer.ParallelWriter ecb;
            
            public void Execute(int index)
            {
                var e = ecb.Instantiate(index, Prototype);
                Random rando = new Random(seed + (uint)(index*214));
                float range = 100f;
                ecb.SetComponent(index, e, new LocalTransform()
                {
                    Position = new float3(rando.NextFloat(-range, range), rando.NextFloat(30.0f, 50f), rando.NextFloat(-range, range)),
                    Rotation = quaternion.identity,
                    Scale = 0.1f + rando.NextFloat(0.01f, 0.8f)
                });
                ecb.SetComponent(index, e, new C_Speed()
                {
                    Value = rando.NextFloat(3.0f, 8.0f)
                });
            }
        }
        
        private void Start()
        {
            SpawnEntities();
        }
        
        private void SpawnEntities()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var manager = world.EntityManager;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

            var desc = new RenderMeshDescription(shadowCastingMode: ShadowCastingMode.On, receiveShadows: true);

            int meshCount = EntPrefabMesh.Length;
            Entity[] prototypes = new Entity[meshCount];
            for (int i = 0; i < meshCount; i++)
            {
                var renderMeshArray = new RenderMeshArray(new[] { EntPrefabMaterial }, new[] { EntPrefabMesh[i] });

                prototypes[i] = manager.CreateEntity();

                RenderMeshUtility.AddComponents(prototypes[i], manager, desc, renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
                manager.AddComponentData(prototypes[i], new LocalTransform()
                {
                    Position = new float3(0f, 0f, 0f), 
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                manager.AddComponent<C_Speed>(prototypes[i]);

                manager.AddComponent<T_FollowPlayer>(prototypes[i]);

                var spawnJob = new SpawnJob
                {
                    seed = (uint)(234567*(i+1)),
                    Prototype = prototypes[i],
                    ecb = ecb.AsParallelWriter(),
                };

                var handle = spawnJob.Schedule(EntityCount, 128);
                handle.Complete();
            }

            ecb.Playback(manager);
            ecb.Dispose();

            for (int i = 0; i < meshCount; i++)
            {
                manager.DestroyEntity(prototypes[i]);
            }
        }
    }
}
#endif