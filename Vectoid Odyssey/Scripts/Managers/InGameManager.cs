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
        static public InGameManager AccessMain { get; private set; }

        public bool AccessUpdateObjects { get; set; }

        private UpdateManager myUpdateManager;
        private MenuManager myMenuManager;

        private List<WorldObject> myObjects, myRemoveQueue, myAddQueue;

        private Player myPlayer;
        private Enemy mySock, mySkull;
        private bool myInUpdate;

        public InGameManager(UpdateManager anUpdateManager, MenuManager aMenuManager)
        {
            AccessMain = this;

            myUpdateManager = anUpdateManager;
            myMenuManager = aMenuManager;

            myObjects = new List<WorldObject>();
            myRemoveQueue = new List<WorldObject>();
        }

        public void InitGame()
        {
            PlayerSetup tempSetup = new PlayerSetup()
            {
                sheet = Load.Get<Texture2D>("Player"),
                maxSpeed = 1f,
                acceleration = 0.5f,
                brakeAcceleration = 0.5f,
                health = 10,
                weapons = new PlayerWeapon[6]
                {
                    PlayerWeapon.NewTeal, null, null, null, null, null
                }
            };

            myPlayer = new Player(new Vector2(0, -1), myMenuManager, tempSetup);

            mySkull = new EnemySkull();
            mySock = new EnemySock();
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

                foreach (WorldObject current in myRemoveQueue)
                {
                    myObjects.Remove(current);
                }

                myRemoveQueue.Clear();

                myInUpdate = false;
            }
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
    }
}
