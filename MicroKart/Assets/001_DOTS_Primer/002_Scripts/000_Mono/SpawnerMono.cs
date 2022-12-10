﻿#if DOTS_PRIMER
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
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace MC.DOTS_Primer
{
    public class SpawnerMono : MonoBehaviour
    {
        public GameObject EntityPrefab;
        public int NumberToSpawn;
        public Vector3 MinSpawnPosition;
        public Vector3 MaxSpawnPosition;
        public float Scale;
        public float MinSpeed;
        public float MaxSpeed;
        public uint RandomSeed;
    }

    public class SpawnerBaker : Baker<SpawnerMono>
    {
        public override void Bake(SpawnerMono authoring)
        {
            AddComponent(new C_SpawnData
            {
                EntityPrefab = GetEntity(authoring.EntityPrefab),
                NumberToSpawn = authoring.NumberToSpawn,
                MinSpawnPosition = authoring.MinSpawnPosition,
                MaxSpawnPosition = authoring.MaxSpawnPosition,
                Scale = authoring.Scale,
                MinSpeed = authoring.MinSpeed,
                MaxSpeed = authoring.MaxSpeed
            });

            AddComponent(new C_SpawnerRandom
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed)
            });
        }
    }
}
#endif