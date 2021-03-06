﻿using System;
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

        private static List<(GUIContainerMasked, Point)> myMasks = new List<(GUIContainerMasked, Point)>();

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

            myMasks.Clear();

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

            #region Mask Rendering

            // Creating a matrix, alpha effect is necessary for the mask system to work

            Matrix tempMatrix = Matrix.CreateOrthographicOffCenter(0,
                aGraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
                aGraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight,
                0, 0, 1);

            AlphaTestEffect testAlpha = new AlphaTestEffect(aGraphicsDeviceManager.GraphicsDevice)
            {
                Projection = tempMatrix
            };

            MouseState tempMState = Input.GetMouseState;
            KeyboardState tempKState = Input.GetKeyboardState;

            int tempIterations = 1; // Why is this not a for-loop? Your guess is as good as mine.
            foreach ((GUIContainerMasked mask, Point position) in myMasks)
            {
                DepthStencilState
                    tempStencilA = new DepthStencilState
                    {
                        StencilEnable = true,
                        StencilFunction = CompareFunction.Always,
                        StencilPass = StencilOperation.Replace,
                        ReferenceStencil = tempIterations,
                        DepthBufferEnable = false,
                    },

                    tempStencilB = new DepthStencilState
                    {
                        StencilEnable = true,
                        StencilFunction = CompareFunction.LessEqual,
                        StencilPass = StencilOperation.Keep,
                        ReferenceStencil = tempIterations,
                        DepthBufferEnable = true,
                    };

                Texture2D tempTransparent = new Texture2D(aGraphicsDeviceManager.GraphicsDevice, 1, 1);
                tempTransparent.SetData(new Color[] { Color.Transparent });

                // First render a 0-opaque back buffer and the according render mask

                aSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, tempStencilA, null, testAlpha);

                aSpriteBatch.Draw(tempTransparent, new Rectangle(0, 0, aGraphicsDeviceManager.PreferredBackBufferWidth, aGraphicsDeviceManager.PreferredBackBufferHeight), Color.Black);
                aSpriteBatch.Draw(mask.Mask.AccessTexture, new Rectangle(mask.Mask.AccessRectangle.Location + mask.AccessOrigin, mask.Mask.AccessRectangle.Size), Color.Transparent);

                aSpriteBatch.End();

                // Render every consequential member in order of layer within the mask

                aSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, tempStencilB, null, null);

                IGUIMember[] tempMaskGuiMembers = GUI.GetMembers(mask, tempMState, tempKState, aDeltaTime, mask.AccessOrigin).OrderBy(o => o.AccessLayer.GetDepth).ToArray();

                foreach (IGUIMember guiMember in tempMaskGuiMembers)
                {
                    guiMember.Draw(aSpriteBatch, tempMState, tempKState, aDeltaTime);
                }

                aSpriteBatch.End();

                // Iterating the layer number so that multiple simultanious masks are possible

                ++tempIterations;
            }

            #endregion
        }

        public static void AddRenderer(Renderer renderer)
            => mainController.myRenderers.Add(renderer);

        public static void RemoveRenderer(Renderer renderer)
            => mainController.myRenderers.Remove(renderer);

        public static void TemporaryAddMask(GUIContainerMasked aMask, Point anOrigin)
            => myMasks.Add((aMask, anOrigin));
    }
}
