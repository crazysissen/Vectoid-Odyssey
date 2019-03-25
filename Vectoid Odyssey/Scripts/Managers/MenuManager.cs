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

        // Main menu
        public GUI.Collection myMainMenu, myOptionsMenu, myPlayMenu;
        #region Elements
        Renderer.SpriteScreen tTitle;
        GUI.Button bStart, bOptions, bQuit, bSewer, bArkan;
        #endregion

        //In Game
        public GUI.Collection myPauseMenu, myHUD, myUpgradeMenu;
        #region Elements
        #endregion

        public MenuManager(UpdateManager anUpdateManager)
        {
            myUpdateManager = anUpdateManager;
        }
        
        public void CreateMainMenu()
        {
            Point tempRes = VectoidOdyssey.AccessResolution, tempGameRes = VectoidOdyssey.GetGameResolution;

            
            tTitle =  
        }
    }
}
