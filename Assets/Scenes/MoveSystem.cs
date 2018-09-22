#define USE_JOB_SYSTEM

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

#if USE_JOB_SYSTEM

public class MoveSystem : JobComponentSystem {

    public struct Data : IJobProcessComponentData<PlayerInput, Position> {

        public float moveSpeed;
        public float dt;

        public void Execute([ReadOnly]ref PlayerInput input, ref Position position) {
            var pos = position.Value;
            var target = input.target;
            if (math.all(target == pos)) {
                return;
            }
            var diff = target - pos;
            var lenToGo = math.length(diff);
            var lenGo = dt * moveSpeed;
            if (lenGo < lenToGo) {
                pos += diff * lenGo / lenToGo;
            } else {
                pos = target;
            }
            position = new Position { Value = pos };
        }
    }

    float moveSpeed = 5f;

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        return new Data() {
            dt = Time.deltaTime,
            moveSpeed = moveSpeed,
        }.Schedule(this, inputDeps);
    }
}

#else

public class MoveSystem : ComponentSystem {

    public struct Data {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<PlayerInput> Input;
        public ComponentDataArray<Position> Position;
    }

    [Inject] private Data m_Data;

    float moveSpeed = 5f;

    protected override void OnUpdate() {
        float dt = Time.deltaTime;
        for (int i = 0; i < m_Data.Length; ++i) {
            var pos = m_Data.Position[i].Value;
            var target = m_Data.Input[i].target;
            if (math.all(target == pos)) {
                continue;
            }
            var diff = target - pos;
            var lenToGo = math.length(diff);
            var lenGo = dt * moveSpeed;
            if (lenGo < lenToGo) {
                pos += diff * lenGo / lenToGo;
            } else {
                pos = target;
            }
            m_Data.Position[i] = new Position { Value = pos };
        }
    }
}

#endif