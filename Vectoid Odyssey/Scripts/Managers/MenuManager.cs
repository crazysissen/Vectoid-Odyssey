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
        Renderer.SpriteScreen sDungeon, sCrawlers, sOdyssey, sBackground;
        GUI.Button bStart, bStart2, bOptions, bQuit;
        #endregion

        //In Game
        public GUI.Collection myHUD, myHUDExpanded/*, myUpgradeMenu*/;
        #region Elements
        GUI.MaskedCollection myHPMask;
        Renderer.SpriteScreen sHudBig, sHudSmall, sHudStats, sHPBar;
        Renderer.SpriteScreen[] sItems, sItemsExpanded;
        List<Renderer.SpriteScreenFloating> sWeapons;
        Renderer.SpriteScreenFloating sActiveWeapon;
        Renderer.Text tHP, tHP2, tAmmo, tAmmo2, tPopup;
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
            Music.Play("SongGlitch");

            // Graphic Initialization

            Point tempRes = DCOdyssey.AccessResolution, tempGameRes = DCOdyssey.GetGameResolution;

            myMainMenu = new GUI.Collection(false);
            //myMainMenu.AccessOrigin = new Point(80, 40);

            myOptionsMenu = new GUI.Collection(false);

            myPlayMenu = new GUI.Collection(false);

            myMainCollection.Add(myMainMenu, myOptionsMenu, myPlayMenu);

            //tTitle = new Renderer.Text(Layer.GUI, Font.Bold, "Vectoid: ODYSSEY", 12, 0, Vector2.Zero, Vector2.Zero, new Color(203, 243, 130));

            Color tempButtonColor = new Color(255, 255, 255);

            bStart = new GUI.Button(Layer.GUI, ScreenRectangle(204, 118, 80, 24), Load.Get<Texture2D>("Level1"), tempButtonColor) { AccessScaleEffect = true };
            //bStart.AddText("Start 1", 4, true, Color.White, Font.Bold);
            bStart.OnClick += MainMenuStart;

            bStart2 = new GUI.Button(Layer.GUI, ScreenRectangle(204, 145, 80, 24), Load.Get<Texture2D>("Level2"), tempButtonColor) { AccessScaleEffect = true };
            //bStart2.AddText("Start 2", 4, true, Color.White, Font.Bold);
            bStart2.OnClick += MainMenuStart2;

            bOptions = new GUI.Button(Layer.GUI, ScreenRectangle(204, 172, 80, 24), Load.Get<Texture2D>("Options"), tempButtonColor) { AccessScaleEffect = true };
            //bOptions.AddText("Options", 4, true, Color.White, Font.Bold);
            bOptions.OnClick += MainMenuOptions;

            bQuit = new GUI.Button(Layer.GUI, ScreenRectangle(204, 199, 80, 24), Load.Get<Texture2D>("QuitGame"), tempButtonColor) { AccessScaleEffect = true };
            //bQuit.AddText("Quit", 4, true, Color.White, Font.Bold);
            bQuit.OnClick += MainMenuQuit;

            sDungeon = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("MainLogoDungeon"), ScreenRectangle(116, 11, 108, 30), Color.White);
            sCrawlers = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("MainLogoCrawlers"), ScreenRectangle(234, 11, 128, 30), Color.White);
            sOdyssey = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("MainLogoOdyssey"), ScreenRectangle(180, 43, 128, 34), Color.White);
            sBackground = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -10), Load.Get<Texture2D>("MainMenuBackground"), new Rectangle(Point.Zero, DCOdyssey.AccessResolution), Color.White);

            myMainMenu.Add(/*tTitle, */bStart, bStart2, bOptions, bQuit, sDungeon, sCrawlers, sOdyssey, sBackground);
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
            myHUDExpanded = new GUI.Collection(false) { AccessActive = false };
            myMainCollection.Add(myHUD, myHUDExpanded);

            sItems = new Renderer.SpriteScreen[9];
            for (int i = 0; i < sItems.Length; i++)
            {
                Renderer.SpriteScreen tempRenderer = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("Square"), ScreenRectangle(198 + 13 * i, 3, 8, 8), Color.White) { AccessActive = false };

                sItems[i] = tempRenderer;
                myHUD.Add(tempRenderer);
            }

            sItemsExpanded = new Renderer.SpriteScreen[9];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Renderer.SpriteScreen tempRenderer = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 11), Load.Get<Texture2D>("Square"), ScreenRectangle(335 + 13 * x, 15 + 13 * y, 8, 8), Color.White) { AccessActive = false };

                    sItemsExpanded[x + y * 3] = tempRenderer;
                    myHUDExpanded.Add(tempRenderer);
                }
            }

            sActiveWeapon = new Renderer.SpriteScreenFloating(Layer.GUI, Load.Get<Texture2D>("Teal"), ScreenPoint(177, 13).ToVector2(), DCOdyssey.GetScreenPoint, Color.White, 0, new Vector2(16, 16), SpriteEffects.None) { AccessSourceRectangle = new Rectangle(0, 0, 32, 32) };

            string[] tempWeaponNames = { "Teal", "Red", "Green", "Pink", "LightBlue", "Orange" };
            sWeapons = new List<Renderer.SpriteScreenFloating>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Renderer.SpriteScreenFloating tempRenderer = new Renderer.SpriteScreenFloating(new Layer(MainLayer.GUI, 11), Load.Get<Texture2D>(tempWeaponNames[x + y * 3]), ScreenPoint(117 + x * 25, 14 + y * 25).ToVector2(), DCOdyssey.GetScreenPoint, Color.White, 0, new Vector2(16, 16), SpriteEffects.None) { AccessSourceRectangle = new Rectangle(0, 0, 32, 32) };

                    sWeapons.Add(tempRenderer);
                    myHUDExpanded.Add(tempRenderer);
                }
            }

            tPopup = new Renderer.Text(Layer.GUI, Font.Styled, "", DCOdyssey.GetFontScale * 1.8f, 0, ScreenPoint(240, 50).ToVector2(), Vector2.Zero, new Color(255, 162, 0));

            bResume = new GUI.Button(new Layer(MainLayer.GUI, 11), ScreenRectangle(253, 15, 73, 11), new Color(255, 162, 0));
            bResume.AddText("Close Menu", DCOdyssey.GetFontScale * 0.9f, true, Color.Black, Font.Default);
            bResume.OnClick += Resume;

            bHudOptions = new GUI.Button(new Layer(MainLayer.GUI, 11), ScreenRectangle(253, 27, 73, 11), new Color(255, 162, 0));
            bHudOptions.AddText("Options", DCOdyssey.GetFontScale * 0.9f, true, Color.Black, Font.Default);
            bHudOptions.OnClick += HUDOptions;

            bExit = new GUI.Button(new Layer(MainLayer.GUI, 11), ScreenRectangle(253, 39, 73, 11), new Color(255, 162, 0));
            bExit.AddText("Exit to Desktop", DCOdyssey.GetFontScale * 0.9f, true, Color.Black, Font.Default);
            bExit.OnClick += HUDQuit;

            sHudBig = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, 10), Load.Get<Texture2D>("InGameMenu"), ScreenRectangle(100, 0, 280, 60), Color.White);
            sHudSmall = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -1), Load.Get<Texture2D>("InGameMenuSmall"), ScreenRectangle(100, 0, 280, 60), Color.White);
            sHudStats = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -1), Load.Get<Texture2D>("InGameHUD"), ScreenRectangle(0, 0, 51, 67), Color.White);

            myHPMask = new GUI.MaskedCollection() { Mask = new Mask(Load.Get<Texture2D>("Square"), ScreenRectangle(0, -64, 12, 64), Color.TransparentBlack, false), AccessOrigin = ScreenPoint(2, 66) };
            sHPBar = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("HUDHealthBar"), ScreenRectangle(0, -64, 12, 64), Color.White);
            myHPMask.Add(sHPBar);

            tHP = new Renderer.Text(new Layer(MainLayer.GUI, 1), Font.Small, "10/10", DCOdyssey.GetFontScale * 0.9f, 0, ScreenPoint(17, 1).ToVector2(), Vector2.Zero, Color.Black);
            tHP2 = new Renderer.Text(Layer.GUI, Font.Small, "10/10", DCOdyssey.GetFontScale * 0.9f, 0, ScreenPoint(18, 2).ToVector2(), Vector2.Zero, new Color(121, 121, 121));

            tAmmo = new Renderer.Text(new Layer(MainLayer.GUI, 1), Font.Small, "20", DCOdyssey.GetFontScale * 0.9f, 0, ScreenPoint(17, 14).ToVector2(), Vector2.Zero, Color.Black);
            tAmmo2 = new Renderer.Text(Layer.GUI, Font.Small, "20", DCOdyssey.GetFontScale * 0.9f, 0, ScreenPoint(18, 15).ToVector2(), Vector2.Zero, new Color(121, 121, 121));

            myHUD.Add(sHudSmall, sHudStats, myHPMask, tHP, tHP2, tAmmo, tAmmo2, sActiveWeapon, tPopup);
            myHUDExpanded.Add(sHudBig, bResume, bHudOptions, bExit);
        }

        void Resume()
        {
            Player.AccessMainPlayer.TogglePause();
        }

        void HUDOptions()
        {

        }

        void HUDQuit()
        {
            DCOdyssey.Quit();
        }

        public void SetStats(int hp, int maxHp, int ammo)
        {
            StringBuilder tempStringBuilder = new StringBuilder(hp + "/" + maxHp);
            tHP.AccessString = tempStringBuilder;
            tHP2.AccessString = tempStringBuilder;

            tempStringBuilder = new StringBuilder(ammo.ToString());
            tAmmo.AccessString = tempStringBuilder;
            tAmmo2.AccessString = tempStringBuilder;

            int tempHeight = (int)Math.Round(64f * (float)hp / maxHp);
            myHPMask.Mask = new Mask(Load.Get<Texture2D>("Square"), ScreenRectangle(0, -tempHeight, 12, tempHeight), Color.TransparentBlack, false);
        }

        public void UpdateItems(Item[] someItems, Texture2D anActiveWeaponTexure)
        {
            sActiveWeapon.AccessTexture = anActiveWeaponTexure;

            for (int i = 0; i < 9; i++)
            {
                if (someItems.Length > i)
                {
                    sItems[i].AccessActive = true;
                    sItems[i].AccessSourceRectangle = new Rectangle(0, 0, 8, 8);
                    sItems[i].AccessTexture = someItems[i].GetTexture;
                    sItems[i].AccessColor = someItems[i].AccessColor ?? Color.White;

                    sItemsExpanded[i].AccessActive = true;
                    sItemsExpanded[i].AccessSourceRectangle = new Rectangle(0, 0, 8, 8);
                    sItemsExpanded[i].AccessTexture = someItems[i].GetTexture;
                    sItemsExpanded[i].AccessColor = someItems[i].AccessColor ?? Color.White;

                    continue;
                }

                sItems[i].AccessActive = false;
                sItemsExpanded[i].AccessActive = false;
            }
        }

        public void SetPopup(string aString)
        {
            tPopup.AccessString = new StringBuilder(aString);
            tPopup.AccessOrigin = tPopup.AccessFont.MeasureString(aString) * 0.5f;
        }

        public void OpenPauseMenu()
        {
            myHUDExpanded.AccessActive = true;
        }

        public void ClosePauseMenu()
        {
            myHUDExpanded.AccessActive = false;
        }

        #endregion

        public static Point ScreenPoint(Point aGamePoint) => (aGamePoint.ToVector2() * DCOdyssey.GetScreenPoint).RoundToPoint();
        public static Point ScreenPoint(int x, int y) => ScreenPoint(new Point(x, y));
        public static Rectangle ScreenRectangle(int x, int y, int aWidth, int aHeight) => new Rectangle(ScreenPoint(x, y), ScreenPoint(aWidth, aHeight));
    }
}
