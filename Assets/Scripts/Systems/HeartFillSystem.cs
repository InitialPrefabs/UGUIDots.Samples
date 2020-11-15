using Unity.Entities;
using Unity.Mathematics;

namespace UGUIDOTS.Sample.Systems {

    public class HeartFillSystem : SystemBase {

        protected override void OnUpdate() {
            float elapsedTime = (float)Time.ElapsedTime;

            Entities.WithAll<SpriteData>().ForEach((ref AxisFillAmount c0) => {
                var current = ((float)math.cos(elapsedTime) + 1) / 2;
                c0.FillAmount = current;
            }).Run();
        }
    }
}
