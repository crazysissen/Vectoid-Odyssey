using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class Camera
    {
        const float
            UNIVERSALMODIFIER = 1.0f;

        // A square based on the average distances to the screen edges, divided into pieces. This makes the scale optimally similar across screen dimensions and formats.
        public const int
            WORLDUNITPIXELS = 8;

        public Vector2 AccessPosition { get; set; }
        public float AccessScale { get; set; }

        public Vector2 CenterCoordinate { get; private set; }

        private float myStandardScaleMultiplier, myStandardSquareDiameter;

        public Camera(GraphicsDeviceManager graphics)
        {
            int tempScreenWidth = graphics.PreferredBackBufferWidth,
                tempScreenHeight = graphics.PreferredBackBufferHeight;

            myStandardSquareDiameter = 0.5f * (tempScreenWidth + tempScreenHeight);

            myStandardScaleMultiplier = myStandardSquareDiameter / (WORLDUNITPIXELS * 2);

            CenterCoordinate = new Vector2(tempScreenWidth * 0.5f, tempScreenHeight * 0.5f);
        }

        public Vector2 WorldToScreenPosition(Vector2 worldPosition)
            => CenterCoordinate + (worldPosition - AccessPosition) * myStandardSquareDiameter * 0.5f * AccessScale * UNIVERSALMODIFIER;

        public Vector2 ScreenToWorldPosition(Vector2 screenPosition)
            => (screenPosition - CenterCoordinate) / (myStandardSquareDiameter * 0.5f * AccessScale * UNIVERSALMODIFIER) + AccessPosition;

        public Vector2 WorldToScreenSize(Vector2 size)
            => size * UNIVERSALMODIFIER * myStandardScaleMultiplier * AccessScale;

        public Vector2 ScreenToWorldSize(Vector2 size)
            => size / (UNIVERSALMODIFIER * AccessScale * myStandardScaleMultiplier);
    }
}
