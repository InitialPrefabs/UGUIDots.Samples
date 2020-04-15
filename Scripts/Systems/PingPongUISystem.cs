using UGUIDots.Render;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UGUIDots.Sample.Systems {

    // TODO: Switch to copying this over to the shader
    [UpdateInGroup(typeof(UITransformUpdateGroup))]
    public class PingPongUISystem : SystemBase {

        private EntityCommandBufferSystem cmdBufferSystem;

        private readonly int Translation = Shader.PropertyToID("_Translation");

        protected override void OnCreate() {
            cmdBufferSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
        }

        protected unsafe override void OnUpdate() {
            var ltws         = GetComponentDataFromEntity<LocalToWorld>(true);
            var parents      = GetComponentDataFromEntity<Parent>(true);
            var cmdBuffer    = cmdBufferSystem.CreateCommandBuffer();

            TimeData* time = stackalloc TimeData[1] { Time };

            Entities.ForEach((Entity e, ref PingPongPositions c0, ref Translation c2, ref LocalToParent c3, in MaterialPropertyIndex c1) => {
                var position = ltws[e].Position.xy;
                if (math.distance(position, c0.LHS) <= 0.2f) {
                    c0.Target = c0.RHS;
                }

                if (math.distance(position, c0.RHS) <= 0.2f) {
                    c0.Target = c0.LHS;
                }

                position = math.lerp(position.xy, c0.Target, time->DeltaTime);

                var parent = parents[e].Value;

                var worldSpace = float4x4.TRS(new float3(position, 0), quaternion.identity, new float3(1));
                var localSpace = new LocalToParent { Value = math.mul(math.inverse(ltws[parent].Value), worldSpace) };

                var root = RecurseGetRoot(parents, e);

                var batch = EntityManager.GetComponentData<MaterialPropertyBatch>(root);
                var property = batch.Value[c1.Value];
                property.SetVector(Translation, new float4(localSpace.Position, 0));

                c3 = localSpace;
                c2 = new Translation { Value = localSpace.Position };

            }).WithNativeDisableUnsafePtrRestriction(time).WithoutBurst().Run();
        }

        private Entity RecurseGetRoot(ComponentDataFromEntity<Parent> parents, Entity child) {
            if (parents.Exists(child)) {
                return RecurseGetRoot(parents, parents[child].Value);
            }

            return child;
        }
    }
}
