using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    static class ImageProcessing
    {
        static GraphicsDeviceManager graphics;

        public static void Init(GraphicsDeviceManager aGraphics)
            => graphics = aGraphics;

        public static Texture2D NewWithReplacedColors(Texture2D aTexture, params (Color oldColor, Color newColor)[] someColorPairs)
        {
            Texture2D tempNewTexture = new Texture2D(aTexture.GraphicsDevice, aTexture.Width, aTexture.Height);

            Color[] tempColors = new Color[aTexture.Width * aTexture.Height];
            aTexture.GetData(tempColors);

            foreach ((Color oldColor, Color newColor) in someColorPairs)
            {
                for (int i = 0; i < tempColors.Length; ++i)
                {
                    if (tempColors[i] == oldColor)
                    {
                        tempColors[i] = newColor;
                    }
                }
            }

            tempNewTexture.SetData(tempColors);

            return tempNewTexture;
        }

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
