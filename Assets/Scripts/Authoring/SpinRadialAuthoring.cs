using Unity.Entities;
using UnityEngine;

namespace UGUIDOTS.Sample {

    public struct Spin : IComponentData {
        public float Speed;
    }

    public class SpinRadialAuthoring : MonoBehaviour, IConvertGameObjectToEntity {
        
        public float Speed = 10f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData(entity, new Spin { Speed = Speed });
        }
    }
}
