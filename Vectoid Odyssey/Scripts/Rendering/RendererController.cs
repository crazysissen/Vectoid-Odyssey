using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class RendererController
    {
        private List<Renderer> myRenderers;

        private bool myChangedFlag;

        public RendererController()
        {
            myRenderers = new List<Renderer>();
            myChangedFlag = false;
        }

        public void Draw(SpriteBatch aSpriteBatch, GraphicsDeviceManager aGraphicsDeviceManager, float aDeltaTime)
        {

        }

        public void AddRenderer(Renderer aRenderer)
        {

        }

        public void RemoveRenderer(Renderer aRenderer)
        {

        }
    }
}
