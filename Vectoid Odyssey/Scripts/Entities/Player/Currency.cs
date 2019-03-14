using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    [Serializable]
    class Currency
    {
        public int GetDelta => myValues?[0] ?? 0;
        public int GetTheta => myValues?[1] ?? 0;
        public int GetGamma => myValues?[2] ?? 0;

        private int[] myValues;

        public int this[int index]
        {
            get => (index >= 0 && index < 3) ? myValues?[index] ?? 0 : 0;
        }

        public Currency(int aD, int aT, int aG)
        {
            Set(aD, aT, aG);
        }

        public void Set(int aD, int aT, int aG)
            => myValues = new int[] { aD, aT, aG };

        public void Reset()
            => Set(0, 0, 0);

        public void ChangeDelta(int change) 
            => myValues[0] += change;

        public void ChangeTheta(int change) 
            => myValues[1] += change;

        public void ChangeGamma(int change) 
            => myValues[2] += change;
    }
}
