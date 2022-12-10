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
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace MC.DOTS_Primer
{
    public partial class BasicMoveSystem : SystemBase
    {
        private GameObject _mainPlayer = null;

        protected override void OnCreate()
        {
            RequireForUpdate<C_Speed>();
        }

        protected override void OnUpdate()
        {
            if (_mainPlayer == null)
            {
                _mainPlayer = GameObject.FindWithTag("Player");
                if (_mainPlayer == null) return;
            }

            float3 playerPos = _mainPlayer.transform.position;
            var deltaTime = SystemAPI.Time.DeltaTime;

            new BasicMoveJob
            {
                PlayerPos = playerPos,
                DeltaTime = deltaTime
            }.ScheduleParallel();

            ///NOTE: Below is commented out code that provides an alternate method
            ///to lines 30-34 (BasicMoveJob). The Entities.ForEach API will ONLY
            ///work with SystemBase but can be particularly useful for debugging,
            ///as it allows you to use .WithName that assigns a name in the System
            ///window. IJobEntity jobs paired with an ISystem, as shown in 
            ///S_SpawnEntitySystem.cs, is the more performant option, as it can be
            ///fully bursted.
            //Entities
            //    .WithName("S_BasicMoveSystem")
            //    .WithAll<T_FollowPlayer>()
            //    .ForEach((ref LocalTransform transform, in C_Speed speed) =>
            //    {
            //        var currentPos = transform.Position;
            //        if (math.distance(currentPos, playerPos) < 1.5f) return;

            //        var targetDir = math.normalize(playerPos - currentPos);
            //        var newPos = currentPos + (targetDir * (speed.Value * deltaTime));
            //        transform.Position = newPos;

            //    }).ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(T_FollowPlayer))]
    public partial struct BasicMoveJob : IJobEntity
    {
        public float3 PlayerPos;
        public float DeltaTime;

        [BurstCompile]
        private void Execute(ref LocalTransform transform, in C_Speed speed)
        {
            var currentPos = transform.Position;
            if (math.distance(currentPos, PlayerPos) < 1.5f) return;

            var targetDir = math.normalize(PlayerPos - currentPos);
            var newPos = currentPos + (targetDir * (speed.Value * DeltaTime));
            transform.Position = newPos;
        }
    }

}
#endif