using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Translation translation, ref Rotation rotation, in MovementData movementData) =>
        {
            //if (Input.GetKey(movementData.forwardKey))
            //{
            //    translation.Value.z += movementData.movementSpeed * deltaTime;
            //}
            //if (Input.GetKey(movementData.backKey))
            //{
            //    translation.Value.z -= movementData.movementSpeed * deltaTime;
            //}
            if (Input.GetKey(movementData.leftKey) && translation.Value.x >= -1.8f)
            {
                translation.Value.x -= movementData.movementSpeed * deltaTime;
            }
            if (Input.GetKey(movementData.rightKey) && translation.Value.x <= 1.8f)
            {
                translation.Value.x += movementData.movementSpeed * deltaTime;
            }

            //rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(movementData.rotationSpeed * deltaTime)));
        }).Run();
    }
}
