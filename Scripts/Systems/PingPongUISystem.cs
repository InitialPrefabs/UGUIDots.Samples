using UGUIDots.Render;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace UGUIDots.Sample.Systems {

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class PingPongUISystem : SystemBase {

        private EntityCommandBufferSystem cmdBufferSystem;

        protected override void OnCreate() {
            cmdBufferSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
        }

        protected unsafe override void OnUpdate() {
            var ltws = GetComponentDataFromEntity<LocalToWorld>(true);
            var ltps = GetComponentDataFromEntity<LocalToParent>(false);
            var translations = GetComponentDataFromEntity<Translation>(false);
            var cmdBuffer = cmdBufferSystem.CreateCommandBuffer();

            TimeData* time = stackalloc TimeData[1] { Time };

            Entities.ForEach((Entity e, DynamicBuffer<Float4MaterialPropertyParam> b0, ref Parent c1, ref PingPongPositions c0) => {
                var position = ltws[e].Position.xy;
                if (math.distance(position, c0.LHS) <= 0.1f) {
                    c0.Target = c0.RHS;
                }

                if (math.distance(position, c0.RHS) <= 0.1f) {
                    c0.Target = c0.LHS;
                }

                position = math.lerp(position.xy, c0.Target, time->DeltaTime);

                var worldSpace = float4x4.TRS(new float3(position, 0), quaternion.identity, new float3(1));
                var localSpace = new LocalToParent { Value = math.mul(math.inverse(ltws[c1.Value].Value), worldSpace) };

                ltps[e]         = localSpace;
                translations[e] = new Translation { Value = localSpace.Position };

                var property = b0[0].ID;

                b0[0] = new Float4MaterialPropertyParam(
                    property,
                    new float4(localSpace.Position, 1)
                );

                cmdBuffer.AddComponent(e, new BuildUIElementTag { });
            }).WithNativeDisableUnsafePtrRestriction(time).Run();
        }
    }
}
