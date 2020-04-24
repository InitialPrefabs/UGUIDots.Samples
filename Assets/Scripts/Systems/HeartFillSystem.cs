using UGUIDots.Render;
using UGUIDots.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace UGUIDots.Sample.Systems {
    [UpdateInGroup(typeof(UpdateMaterialGroup))]
    public class HeartFillSystem : SystemBase {

        // TODO: Implement this
        protected override void OnUpdate() {
            var parents = GetComponentDataFromEntity<Parent>(true);

            Entities.ForEach((Entity entity, ref FillAmount c0, in MaterialPropertyIndex c1, in SpriteData c2) => {
                var current = c0.Amount;
                current     = ((float)math.cos(Time.ElapsedTime) + 1) / 2;
                c0.Amount   = current;

                var canvasRoot     = HierarchyUtils.GetRoot(entity, parents);
                var properties     = EntityManager.GetComponentData<MaterialPropertyBatch>(canvasRoot);
                var associatedProp = properties.Value[c1.Value];

                associatedProp.SetInt(ShaderIDConstants.FillType, c0.FillTypeAsInt());
                associatedProp.SetFloat(ShaderIDConstants.Fill, current);
            }).WithoutBurst().Run();
        }
    }
}
