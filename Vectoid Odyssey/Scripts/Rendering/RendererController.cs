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
        public static GraphicsDeviceManager AccessManager { get; private set; }

        private List<Renderer> myRenderers;
        private bool myChangedFlag;

        public void Init(GraphicsDeviceManager aGraphicsDeviceManager)
        {
            AccessManager = aGraphicsDeviceManager;

            myRenderers = new List<Renderer>();

            Renderer.StaticInit(this);
        }

        public void Draw(SpriteBatch aSpriteBatch, float aDeltaTime)
        {
            if (myChangedFlag)
            {
                // Sorts renderers by their depth
                myRenderers = myRenderers.OrderBy(o => o?.GetDepth).ToList();
            }

            foreach (Renderer r in myRenderers)
            {
                r.Render(aSpriteBatch, aDeltaTime);
            }

            myChangedFlag = false;
        }

        public void AddRenderer(Renderer aRenderer)
        {
            myRenderers?.Add(aRenderer);
            myChangedFlag = true;
        }

        public void RemoveRenderer(Renderer aRenderer)
            => myRenderers?.Remove(aRenderer);

        public void ChangedState()
            => myChangedFlag = true;
    }
}
