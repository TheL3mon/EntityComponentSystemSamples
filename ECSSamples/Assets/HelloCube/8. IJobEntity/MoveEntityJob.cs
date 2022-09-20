using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct MoveEntityJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(ref Translation translation, in RotationSpeed_IJobEntity speed)
    {
        translation.Value.y = math.sin(DeltaTime*1000);//speed.Speed * DeltaTime;
    }
}
