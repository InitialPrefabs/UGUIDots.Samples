using Unity.Entities;
using Unity.Mathematics;

namespace UGUIDOTS.Sample.Systems {

    public class HeartFillSystem : SystemBase {

        protected override void OnUpdate() {
            float elapsedTime = (float)Time.ElapsedTime;
            var current = ((float)math.cos(elapsedTime) + 1) / 2;

            Entities.WithAll<SpriteData>().ForEach((ref AxisFillAmount c0) => {
                // Set all fill amounts on their axis. Flipping the axis direction is also supported.
                c0.FillAmount = current;
            }).Run();

            Entities.WithNone<Spin>().WithAll<SpriteData>().ForEach((ref RadialFillAmount c0) => {
                // Set the primary fill amount which is Arc2
                c0.Arc2 = current;
            }).Run();

            var dt = Time.DeltaTime;

            Entities.WithAll<SpriteData>().ForEach((ref RadialFillAmount c0, in Spin c1) => {
                // In this example, we're just rotating the filled sprite, we can manipulate Arc1 and Arc2.
                c0.Angle = (c0.Angle + c1.Speed * dt) % 360f;
            }).Run();
        }
    }
}
