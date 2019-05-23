using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    // TEMPORARY SOLUTION
    class MenuManager
    {
        public UpdateManager myUpdateManager;

        public GUI.Collection myMainCollection;

        // Main menu
        public GUI.Collection myMainMenu, myOptionsMenu, myPlayMenu;
        #region Elements
        Renderer.Text tTitle;
        GUI.Button bStart, bStart2, bOptions, bQuit, bSewer, bArkan;
        #endregion

        //In Game
        public GUI.Collection myHUD, myHUDExpanded/*, myUpgradeMenu*/;
        GUI.MaskedCollection myHPBar, myAmmoBar;
        #region Elements
        Renderer.SpriteScreen sHudBig, sHudSmall, sHudStats;
        Renderer.SpriteScreenFloating[] sWeapons, sItems;
        GUI.Button bResume, bHudOptions, bExit;
        GUI.Button[] bWeapons;
        #endregion

        public MenuManager(UpdateManager anUpdateManager)
        {
            myUpdateManager = anUpdateManager;
            myMainCollection = new GUI.Collection(true);
        }

        #region MainMenu

        public void CreateMainMenu()
        {
            Point tempRes = DCOdyssey.AccessResolution, tempGameRes = DCOdyssey.GetGameResolution;

            myMainMenu = new GUI.Collection(false);
            myMainMenu.AccessOrigin = new Point(80, 40);

            myOptionsMenu = new GUI.Collection(false);

            myPlayMenu = new GUI.Collection(false);

            myMainCollection.Add(myMainMenu, myOptionsMenu, myPlayMenu);

            tTitle = new Renderer.Text(Layer.GUI, Font.Bold, "Vectoid: ODYSSEY", 12, 0, Vector2.Zero, Vector2.Zero, new Color(203, 243, 130));

            Color tempButtonColor = new Color(81, 162, 0);

            bStart = new GUI.Button(Layer.GUI, new Rectangle(0, 240, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bStart.AddText("Start 1", 4, true, Color.White, Font.Bold);
            bStart.OnClick += MainMenuStart;

            bStart2 = new GUI.Button(Layer.GUI, new Rectangle(240, 240, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bStart2.AddText("Start 2", 4, true, Color.White, Font.Bold);
            bStart2.OnClick += MainMenuStart2;

            bOptions = new GUI.Button(Layer.GUI, new Rectangle(0, 340, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bOptions.AddText("Options", 4, true, Color.White, Font.Bold);
            bOptions.OnClick += MainMenuOptions;

            bQuit = new GUI.Button(Layer.GUI, new Rectangle(0, 440, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bQuit.AddText("Quit", 4, true, Color.White, Font.Bold);
            bQuit.OnClick += MainMenuQuit;

            myMainMenu.Add(tTitle, bStart, bStart2, bOptions, bQuit);
        }

        private void MainMenuStart()
        {
            myMainMenu.AccessActive = false;

            myUpdateManager.StartGame(MapFetcher.MapType.Sewer1, null);
        }

        private void MainMenuStart2()
        {
            myMainMenu.AccessActive = false;

            myUpdateManager.StartGame(MapFetcher.MapType.Sewer2, null);
        }

        private void MainMenuOptions()
        {

        }

        private void MainMenuQuit()
        {
            DCOdyssey.Quit();
        }

        #endregion

        #region HUD

        public void CreateHUD()
        {
            myHUD = new GUI.Collection(false);
            myHUDExpanded = new GUI.Collection(false);
            myMainCollection.Add(myHUD, myHUDExpanded);

            sHudBig = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 10), Load.Get<Texture2D>("InGameMenu"), ScreenRectangle(100, 0, 280, 60), Color.White);
            sHudSmall = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("InGameMenuSmall"), ScreenRectangle(100, 0, 280, 60), Color.White);
            sHudStats = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 0), Load.Get<Texture2D>("InGameHUD"), ScreenRectangle(0, 203, 66, 67), Color.White);

            //myHPBar = new GUI.MaskedCollection() { Mask = new Mask( }
            myAmmoBar = new GUI.MaskedCollection();

            myHUD.Add(sHudSmall, sHudStats);
            //myHUDExpanded.Add(sHudBig, );
        }

        #endregion

        public static Point ScreenPoint(Point aGamePoint) => (aGamePoint.ToVector2() * DCOdyssey.GetScreenPoint).RoundToPoint();
        public static Point ScreenPoint(int x, int y) => ScreenPoint(new Point(x, y));
        public static Rectangle ScreenRectangle(int x, int y, int aWidth, int aHeight) => new Rectangle(ScreenPoint(x, y), ScreenPoint(aWidth, aHeight));
    }
}
