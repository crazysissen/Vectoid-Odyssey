using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    // For now, includes 1D simplex perlin noise
    class Noise
    {
        // Permutation table of length 512
        private readonly byte[] myPermutations;
        private int mySeed;

        public Noise() : this(new Random().Next())
        { }

        public Noise(int aSeed)
        {
            mySeed = aSeed;

            Random tempRandom = new Random(aSeed);
            myPermutations = new byte[0x200];
            tempRandom.NextBytes(myPermutations);
        }

        public float Generate(float x)
        {
            int tempIndex = x.Floor();
            float tempX = x - tempIndex;

            float
                tempFirstValue = (float)Math.Pow(1.0f - tempX * tempX, 4) * Gradient(myPermutations[tempIndex & 0xff], tempX),
                tempIntermittent = 1 - (tempX * tempX - 2 * tempX + 1),
                tempSecondValue = (float)Math.Pow(tempIntermittent, 4) * Gradient(myPermutations[(tempIndex + 1) & 0xff], tempX - 1);

            //Factor 0.395 scales the range to precisely - 1-> 1
            return 0.395f * (tempFirstValue + tempSecondValue);
        }

        private float Gradient(int aHash, float x)
        {
            int tempHash = aHash & 15;
            float tempGradient = 1.0f + (tempHash & 7);

            if ((tempHash & 8) != 0)
                return -tempGradient * x;
            
            return tempGradient * x;
        }
    }
}
