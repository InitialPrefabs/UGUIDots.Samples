using UnityEngine;
using Unity.Jobs.LowLevel.Unsafe;

namespace UGUIDOTS {

    public class MaxThreadCount : MonoBehaviour {

        public int JobWorkerCount = 2;
        
        private void Awake() {
            JobsUtility.JobWorkerCount = JobWorkerCount;
        }
    }
}
