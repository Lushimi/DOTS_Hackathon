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
    public partial class Sys_entFollow : SystemBase
    {
        public GameObject obj_car;
        public float speedMultiplier;


        //[+] C_speed is a ICmpt attached to entities
        protected override void OnCreate()
        {
            // RequireForUpdate<C_Speed>();
        }

        protected override void OnUpdate()
        {
            if (obj_car == null)
            {
                obj_car = GameObject.FindWithTag("objCar");
                if (obj_car == null) return;
            }

            float3 objPos = obj_car.transform.position;
            var deltaTime = SystemAPI.Time.DeltaTime;

            new BasicMoveJob
            {
                objPos = objPos,
                DeltaTime = deltaTime
            }.ScheduleParallel();



            //[+]---------------------------------------
            float3 sigmaPos= float3.zero;
            float3 sigmaVel= float3.zero;
            foreach ((TransformAspect transpect, RefRW<Cmpt_NpKart> entCar) in SystemAPI.Query<TransformAspect, RefRW<Cmpt_NpKart>>()){
                float3 carPos = transpect.WorldPosition;
                float3 carVel= entCar.ValueRW.velocity;
                sigmaPos+=carPos;
                sigmaVel+=carVel;
            }

            foreach ((TransformAspect transpect, RefRW<Cmpt_NpKart> entCar) in SystemAPI.Query<TransformAspect, RefRW<Cmpt_NpKart>>()){
                float3 centroid= (sigmaPos/500);
                float3 avgVel= (sigmaVel/500);

                float speed= entCar.ValueRW.speed;
                float speedLimit= entCar.ValueRW.speedLimit;
                float3 selfPos = transpect.WorldPosition;
                float3 velocity= entCar.ValueRW.velocity;
                float radar = entCar.ValueRW.radar;

                float radius=  (math.distance(selfPos, centroid))+ 0.000001f;
                float displacement= math.distance(selfPos, float3.zero);
                float3 direction= math.normalize(selfPos);


                entCar.ValueRW.velocity+= centroid * (radius/radar) ;
                entCar.ValueRW.velocity-= centroid * (radar/radius) ;

                if(math.length(entCar.ValueRW.velocity)>speedLimit){
                    entCar.ValueRW.velocity= math.normalize(entCar.ValueRW.velocity)* speedLimit;
                }

                transpect.WorldPosition-= entCar.ValueRW.velocity*deltaTime*speed;
                transpect.LookAt(entCar.ValueRW.velocity);

            }
        
        }
    }






    // [+] typeof(<ICombonent tag>)
    [BurstCompile]
    [WithAll(typeof(Cmpt_NpKart))]
    public partial struct BasicMoveJob : IJobEntity
    {
        public float3 objPos;
        public float DeltaTime;

        [BurstCompile]
        // private void Execute(ref LocalTransform transform, in Cmpt_NpKart entCar)
        private void Execute(ref TransformAspect transpect, in Cmpt_NpKart entCar)
        {
            var currentPos = transpect.WorldPosition;
            if (math.distance(currentPos, objPos) < 1.5f) return;

            var targetDir = math.normalize(objPos - currentPos);
            var newPos = currentPos - (targetDir * (entCar.speed * DeltaTime))*3.2f;

            transpect.WorldPosition = newPos;
            transpect.LookAt(newPos-currentPos );
        }
    }

}
#endif