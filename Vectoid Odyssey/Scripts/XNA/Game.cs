using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DCOdyssey
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DCOdyssey : Game
    {
        static public Vector2 GetScreenPoint { get; private set; }
        static public Point GetGameResolution => new Point(480, 270);
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
        static private DCOdyssey mainGame;

        // XNA
        private GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        // LOCAL
        private SplashHandler mySplashHandler;
        private RendererController myRendererController;
        private UpdateManager myUpdateManager;

        public DCOdyssey()
        {
            mainGame = this;

            Point tempRes = GetGameResolution;

            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = tempRes.X * 3,
                PreferredBackBufferHeight = tempRes.Y * 3,
                IsFullScreen = false
            };

            GetScreenPoint = AccessResolution.ToVector2() / GetGameResolution.ToVector2();

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            Font.Init();
            Preferences.Init();
            Sound.Init();

            myRendererController = new RendererController();
            myUpdateManager = new UpdateManager(this);

            myRendererController.Init(myGraphics, Vector2.Zero, 2 / 46.875f, Color.Black);

            mySplashHandler = new SplashHandler();
            mySplashHandler.InitAndPlay(InitUpdate);
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            Load.ImportAll(Content);
        }

        protected override void Update(GameTime aGameTime)
        {
            float tempDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            if (mySplashHandler != null)
            {
                mySplashHandler.Update(tempDeltaTime);
                base.Update(aGameTime);
                return;
            }

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

        public static void Quit()
        {
            mainGame.Exit();
        }

        private void InitUpdate()
        {
            mySplashHandler.Destroy();
            mySplashHandler = null;

            myUpdateManager.Init();
        }
    }
}
