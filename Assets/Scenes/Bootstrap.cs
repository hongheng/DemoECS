using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using Unity.Mathematics;

public class Bootstrap : MonoBehaviour {

    public Mesh PlayerMesh;
    public Material PlayerMaterial;

    public static float3 PlayerInitPos = new float3(1f, 1f, 0);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private void Start() {
        Debug.Log("Start ECS");
        var EntityManager = World.Active.GetOrCreateManager<EntityManager>();
        var playerArchetype = EntityManager.CreateArchetype(
            typeof(TransformMatrix),
            typeof(Position),
            typeof(PlayerInput)
        );

        var player = EntityManager.CreateEntity(playerArchetype);
        EntityManager.SetComponentData(player, new Position(PlayerInitPos));
        EntityManager.SetComponentData(player, new PlayerInput { target = PlayerInitPos });
        EntityManager.AddSharedComponentData(player, new MeshInstanceRenderer {
            mesh = PlayerMesh,
            material = PlayerMaterial,
        });
    }

}
