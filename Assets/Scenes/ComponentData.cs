using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct PlayerInput : IComponentData {
    public float3 target;
}