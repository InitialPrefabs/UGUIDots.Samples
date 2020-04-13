using Unity.Entities;
using Unity.Mathematics;

namespace UGUIDots.Sample {

    public struct PingPongPositions : IComponentData {
        public float2 LHS, RHS;
        public float2 Target;
    }
}
