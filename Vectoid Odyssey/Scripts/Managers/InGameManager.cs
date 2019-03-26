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
            CAMERAELEVATION = 6,
            MOUSELERP = 0.15f;

        static public InGameManager AccessMain { get; private set; }

        public bool AccessUpdateObjects { get; set; }

        private UpdateManager myUpdateManager;
        private MenuManager myMenuManager;

        private List<WorldObject> myObjects, myRemoveQueue, myAddQueue;

        private Renderer.Sprite myBackground; // TODO: Implement map type. Temporary solution

        private Map myMap;
        private Player myPlayer;
        private Enemy mySock, mySkull, myCrab; // TODO: Remove temporary enemy tests
        private int lastIndex = -1;
        private bool myInUpdate;

        public InGameManager(UpdateManager anUpdateManager, MenuManager aMenuManager)
        {
            AccessMain = this;

            myUpdateManager = anUpdateManager;
            myMenuManager = aMenuManager;

            myObjects = new List<WorldObject>();
            myRemoveQueue = new List<WorldObject>();
            myAddQueue = new List<WorldObject>();
        }

        public void InitGame()
        {
            PlayerSetup tempSetup = new PlayerSetup()
            {
                sheet = Load.Get<Texture2D>("Player"),
                maxSpeed = 1.6f,
                acceleration = 1.8f,
                brakeAcceleration = 5.0f,
                jumpSpeed = 1.5f,
                maxJumpSpeed = 2.0f,
                health = 10,
                weapons = new PlayerWeapon[6]
                {
                    new PlayerWeapon.Teal(), null, null, null, null, null
                }
            };

            myBackground = new Renderer.Sprite(new Layer(MainLayer.Background, 0), Load.Get<Texture2D>("TempMap"), new Vector2(0, 0), Vector2.One, Color.White, 0, Vector2.One * 256);

            myPlayer = new Player(new Vector2(0, -1), myMenuManager, tempSetup);

            mySkull = new EnemySkull(new Vector2(-26.0f, -2.5f));
            mySock = new EnemySock(new Vector2(18.5f, -0.5f));
            myCrab = new EnemyCrab(new Vector2(-22.0f, -1.0f));
        }

        public void Update(float aDeltaTime)
        {
            if (AccessUpdateObjects)
            {
                myInUpdate = true;

                foreach (WorldObject current in myObjects)
                {
                    if (current.AccessActive)
                    {
                        current.BaseUpdate(aDeltaTime);
                    }
                }

                HitDetector.UpdateAll();

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

            myBackground.Destroy();
        }

        public RoomBounds GetCurrentBounds(Vector2 aPosition, int? anObjectIndex = null)
            => (anObjectIndex == null) ? myMap.GetRoom(aPosition) : myMap.GetRoom(aPosition, anObjectIndex.Value);

        public int GetNewIndex()
            => ++lastIndex;

        private void SetCamera()
        {
            RendererController.AccessCamera.AccessPosition = new Vector2(myPlayer.AccessPosition.PixelPosition().X, -CAMERAELEVATION).Lerp(RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()), MOUSELERP);
        }
    }
}
