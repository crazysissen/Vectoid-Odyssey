using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    class UpdateManager
    {
        private VectoidOdyssey myGame; // it really is!!1

        private InGameManager myInGameManager;
        private MenuManager myMenuManager;

        public UpdateManager(VectoidOdyssey aGame)
        {
            myGame = aGame;
        }

        public void Init()
        {
            MusicManager.Init();

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
