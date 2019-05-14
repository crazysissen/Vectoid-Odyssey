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

        private readonly float
            myLeftWall, myLeftFloor, myLeftCeiling,
            myFloor, myCeiling,
            myRightWall, myRightFloor, myRightCeiling;

        public RoomBounds(float aLWall, float aLFloor, float aLCeiling, float aFloor, float aCeiling, float aRWall, float aRFloor, float aRCeliling)
        {
            myLeftWall = aLWall * 2;
            myLeftFloor = aLFloor * 2;
            myLeftCeiling = aLCeiling * 2;

            myFloor = aFloor * 2;
            myCeiling = aCeiling * 2;

            myRightWall = aRWall * 2;
            myRightFloor = aRFloor * 2;
            myRightCeiling = aRCeliling * 2;

            AccessCenter = new Vector2(0.5f * (myLeftWall + myRightWall), 0.5f * (myCeiling + myFloor));
        }

        public bool InRoom(Vector2 aPosition)
            => aPosition.X > myLeftWall && aPosition.X < myRightWall && aPosition.Y > myCeiling && aPosition.Y < myFloor;

        public Vector2 Correction(Vector2 aTopLeft, Vector2 aBottomRight, bool anUpwardsBool)
        {
            bool tempOn = aBottomRight.Y < myFloor, tempUnder = aTopLeft.Y > myCeiling;

            Vector2 tempAdditive = new Vector2();

            // If within main room
            if (aBottomRight.X > myLeftWall && aTopLeft.X < myRightWall)
            {
                if (!tempOn)
                {
                    tempAdditive += new Vector2(0, myFloor - aBottomRight.Y);
                }

                if (!tempUnder)
                {
                    tempAdditive += new Vector2(0, myCeiling - aTopLeft.Y);
                }
            }
            //else
            //{
            //    return Vector2.Zero;
            //}

            bool tempCompletelyInLeft = aTopLeft.X >= myLeftWall, tempCompletelyInRight = aBottomRight.X <= myRightWall;

            if (tempCompletelyInLeft && tempCompletelyInRight)
            {
                return anUpwardsBool ? tempAdditive * new Vector2(0, 1) : new Vector2(tempAdditive.X, tempAdditive.Y.Min(0));
            }

            if (!tempCompletelyInLeft)
            {
                if (aTopLeft.Y + tempAdditive.Y < myLeftCeiling)
                {
                    tempAdditive += (myLeftWall - aTopLeft.X > myLeftCeiling - aTopLeft.Y) ? new Vector2(0, myLeftCeiling - aTopLeft.Y) : new Vector2(myLeftWall - aTopLeft.X, 0);
                }

                if (aBottomRight.Y + tempAdditive.Y > myLeftFloor)
                {
                    tempAdditive += (myLeftWall - aTopLeft.X > aBottomRight.Y - myLeftFloor) ? 
                        new Vector2(0, myLeftFloor - aBottomRight.Y) : 
                        new Vector2(myLeftWall - aTopLeft.X, 0);
                }

                return anUpwardsBool ? tempAdditive * new Vector2(0, 1) : new Vector2(tempAdditive.X, tempAdditive.Y.Min(0));
            }

            if (!tempCompletelyInRight)
            {
                if (aTopLeft.Y + tempAdditive.Y < myRightCeiling)
                {
                    tempAdditive += (aBottomRight.X - myRightWall > myRightCeiling - aTopLeft.Y) ? new Vector2(0, myRightCeiling - aTopLeft.Y) : new Vector2(myRightWall - aBottomRight.X, 0);
                }

                if (aBottomRight.Y + tempAdditive.Y > myRightFloor)
                {
                    tempAdditive += (aBottomRight.X - myRightWall > aBottomRight.Y - myRightFloor) ? new Vector2(0, myRightFloor - aBottomRight.Y) : new Vector2(myRightWall - aBottomRight.X, 0);
                }

                return anUpwardsBool ? tempAdditive * new Vector2(0, 1) : new Vector2(tempAdditive.X, tempAdditive.Y.Min(0));
            }

            return Vector2.Zero;
        }
    }
}
