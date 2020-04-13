using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace UGUIDots.Sample.Authoring {
    public class PingPongPositionsAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {

        public float Width = 150f;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData(entity, new PingPongPositions {
                LHS    = ((float3)(transform.position - new Vector3(150, 0, 0))).xy,
                RHS    = ((float3)(transform.position + new Vector3(150, 0, 0))).xy,
                Target = ((float3)(transform.position - new Vector3(150, 0, 0))).xy,
            });
        }
    }
}
