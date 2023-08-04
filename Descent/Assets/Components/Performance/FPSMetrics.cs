using UnityEngine;
using System.Collections.Generic;

/* Components -> Performance -> FPSMetrics
 * 
 * Class Description:
 * Calculates the average and minimum FPS over an adjustable range of samples.
 */

namespace Components.Performance
{
    [System.Serializable]
    public class FPSMetrics
    {
        [Header("Sampling Options")]
        public int samplesMin = 10;
        public int samplesMax = 200;
        public float updatesPerSecond = 3f;

        private Queue<float> deltaTimes = new();

        public float averageFPS
        {
            get
            {
                float numSamples = deltaTimes.Count;
                float totalTimePassed = 0;
                foreach (float deltaT in deltaTimes)
                    totalTimePassed += deltaT;
                float meanDeltaT = totalTimePassed / numSamples;
                float meanFPS = 1f / meanDeltaT;
                return meanFPS;
            }
        }

        public void Sample()
        {
            deltaTimes.Enqueue(Time.deltaTime);
            while (deltaTimes.Count > samplesMin)
                deltaTimes.Dequeue();
        }
    }
}