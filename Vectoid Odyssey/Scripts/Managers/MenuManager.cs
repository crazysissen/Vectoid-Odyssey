using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    class MenuManager
    {
        private UpdateManager myUpdateManager;

        // Main menu
        private GUI.Collection myMainMenu, myOptionsMenu, myPlayMenu;

        //In Game
        private GUI.Collection myPauseMenu, myHUD, myUpgradeMenu;

        public MenuManager(UpdateManager anUpdateManager)
        {
            myUpdateManager = anUpdateManager;
        }
        
        public void CreateMainMenu()
        {

        }
    }
}
