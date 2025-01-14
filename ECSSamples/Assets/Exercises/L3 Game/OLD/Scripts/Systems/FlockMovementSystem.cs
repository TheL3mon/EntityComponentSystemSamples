using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public partial class FlockMovementSystem : SystemBase
{
    public NativeParallelMultiHashMap<int, FlockComponentData> cellVsEntityPositions;

    public static int GetUniqueKeyForPosition(float3 position, int cellSize)
    {
        return (int)((15 * math.floor(position.x / cellSize)) + (17 * math.floor(position.y / cellSize)) + (19 * math.floor(position.z / cellSize)));
    }

    protected override void OnCreate()
    {
        cellVsEntityPositions = new NativeParallelMultiHashMap<int, FlockComponentData>(0, Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        EntityQuery eq = GetEntityQuery(typeof(FlockComponentData));
        cellVsEntityPositions.Clear();
        if (eq.CalculateEntityCount() > cellVsEntityPositions.Capacity)
        {
            cellVsEntityPositions.Capacity = eq.CalculateEntityCount();
        }

        NativeParallelMultiHashMap<int, FlockComponentData>.ParallelWriter cellVsEntityPositionsParallel = cellVsEntityPositions.AsParallelWriter();
        Entities.ForEach((ref FlockComponentData bc, ref Translation trans) =>
        {
            FlockComponentData bcValues = new FlockComponentData();
            bcValues = bc;
            bcValues.currentPosition = trans.Value;
            cellVsEntityPositionsParallel.Add(GetUniqueKeyForPosition(trans.Value, bc.cellSize), bcValues);
        }).ScheduleParallel();

        float deltaTime = Time.DeltaTime;
        NativeParallelMultiHashMap<int, FlockComponentData> cellVsEntityPositionsForJob = cellVsEntityPositions;
        Entities.WithBurst().WithReadOnly(cellVsEntityPositionsForJob).ForEach((ref FlockComponentData bc, ref Translation trans, ref Rotation rot) =>
        {
            int key = GetUniqueKeyForPosition(trans.Value, bc.cellSize);
            NativeParallelMultiHashMapIterator<int> nmhKeyIterator;
            FlockComponentData neighbour;
            int total = 0;
            float3 separation = float3.zero;
            float3 alignment = float3.zero;
            float3 coheshion = float3.zero;

            if (cellVsEntityPositionsForJob.TryGetFirstValue(key, out neighbour, out nmhKeyIterator))
            {
                do
                {
                    if (!trans.Value.Equals(neighbour.currentPosition) && math.distance(trans.Value, neighbour.currentPosition) < bc.perceptionRadius)
                    {
                        float3 distanceFromTo = trans.Value - neighbour.currentPosition;
                        separation += (distanceFromTo / math.distance(trans.Value, neighbour.currentPosition));
                        coheshion += neighbour.currentPosition;
                        alignment += neighbour.velocity;
                        total++;
                    }
                } while (cellVsEntityPositionsForJob.TryGetNextValue(out neighbour, ref nmhKeyIterator));
                if (total > 0)
                {
                    coheshion = coheshion / total;
                    coheshion = coheshion - (trans.Value + bc.velocity);
                    coheshion = math.normalize(coheshion) * bc.cohesionBias;

                    separation = separation / total;
                    separation = separation - bc.velocity;
                    separation = math.normalize(separation) * bc.separationBias;

                    alignment = alignment / total;
                    alignment = alignment - bc.velocity;
                    alignment = math.normalize(alignment) * bc.alignmentBias;

                }

                bc.acceleration += (coheshion + alignment + separation);
                rot.Value = math.slerp(rot.Value, quaternion.LookRotation(math.normalize(bc.velocity), math.up()), deltaTime * 10);
                bc.velocity = bc.velocity + bc.acceleration;
                bc.velocity = math.normalize(bc.velocity) * bc.speed;
                trans.Value = math.lerp(trans.Value, (trans.Value + bc.velocity), deltaTime * bc.step);
                bc.acceleration = math.normalize(bc.target - trans.Value) * bc.targetBias;
            }
        }).ScheduleParallel();
    }

    protected override void OnDestroy()
    {
        cellVsEntityPositions.Dispose();
    }
}
