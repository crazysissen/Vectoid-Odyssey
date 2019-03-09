using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    // TEMPORARY SOLUTION
    class MenuManager
    {
        public UpdateManager myUpdateManager;

        // Main menu
        public GUI.Collection myMainMenu, myOptionsMenu, myPlayMenu;

        //In Game
        public GUI.Collection myPauseMenu, myHUD, myUpgradeMenu;

        public MenuManager(UpdateManager anUpdateManager)
        {
            myUpdateManager = anUpdateManager;
        }
        
        public void CreateMainMenu()
        {

        }
    }
}
