using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public class DannyFlockSpawner : MonoBehaviour
{
    public int boidsPerInterval;
    public int boidsToSpawn;
    public float interval;
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float3 target;
    public Material material;
    public Mesh mesh;
    public float maxSpeed;
    public float step;
    public int cellSize;
    private EntityManager entityManager;
    private Entity entity;
    private float elapsedTime;
    private int totalSpawnedBoids;
    private EntityArchetype ea;
    private float3 currentPosition;

    void Start()
    {
        totalSpawnedBoids = 0;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        currentPosition = this.transform.position;
        ea = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(FlockComponentData)
            );
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime = 0;
            for (int i = 0; i <= boidsPerInterval; i++)
            {
                if (totalSpawnedBoids == boidsToSpawn)
                {
                    break;
                }

                Entity e = entityManager.CreateEntity(ea);

                entityManager.AddComponentData(e, new Translation
                {
                    Value = currentPosition
                });
                entityManager.AddComponentData(e, new FlockComponentData
                {
                    velocity = math.normalize(UnityEngine.Random.insideUnitSphere) * maxSpeed,
                    perceptionRadius = perceptionRadius,
                    speed = maxSpeed,
                    step = step,
                    cohesionBias = cohesionBias,
                    separationBias = separationBias,
                    alignmentBias = alignmentBias,
                    target = target,
                    targetBias = targetBias,
                    cellSize = cellSize
                });
                entityManager.AddSharedComponentData(e, new RenderMesh
                {
                    mesh = mesh,
                    material = material,
                    castShadows = UnityEngine.Rendering.ShadowCastingMode.Off
                });
                totalSpawnedBoids++;
            }
        }
    }
}
