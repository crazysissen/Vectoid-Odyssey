using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class CompleteMap : Map
    {
        private Texture2D myTexture;

        private Renderer.Sprite myRenderer;

        public CompleteMap(Texture2D aTexture, RoomBounds[] someRoomBounds, Vector2 aSpawnPosition)
        {
            myTexture = aTexture;
            myBounds.AddRange(someRoomBounds);

            myRenderer = new Renderer.Sprite(new Layer(MainLayer.Background, -1), myTexture, Vector2.Zero, Vector2.One, Color.White, 0, Vector2.Zero);
        }
    }
}
