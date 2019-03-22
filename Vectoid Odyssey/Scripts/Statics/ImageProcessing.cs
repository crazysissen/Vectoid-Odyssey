using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    static class ImageProcessing
    {
        static GraphicsDeviceManager graphics;

        public static void Init(GraphicsDeviceManager aGraphics)
            => graphics = aGraphics;

        public class TextureAssembly
        {
            private int myHeight, myWidth;
            private List<TexturePosition> myTextures = new List<TexturePosition>();

            public TextureAssembly(int aWidth, int aHeight)
            {
                myWidth = aWidth;
                myHeight = aHeight;
            }

            public void Add(Texture2D aTexture, Point aPosition)
            {

            }

            public Texture2D Assemble()
            {
                Texture2D tempTexture = new Texture2D(graphics.GraphicsDevice, myWidth, myHeight);

                return tempTexture;
            }

            public struct TexturePosition
            {
                public Texture2D t;
                public Point p;
            }
        }
    }
}
