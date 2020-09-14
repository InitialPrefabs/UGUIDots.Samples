using Unity.Entities;

namespace UGUIDOTS.Sample {

    public struct PingPongPositions : IComponentData {
        public float OriginalWidth;
        public float AdjustedWidth;
        public float Target;
    }
}
