using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    struct RoomBounds
    {
        public float
            leftWall, leftFloor, leftCeiling,
            floor, ceiling,
            rightWall, rightFloor, rightCeiling;

        public RoomBounds(float aLWall, float aLFloor, float aLCeiling, float aFloor, float aCeiling, float aRWall, float aRFloor, float aRCeliling)
        {
            leftWall = aLWall;
            leftFloor = aLFloor;
            leftCeiling = aLCeiling;

            floor = aFloor;
            ceiling = aCeiling;

            rightWall = aRWall;
            rightFloor = aRFloor;
            rightCeiling = aRCeliling;
        }

        public Vector2 Correction(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            bool
                tempOnLeft = aBottomRight.Y < leftFloor, tempOn = aBottomRight.Y < floor, tempOnRight = aBottomRight.Y < rightFloor,
                tempUnderLeft = aTopLeft.Y > leftCeiling, tempUnder = aTopLeft.Y > ceiling, tempUnderRight = aTopLeft.Y > rightCeiling,
                inLeft = aTopLeft.X > leftWall, inRight = aBottomRight.X < rightWall;

            // If within main room
            if (inLeft && inRight)
            {
                if (!tempOn)
                {
                    return new Vector2(0, floor - aBottomRight.Y);
                }

                if (!tempUnder)
                {
                    return new Vector2(0, ceiling - aTopLeft.Y);
                }

                return Vector2.Zero;
            }

            return Vector2.Zero;

            //Vector2 tempVector = new Vector2();

            //if (!inle)
        }
    }
}
