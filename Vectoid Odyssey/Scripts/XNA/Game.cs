using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VectoidOdyssey
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class VectoidOdyssey : Game
    {
        static public Point GetGameResolution => new Point(512, 288);
        static public Point AccessResolution
        {
            get => new Point(mainGame.myGraphics.PreferredBackBufferWidth, mainGame.myGraphics.PreferredBackBufferHeight);

            set
            {
                mainGame.myGraphics.PreferredBackBufferWidth = value.X;
                mainGame.myGraphics.PreferredBackBufferHeight = value.Y;

                mainGame.myGraphics.ApplyChanges();
            }
        }

        // SINGLETON
        static private VectoidOdyssey mainGame;

        // XNA
        GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;

        // LOCAL
        RendererController myRendererController;
        UpdateManager myUpdateManager;

        public VectoidOdyssey()
        {
            mainGame = this;

            Point tempRes = GetGameResolution;

            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = tempRes.X * 3,
                PreferredBackBufferHeight = tempRes.Y * 3,
            };

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            Font.Init();

            myRendererController = new RendererController();
            myUpdateManager = new UpdateManager(this);

            myRendererController.Init(myGraphics, Vector2.Zero, 20.0f / GetGameResolution.X, Color.Black);
            myUpdateManager.Init();
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            Load.ImportAll(Content);
        }

        protected override void Update(GameTime aGameTime)
        {
            float tempDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            Input.Update();

            myUpdateManager.Update(tempDeltaTime);

            base.Update(aGameTime);
        }

        protected override void Draw(GameTime aGameTime)
        {
            float tempDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            myRendererController.Draw(myGraphics, mySpriteBatch, aGameTime, tempDeltaTime);

            base.Draw(aGameTime);
        }
    }
}
