using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class DannySystem : SystemBase
{
    protected override void OnUpdate()
    {
        //var deltaTime = Time.DeltaTime;

        //Entities.ForEach((Entity sphere, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, in DannyData dannyData) =>
        //{
        //    if (Input.GetKey(dannyData.jumpKey))
        //    {
        //        var forceVector = (float3)Vector3.right * dannyData.jumpForce * deltaTime;
        //        physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
        //    }
            
        //    //float3 linearVel = physicsVelocity.Linear;
        //    //float3 angularVel = physicsVelocity.Angular;
        //    //float linearVelLength = math.sqrt(math.pow(linearVel.x, 2) + math.pow(linearVel.y, 2) + math.pow(linearVel.z, 2));
        //    //float angularVelLength = math.sqrt(math.pow(angularVel.x, 2) + math.pow(angularVel.y, 2) + math.pow(angularVel.z, 2));
        //    //if (linearVelLength <= 0.5f || angularVelLength <= 0.5f)
        //    //{
        //    //    var forceVector = (float3)Vector3.up * dannyData.jumpForce * deltaTime;
        //    //    physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
        //    //}
        //}).Run();
    }
}
