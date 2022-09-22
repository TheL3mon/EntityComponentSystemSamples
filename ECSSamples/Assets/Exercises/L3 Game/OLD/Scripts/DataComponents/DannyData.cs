using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct DannyData : IComponentData
{
    public float jumpForce;
    public KeyCode jumpKey;
}