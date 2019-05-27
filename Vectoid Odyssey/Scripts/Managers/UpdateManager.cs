using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCOdyssey
{
    class UpdateManager
    {
        private DCOdyssey myGame; // it really is!!1

        private SplashHandler mySplashHandler;
        private InGameManager myInGameManager;
        private MenuManager myMenuManager;

        public UpdateManager(DCOdyssey aGame)
        {
            myGame = aGame;
        }

        public void Init()
        {
            Music.Init();

            myMenuManager = new MenuManager(this);

            myMenuManager.CreateMainMenu(); 
        }

        public void Update(float aDeltaTime)
        {
            if (myInGameManager != null)
            {
                myInGameManager.Update(aDeltaTime);
            }
        }

        public void StartGame(MapFetcher.MapType aMapType, object[] arg)
        {
            MapFetcher tempMapFetcher = new MapFetcher();

            myInGameManager = new InGameManager(this, myMenuManager) { AccessUpdateObjects = true };
            myInGameManager.InitGame(tempMapFetcher.Get(aMapType, arg));
        }

        public void ExitGame()
        {

        }
    }
}
