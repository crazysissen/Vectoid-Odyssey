using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    static class MathV
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

        public static float Abs(this float thisValue)
            => Math.Abs(thisValue);

        public static float Abs(this int thisValue)
           => Math.Abs(thisValue);

        public static float Lerp(this float thisValue, float aMin, float aMax)
            => aMin + (aMax - aMin) * thisValue;

        public static Vector2 ToVector2(this float aRadian)
            => (new Vector2(0, -1)).Rotate(aRadian);

        public static float ToRadian(this Vector2 aVector)
            => (float)Math.Atan2(aVector.Y, aVector.X);

        public static Vector2 Rotate(this Vector2 thisVector, float aRadian)
        {
            float tempSin = (float)Math.Sin(aRadian);
            float tempCos = (float)Math.Cos(aRadian);

            return new Vector2()
            {
                X = (tempCos * thisVector.X) - (tempSin * thisVector.Y),
                Y = (tempSin * thisVector.X) + (tempCos * thisVector.Y)
            };
        }

        public static Point RoundToPoint(this Vector2 aVector)
            => new Point((int)Math.Round(aVector.X), (int)Math.Round(aVector.Y));

        public static int HighestPowerLessThanOrEqual(this int thisNumber, out int outPower)
        {
            if (thisNumber < 2)
            {
                outPower = 0;

                return 0;
            }

            outPower = 1;

            int tempCurrent = 2;

            while (tempCurrent * 2 <= thisNumber)
            {
                ++outPower;

                tempCurrent *= 2;
            }

            return tempCurrent;
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

            int tempCurrent = 2;

            while (tempCurrent < thisNumber)
            {
                ++outPower;

                tempCurrent *= 2;
            }

            return tempCurrent;
        }

        public static int LowestPowerMoreThanOrEqual(this int thisNumber)
            => thisNumber.LowestPowerMoreThanOrEqual(out int voidInt);

        public static float RotationTowards(this Vector2 thisOrigin, Vector2 aTarget)
            => (float)Math.Atan2(aTarget.Y - thisOrigin.Y, aTarget.X - thisOrigin.X);

        public static float Min(this float thisValue, float aMinimum) 
            => thisValue < aMinimum ? aMinimum : thisValue;

        public static float Max(this float thisValue, float aMaximum) 
            => thisValue > aMaximum ? aMaximum : thisValue;

        public static float Clamp(this float thisValue, float aMin, float aMax)
            => thisValue.Clamp(aMin, aMax, out bool voidBool);

        public static float Clamp(this float thisValue, float aMin, float aMax, out bool outClamped)
        {
            outClamped = true;

            if (thisValue > aMax)
                return aMax;

            if (thisValue < aMin)
                return aMin;

            outClamped = false;

            return thisValue;
        }

        public static float ClampThis(this ref float thisValue, float aMin, float aMax)
            => thisValue = thisValue.Clamp(aMin, aMax);

        public static float Wrap(this float thisValue, float aMin, float aMax)
        {
            if (thisValue > aMax || thisValue < aMin)
            {
                float tempValue = thisValue - aMin;

                return tempValue % (aMax - aMin) + aMin + (tempValue < 0 ? (aMax - aMin) : 0);
            }

            return thisValue;
        }

        public static float WrapThis(this ref float thisValue, float aMin, float aMax)
            => thisValue.Wrap(aMin, aMax);

        public static Vector2 Normalized(this Vector2 thisVector)
        {
            Vector2 tempReturnVector = thisVector;
            tempReturnVector.Normalize();
            return tempReturnVector;
        }

        public static Vector2 PixelPosition(this Vector2 thisVector)
            => ((thisVector * 8).RoundToPoint()).ToVector2() * 0.125f;

        public static Vector2 Lerp(this Vector2 thisVector, Vector2 aTarget, float aValue) 
            => thisVector + (aTarget - thisVector) * aValue;
    }
}
