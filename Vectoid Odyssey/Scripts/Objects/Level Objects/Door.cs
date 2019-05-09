using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Door : LevelObject
    {
        const float
            OPENDISTANCE = 5.5f,
            CLOSEDISTANCE = 7.0f,
            OPENTIME = 0.6f;

        private Renderer.Sprite myRenderer;
        private Vector2 myVicinityOrigin;
        private bool myLocked, myOpen;
        private float myTimer;

        public Door(Vector2 aTopLeft, int? aKey = null)
        {
            myLocked = aKey != null;
            myVicinityOrigin = aTopLeft + new Vector2(1, 5);
            myRenderer = new Renderer.Sprite(Layer.Default, Load.Get<Texture2D>(myLocked ? "LockedDoor" : "UnlockedDoor"), aTopLeft, Vector2.One, Color.White, 0, Vector2.Zero);

            AddCollider(aTopLeft, aTopLeft + new Vector2(2, 8));

            OnPlayerTouch += PlayerTouch;
        }

        protected override void Update(float aDeltaTime)
        {
            if ((Player.AccessMainPlayer.AccessPosition - myVicinityOrigin).Length() < OPENDISTANCE && !myOpen)
            {
                Open();
            }

            if (myTimer > 0)
            {
                if (myOpen)
                {

                }
                else
                {

                }
            }
        }

        public void Open()
        {
            myOpen = true;
            myTimer = OPENTIME;
        }

        private void PlayerTouch(Player aPlayer)
        {

        }

        private void SetFrame(int aFrame) 
            => myRenderer.AccessSourceRectangle = new Rectangle(16 * aFrame, 0, 16, 64);
    }
}
