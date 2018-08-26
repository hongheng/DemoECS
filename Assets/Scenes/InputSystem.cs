using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem {

    struct PlayerData {
        public readonly int Length;
        public ComponentDataArray<PlayerInput> Input;
    }

    [Inject] private PlayerData m_Players;

    float z;

    protected override void OnStartRunning() {
        Debug.Log("OnStartRunning");
        base.OnStartRunning();
        z = Camera.main.WorldToScreenPoint(Bootstrap.PlayerInitPos).z;
    }

    protected override void OnUpdate() {
        if (!Input.GetMouseButtonUp(0)) {
            return;
        }
        float dt = Time.deltaTime;
        for (int i = 0; i < m_Players.Length; ++i) {
            PlayerInput pi;
            var pos = Input.mousePosition;
            pos.z = z;
            pi.target = Camera.main.ScreenToWorldPoint(pos);
            m_Players.Input[i] = pi;
        }
    }

}