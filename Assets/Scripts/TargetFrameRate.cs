using UnityEngine;

namespace UGUIDOTS {

    public class TargetFrameRate : MonoBehaviour {

        private void Awake() {
            Application.targetFrameRate = 60;
        }
    }
}
