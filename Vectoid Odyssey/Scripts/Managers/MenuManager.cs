using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
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
        GUI.Button bStart, bOptions, bQuit, bSewer, bArkan;
        #endregion

        //In Game
        public GUI.Collection myPauseMenu, myHUD, myUpgradeMenu;
        #region Elements
        #endregion

        public MenuManager(UpdateManager anUpdateManager)
        {
            myUpdateManager = anUpdateManager;
            myMainCollection = new GUI.Collection(true);
        }

        #region MainMenu

        public void CreateMainMenu()
        {
            Point tempRes = VectoidOdyssey.AccessResolution, tempGameRes = VectoidOdyssey.GetGameResolution;

            myMainMenu = new GUI.Collection(false);
            myMainMenu.AccessOrigin = new Point(30, 30);

            myOptionsMenu = new GUI.Collection(false);

            myPlayMenu = new GUI.Collection(false);

            myMainCollection.Add(myMainMenu, myOptionsMenu, myPlayMenu);

            tTitle = new Renderer.Text(Layer.GUI, Font.Bold, "Vectoid: ODYSSEY", 12, 0, Vector2.Zero, Vector2.Zero, new Color(203, 243, 130));

            Color tempButtonColor = new Color(81, 162, 0);

            bStart = new GUI.Button(Layer.GUI, new Rectangle(0, 240, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bStart.AddText("Start", 4, true, Color.White, Font.Bold);
            bStart.OnClick += MainMenuStart;

            bOptions = new GUI.Button(Layer.GUI, new Rectangle(0, 340, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bOptions.AddText("Options", 4, true, Color.White, Font.Bold);
            bOptions.OnClick += MainMenuOptions;

            bQuit = new GUI.Button(Layer.GUI, new Rectangle(0, 440, 200, 80), tempButtonColor) { AccessScaleEffect = true };
            bQuit.AddText("Quit", 4, true, Color.White, Font.Bold);
            bQuit.OnClick += MainMenuQuit;

            myMainMenu.Add(tTitle, bStart, bOptions, bQuit);
        }

        private void MainMenuStart()
        {
            myMainMenu.AccessActive = false;

            myUpdateManager.StartGame();
        }

        private void MainMenuOptions()
        {

        }

        private void MainMenuQuit()
        {
            VectoidOdyssey.Quit();
        }

        #endregion
    }
}
