using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ForceFryers
{
    static class MathF
    {
        public const float
            DEGTORAD = (2 * (float)Math.PI) / 360,
            RADTODEG = 360 / (2 * (float)Math.PI);

        /// <summary>Accelerating sine. Equation that from 0-1 accelerates according to a sine wave</summary>
        public static float SineA(float aValue)
            => (float)Math.Sin(aValue * Math.PI * 0.5);

        /// <summary>Decellerating sine. Equation that from 0-1 decellerates according to a sine wave</summary>
        public static float SineD(float aValue)
            => (float)Math.Sin((aValue - 1) * Math.PI * 0.5) + 1;

        public static float Lerp(this float thisValue, float aMin, float aMax)
            => aMin + (aMax - aMin) * thisValue;

        public static Vector2 Rotate(this Vector2 thisVector, float aRadian)
        {
            float tempSin = (float)Math.Sin(aRadian);
            float tempCos = (float)Math.Cos(aRadian);

            thisVector = new Vector2()
            {
                X = (tempCos * thisVector.X) - (tempSin * thisVector.Y),
                Y = (tempSin * thisVector.X) + (tempCos * thisVector.Y)
            };

            return thisVector;
        }

        public static int HighestPowerLessThanOrEqual(this int thisNumber, out int outPower)
        {
            if (thisNumber < 2)
            {
                outPower = 0;

                return 0;
            }

            outPower = 1;

            int temp = 2;

            while (temp * 2 <= thisNumber)
            {
                ++outPower;

                temp *= 2;
            }

            return temp;
        }

        public static int HighestPowerLessThanOrEqual(this int thisNumber)
            => thisNumber.HighestPowerLessThanOrEqual(out int voidInt);

        public static int LowestPowerMoreThanOrEqual(this int thisNumber, out int outPower)
        {
            if (thisNumber <= 0)
            {
                outPower = 0;

                return 0;
            }

            outPower = 1;

            int temp = 2;

            while (temp < thisNumber)
            {
                ++outPower;

                temp *= 2;
            }

            return temp;
        }

        public static int LowestPowerMoreThanOrEqual(this int thisNumber)
            => thisNumber.LowestPowerMoreThanOrEqual(out int voidInt);

        public static float RotationTowards(this Vector2 thisOrigin, Vector2 aTarget)
            => (float)Math.Atan2(aTarget.Y - thisOrigin.Y, aTarget.X - thisOrigin.X);

        public static float Min(this float thisValue, float aMinimum) => thisValue < aMinimum ? aMinimum : thisValue;

        public static float Max(this float thisValue, float aMaximum) => thisValue > aMaximum ? aMaximum : thisValue;

        public static float Clamp(this float thisValue, float aMin, float aMax)
        {
            if (thisValue > aMax)
                return aMax;

            if (thisValue < aMin)
                return aMin;

            return thisValue;
        }

        public static Vector2 Normalized(this Vector2 thisVector)
        {
            Vector2 returnVector = thisVector;
            returnVector.Normalize();
            return returnVector;
        }
    }
}
