using UnityEngine;
using Unity.Entities;

namespace UGUIDOTS.Sample.Authoring {
    public class PingPongPositionsAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {

        public float Width = 150f;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

            var canvas = transform.root.GetComponent<RectTransform>();

            dstManager.AddComponentData(entity, new PingPongPositions {
                OriginalWidth = Width,
                AdjustedWidth = canvas.localScale.x * Width,
                Target        = -canvas.localScale.x * Width
            });
        }
    }
}
