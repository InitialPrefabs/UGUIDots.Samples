using UGUIDOTS.Render;
using UGUIDOTS.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace UGUIDOTS.Sample.Systems {

    public class HeartFillSystem : SystemBase {

        protected override void OnUpdate() {
            var parents = GetComponentDataFromEntity<Unity.Transforms.Parent>(true);

            float elapsedTime = (float)Time.ElapsedTime;

            Entities.WithAll<SpriteData>().ForEach((ref AxisFillAmount c0) => {
                var current = ((float)math.cos(elapsedTime) + 1) / 2;
                c0.FillAmount = current;
            }).Run();
        }
    }
}
