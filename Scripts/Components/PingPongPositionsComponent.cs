using Unity.Entities;

namespace UGUIDots.Sample {

    public struct PingPongPositions : IComponentData {
        public float OriginalWidth;
        public float AdjustedWidth;
        public float Target;
    }
}
