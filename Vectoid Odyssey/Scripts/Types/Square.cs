using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class Square
    {
        public static bool DoubleAll { get; set; }

        public Vector2 GetPosition => new Vector2(myLeft, myTop);
        public Vector2 GetSize => new Vector2(myRight - myLeft, myBottom - myTop);
        public Vector2 GetBRPosition => new Vector2(myRight, myBottom);

        private float myLeft, myRight, myTop, myBottom;

        public Square(float x, float y, float aWidth, float aHeight)
        {
            int tempMultiplier = DoubleAll ? 2 : 1;

            myLeft = x * tempMultiplier;
            myTop = y * tempMultiplier;
            myRight = (x + aWidth) * tempMultiplier;
            myBottom = (y + aHeight) * tempMultiplier;
        }

        public Square(Vector2 aTopLeft, Vector2 aBottomRight) : this(aTopLeft.X, aTopLeft.Y, aBottomRight.X - aTopLeft.X, aBottomRight.Y - aTopLeft.Y)
        { }
    }
}
