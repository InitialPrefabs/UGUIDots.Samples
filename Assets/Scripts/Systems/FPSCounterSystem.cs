using UGUIDOTS.Common;
using Unity.Entities;

namespace UGUIDOTS.Sample.Systems {

    public class FPSCounterSystem : SystemBase {
        protected unsafe override void OnUpdate() {
            var timeData = Time.DeltaTime;

            Entities.WithAll<DynamicTextTag>().ForEach((DynamicBuffer<CharElement> b0) => {
                int fps = (int)(1f / timeData);

                b0.ResizeAndReplaceBufferWithChars(fps);

                char* ptr = (char*)b0.GetUnsafePtr();
                fps.ToChars(ptr, b0.Length, out int count);
            }).Run();
        }
    }
}
