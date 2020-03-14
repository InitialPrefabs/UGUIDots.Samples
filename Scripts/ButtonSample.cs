using UnityEngine;
using Unity.Entities;

namespace UGUIDots.Controls.Authoring {

    public struct SampleID : IComponentData {
        public int Value;
    }

    public class ButtonSample : MonoBehaviour, IConvertGameObjectToEntity {

        public int Value;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            var msgEntity = dstManager.CreateEntity();
            dstManager.AddComponentData(msgEntity, new SampleID { Value = Value });

            dstManager.AddComponentData(entity, new ButtonMessageFramePayload { Value = msgEntity });

#if UNITY_EDITOR
            dstManager.SetName(msgEntity, $"{this.name} Button Payload Template");
#endif
        }
    }
}
