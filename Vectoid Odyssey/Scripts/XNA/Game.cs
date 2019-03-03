using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VectoidOdyssey
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // XNA
        GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;

        // LOCAL
        RendererController myRendererController;
        UpdateController myUpdateController;

        Renderer re;
        float t;

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            re = new Renderer();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            Load.ImportAll(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            t += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (t > 5)
            {
                re = null;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            base.Draw(gameTime);
        }
    }
}
