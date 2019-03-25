using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class RoomBounds
    {
        public Vector2 AccessCenter { get; private set; }

        private float
            myLeftWall, myLeftFloor, myLeftCeiling,
            myFloor, myCeiling,
            myRightWall, myRightFloor, myRightCeiling;

        public RoomBounds(float aLWall, float aLFloor, float aLCeiling, float aFloor, float aCeiling, float aRWall, float aRFloor, float aRCeliling)
        {
            myLeftWall = aLWall;
            myLeftFloor = aLFloor;
            myLeftCeiling = aLCeiling;

            myFloor = aFloor;
            myCeiling = aCeiling;

            myRightWall = aRWall;
            myRightFloor = aRFloor;
            myRightCeiling = aRCeliling;

            AccessCenter = new Vector2(0.5f * (myLeftWall + myRightWall), 0.5f * (myCeiling + myFloor));
        }

        public bool InRoom(Vector2 aPosition)
            => aPosition.X > myLeftWall && aPosition.X < myRightWall && aPosition.Y > myCeiling && aPosition.Y < myFloor;

        public Vector2 Correction(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            bool
                tempOnLeft = aBottomRight.Y < myLeftFloor, tempOn = aBottomRight.Y < myFloor, tempOnRight = aBottomRight.Y < myRightFloor,
                tempUnderLeft = aTopLeft.Y > myLeftCeiling, tempUnder = aTopLeft.Y > myCeiling, tempUnderRight = aTopLeft.Y > myRightCeiling,
                inLeft = aTopLeft.X > myLeftWall, inRight = aBottomRight.X < myRightWall;

            // If within main room
            if (inLeft && inRight)
            {
                if (!tempOn)
                {
                    return new Vector2(0, myFloor - aBottomRight.Y);
                }

                if (!tempUnder)
                {
                    return new Vector2(0, myCeiling - aTopLeft.Y);
                }

                return Vector2.Zero;
            }

            return Vector2.Zero;

            //Vector2 tempVector = new Vector2();

            //if (!inle)
        }
    }
}
