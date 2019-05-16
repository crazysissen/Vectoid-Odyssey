using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VectoidOdyssey
{
    sealed class InGameManager
    {
        const float
            MOUSELERP = 0.15f;

        static public InGameManager AccessMain { get; private set; }

        public bool AccessUpdateObjects { get; set; }

        private UpdateManager myUpdateManager;
        private MenuManager myMenuManager;
        private PauseManager myPauseManager;

        private List<WorldObject> myObjects, myRemoveQueue, myAddQueue;

        private Map myMap;
        private Player myPlayer;
        private int myLastIndex = -1;
        private float myTimeMultiplier;
        private bool myInUpdate, myPaused;

        public InGameManager(UpdateManager anUpdateManager, MenuManager aMenuManager)
        {
            AccessMain = this;

            myUpdateManager = anUpdateManager;
            myMenuManager = aMenuManager;

            myObjects = new List<WorldObject>();
            myRemoveQueue = new List<WorldObject>();
            myAddQueue = new List<WorldObject>();
        }

        public void InitGame(Map aMap)
        {
            PlayerSetup tempSetup = new PlayerSetup()
            {
                sheet = Load.Get<Texture2D>("Player"),
                maxSpeed = 2.0f,
                acceleration = 1.3f,
                brakeAcceleration = 2.0f,
                maxJumpTime = 0.35f,
                jumpStartAcceleration = 4.2f,
                jumpEndAcceleration = 3.3f,
                health = 100,
                nonLinear = 8.5f,
                weapons = new PlayerWeapon[]
                {
                    new PlayerWeapon.Teal(), new PlayerWeapon.Red()
                }
            };

            MusicManager.Play("Sludge");

            myMap = aMap;
            myMap.ActivateEnemies();

            myPlayer = myMap.SpawnPlayer(tempSetup, myMenuManager);

            myPauseManager = new PauseManager();
        }

        public void Update(float aDeltaTime)
        {
            if (AccessUpdateObjects)
            {
                //Console.WriteLine(RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()));

                myInUpdate = true;

                foreach (WorldObject current in myObjects)
                {
                    current.UpdateDynamic(aDeltaTime);
                }

                UpdateCollision(aDeltaTime);

                foreach (WorldObject current in myObjects)
                {
                    if (current.AccessActive)
                    {
                        current.BaseUpdate(aDeltaTime);
                    }
                }

                foreach (WorldObject current in myRemoveQueue)
                {
                    myObjects.Remove(current);
                }

                myObjects.AddRange(myAddQueue);

                myAddQueue.Clear();
                myRemoveQueue.Clear();

                myInUpdate = false;
            }

            SetCamera();
        }

        public void Add(params WorldObject[] addQuery)
        {
            if (myInUpdate)
            {
                myAddQueue.AddRange(addQuery);

                return;
            }

            myObjects.AddRange(addQuery);
        }

        public void Remove(params WorldObject[] removeQuery)
        {
            if (myInUpdate)
            {
                myRemoveQueue.AddRange(removeQuery);

                return;
            }

            foreach (WorldObject current in removeQuery)
            {
                myObjects.Remove(current);
            }
        }

        public void Destroy()
        {
            while (myObjects.Count > 0)
            {
                myObjects[0].Destroy();
            }

            myObjects = null;
            myAddQueue = null;
            myRemoveQueue = null;
        }

        private void UpdateCollision(float aDeltaTime)
        {
            foreach (WorldObject current in myObjects)
            {
                current.SimpleCollision(aDeltaTime, true);
                current.UpdateHitDetector();
            }

            HitDetector.UpdateAll();

            foreach (WorldObject current in myObjects)
            {
                current.SimpleCollision(aDeltaTime, false);
            }
        }

        public RoomBounds GetCurrentBounds(Vector2 aPosition, int? anObjectIndex = null)
            => (anObjectIndex == null) ? myMap.GetRoom(aPosition) : myMap.GetRoom(aPosition, anObjectIndex.Value);

        public int GetNewIndex()
            => ++myLastIndex;

        private void SetCamera()
        {
            RendererController.AccessCamera.AccessPosition = new Vector2(myPlayer.AccessPosition.PixelPosition().X, myPlayer.AccessPosition.PixelPosition().Y).Lerp(RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()), MOUSELERP);
        }
    }
}
