using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCOdyssey
{
    class TimerTable
    {
        public float[] AccessTimes { get; private set; }
        public bool AccessComplete { get; private set; }
        public float AccessCurrent { get; set; }
        public float AccessMaxTime { get; private set; }

        public float AccessCurrentStepProgress { get; private set; }
        public float GetTotalProgress => AccessCurrent / AccessMaxTime;

        public TimerTable(float[] someTimes, float aStartTime = 0.0f)
        {
            AccessCurrent = aStartTime;
            AccessComplete = aStartTime > someTimes.Sum();

            SetTimes(someTimes);
        }

        public int Update(float aDeltaTime)
        {
            AccessCurrent += aDeltaTime;

            float tempAccumulative = 0.0f;

            for (int i = 0; i < AccessTimes.Length; ++i)
            {
                if (AccessTimes[i] + tempAccumulative > AccessCurrent)
                {
                    AccessCurrentStepProgress = (AccessCurrent - tempAccumulative) / AccessTimes[i];
                    return i;
                }

                tempAccumulative += AccessTimes[i];
            }

            AccessComplete = true;
            return AccessTimes.Length - 1;
        }

        public void SetTimes(float[] someTimes)
        {
            AccessTimes = someTimes;
            AccessMaxTime = someTimes.Sum();
        }
    }
}
