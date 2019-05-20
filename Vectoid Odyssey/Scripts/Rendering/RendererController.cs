using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DCOdyssey
{
    class RendererController
    {
        private static RendererController mainController;

        public static Camera AccessCamera { get; private set; }
        public static GUI AccessGUI { get; set; }

        private List<Renderer> myRenderers = new List<Renderer>();
        private Color myBackgroundColor;

        /// <param name="aCameraScale">At 1.0f zoom the screen resolution is 20x12 pixels</param>
        public void Init(GraphicsDeviceManager aGraphicsDeviceManager, Vector2 aCameraPosition, float aCameraScale, Color aBackgroundColor)
        {
            mainController = this;

            AccessGUI = new GUI();

            AccessCamera = new Camera(aGraphicsDeviceManager)
            {
                AccessPosition = aCameraPosition,
                AccessScale = aCameraScale
            };

            myBackgroundColor = aBackgroundColor;
        }

        // Called every frame. Main rendering/drawing 
        public void Draw(GraphicsDeviceManager aGraphicsDeviceManager, SpriteBatch aSpriteBatch, GameTime aGameTime, float aDeltaTime)
        {
            aGraphicsDeviceManager.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, myBackgroundColor, 0, 0);

            MouseState tempMouseState = Input.GetMouseState;
            KeyboardState tempKeyboardState = Input.GetKeyboardState;

            aSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            // Gather all drawables, aka all renderers and GUI elements

            IGUIMember[] tempGuiMembers = GUI.GetMembers(AccessGUI, tempMouseState, tempKeyboardState, (float)aGameTime.ElapsedGameTime.TotalSeconds, Point.Zero);

            List<object> tempAllDrawables = new List<object>();

            tempAllDrawables.AddRange(myRenderers.Where(o => o.AccessAutomaticDraw));
            tempAllDrawables.AddRange(tempGuiMembers);

            // Order drawables by layer using 

            tempAllDrawables = tempAllDrawables.OrderBy(o => (o is IGUIMember) ? (o as IGUIMember).AccessLayer.GetDepth : (o as Renderer).AccessLayer.GetDepth).ToList();

            foreach (object drawable in tempAllDrawables)
            {
                if (drawable is Renderer)
                {
                    Renderer renderer = (drawable as Renderer);

                    if (renderer.AccessActive)
                    {
                        renderer.Draw(aSpriteBatch, AccessCamera, aDeltaTime);
                    }

                    continue;
                }

                (drawable as IGUIMember).Draw(aSpriteBatch, tempMouseState, tempKeyboardState, aDeltaTime);
            }

            aSpriteBatch.End();

            var tempMatrix = Matrix.CreateOrthographicOffCenter(0,
                aGraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
                aGraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight,
                0, 0, 1);

            var a = new AlphaTestEffect(aGraphicsDeviceManager.GraphicsDevice)
            {
                Projection = tempMatrix
            };
        }

        public static void AddRenderer(Renderer renderer)
            => mainController.myRenderers.Add(renderer);

        public static void RemoveRenderer(Renderer renderer)
            => mainController.myRenderers.Remove(renderer);
    }
}
