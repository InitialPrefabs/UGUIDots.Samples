using UGUIDots.Render;
using UGUIDots.Transforms;
using UGUIDots.Transforms.Systems;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UGUIDots.Sample.Systems {

    // TODO: Ping ponging with the resolution change is a bit odd.
    // TODO: Make a note for this.
    [UpdateAfter(typeof(AnchorSystem))]
    [UpdateInGroup(typeof(UITransformUpdateGroup))]
    [DisableAutoCreation]
    public class PingPongReactiveScaleSystem : SystemBase {

        protected override void OnCreate() {
            RequireSingletonForUpdate<ResolutionChangeEvt>();
        }

        protected override void OnUpdate() {
            var ltws = GetComponentDataFromEntity<LocalToWorld>(true);
            var parents = GetComponentDataFromEntity<Parent>(true);
            Entities.ForEach((Entity e, ref PingPongPositions c0) => {

                var root      = RecurseGetRoot(parents, e);
                var ltwRoot   = ltws[root];
                var rootScale = ltwRoot.Scale();

                c0.AdjustedWidth = rootScale.x * c0.OriginalWidth;
            }).WithoutBurst().Run();
        }

        private Entity RecurseGetRoot(ComponentDataFromEntity<Parent> parents, Entity child) {
            if (parents.Exists(child)) {
                return RecurseGetRoot(parents, parents[child].Value);
            }

            return child;
        }
    }

    [UpdateInGroup(typeof(UpdateMaterialGroup))]
    public class PingPongUISystem : SystemBase {

        private EntityCommandBufferSystem cmdBufferSystem;

        private readonly int Translation = Shader.PropertyToID("_Translation");

        private EntityQuery nonEventQuery;

        protected override void OnCreate() {
            cmdBufferSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();

            nonEventQuery = GetEntityQuery(new EntityQueryDesc { 
                None = new [] { 
                    ComponentType.ReadOnly<ResolutionChangeEvt>() 
                }
            });

            RequireForUpdate(nonEventQuery);
        }

        protected override void OnStartRunning() {
            var parents = GetComponentDataFromEntity<Parent>(true);
            Entities.WithAll<PingPongPositions>().ForEach((Entity entity, ref LocalToParent c0, ref Translation c1, in MaterialPropertyIndex c2) => {
                c0 = new LocalToParent {
                    Value = float4x4.TRS(default, quaternion.identity, new float3(1))
                };

                c1 = new Translation {
                    Value = default
                };

                var root = RecurseGetRoot(parents, entity);
                var materialProps = EntityManager.GetComponentData<MaterialPropertyBatch>(root).Value;
                var prop = materialProps[c2.Value];

                prop.SetVector(Translation, new Vector4(0, 0, 0, 1));
            }).WithoutBurst().Run();
        }

        protected unsafe override void OnUpdate() {
            var ltws         = GetComponentDataFromEntity<LocalToWorld>(true);
            var parents      = GetComponentDataFromEntity<Parent>(true);
            var cmdBuffer    = cmdBufferSystem.CreateCommandBuffer();

            TimeData* time = stackalloc TimeData[1] { Time };

            Entities.ForEach((Entity e, ref PingPongPositions c0, ref Translation c2, ref LocalToParent c3, in MaterialPropertyIndex c1) => {
                var x          = c2.Value.x;

                if (math.distance(x, c0.Target) <= 0.2f) {
                    c0.Target = c0.AdjustedWidth;
                }

                if (math.distance(x, c0.Target) <= 0.2f) {
                    c0.Target = -c0.AdjustedWidth;
                }

                x = math.lerp(x, c0.Target, time->DeltaTime);

                var matrix = float4x4.TRS(new float3(x, 0, 0), quaternion.identity, new float3(1));
                var localSpace = new LocalToParent { Value = matrix };

                var root     = RecurseGetRoot(parents, e);
                var batch    = EntityManager.GetComponentData<MaterialPropertyBatch>(root);
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
