using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Renderer
    {
        static private RendererController controller;

        public event Action<SpriteBatch, float> OnRender;

        public float GetDepth => myLayer.GetDepth;

        private Layer myLayer;

        public Renderer() : this(Layer.Default, null) { }

        public Renderer(Layer aLayer) : this(aLayer, null) { }

        public Renderer(Action<SpriteBatch, float> aQueue) : this(Layer.Default, aQueue) { }

        public Renderer(Layer aLayer, Action<SpriteBatch, float> aQueue)
        {
            myLayer = aLayer;

            if (aQueue != null)
            {
                OnRender += aQueue;
            }

            controller?.AddRenderer(this);
        }

        public void Render(SpriteBatch aSpriteBatch, float aDeltaTime)
        {
            OnRender?.Invoke(aSpriteBatch, aDeltaTime);
            Console.WriteLine("Rendering");
        }

        public void SetLayer(Layer aLayer)
        {
            myLayer = aLayer;
            controller.ChangedState();
        }

        public void Remove() 
            => controller.RemoveRenderer(this);

        // Static initialization setting up a static reference to the renderer controller
        public static void StaticInit(RendererController aController) 
            => controller = aController;
    }
}
