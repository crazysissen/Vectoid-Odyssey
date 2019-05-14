﻿using System;
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
        private int? myKey;

        public Door(Vector2 aTopLeft, int? aKey = null)
        {
            myKey = aKey;
            myLocked = aKey != null;
            myVicinityOrigin = aTopLeft + new Vector2(1, 5);
            myRenderer = new Renderer.Sprite(Layer.Default, Load.Get<Texture2D>(myLocked ? "LockedDoor" : "UnlockedDoor"), aTopLeft, Vector2.One, Color.White, 0, Vector2.Zero);

            AddCollider(aTopLeft, aTopLeft + new Vector2(2, 8), true);

            OnPlayerTouch += PlayerTouch;
        }

        protected override void Update(float aDeltaTime)
        {
            float tempPlayerDistance = (Player.AccessMainPlayer.AccessPosition - myVicinityOrigin).Length();

            if (tempPlayerDistance < OPENDISTANCE && !myOpen)
            {
                Trigger(Player.AccessMainPlayer);
            }
            else if (myOpen && tempPlayerDistance > CLOSEDISTANCE)
            {
                Close();
            }

            if (myTimer > 0)
            {
                myTimer -= aDeltaTime;

                if (myOpen) // Door opening
                {
                    if (myTimer < 0)
                    {
                        SetFrame(3);
                    }
                    else
                    {
                        SetFrame(myTimer < OPENTIME * 0.5f ? 1 : 2);
                    }
                }
                else // Door closing
                {
                    if (myTimer < 0)
                    {
                        SetFrame(0);
                    }
                    else
                    {
                        SetFrame(myTimer < OPENTIME * 0.5f ? 1 : 2);
                    }
                }
            }
        }

        public void Trigger(Player aPlayer)
        {
            // TODO: Fix trigger
            
            if (myLocked && !aPlayer.HasItem(ItemType.Key, myKey.Value, true))
            {
                return;
            }

            myLocked = false;
            Open();
        }

        public void Open()
        {
            myOpen = true;

            if (myTimer > 0)
            {
                myTimer = OPENTIME - myTimer;

                return;
            }

            myTimer = OPENTIME;
        }

        public void Close()
        {
            myOpen = false;

            if (myTimer > 0)
            {
                myTimer = OPENTIME - myTimer;

                return;
            }

            myTimer = OPENTIME;
        }

        private void PlayerTouch(Player aPlayer)
        {

        }

        private void SetFrame(int aFrame) 
            => myRenderer.AccessSourceRectangle = new Rectangle(16 * aFrame, 0, 16, 64);
    }
}
