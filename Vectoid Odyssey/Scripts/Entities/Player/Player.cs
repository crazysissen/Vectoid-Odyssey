using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Player : Entity
    {
        const float
            ANIMATIONSPEED = 60.0f,
            JUMPVELOCITY = 0.75f;

        static public Player AccessMainPlayer { get; private set; }

        public Vector2 GetWeaponOrigin => AccessPosition + new Vector2(0.0625f, -0.5625f) * 2;

        private PlayerWeapon GetActiveWeapon => myWeapons[myActiveWeapon];

        private MenuManager myMenuManager;
        private Renderer.Sprite myBodyRenderer;
        private PlayerWeapon[] myWeapons;
        private float myMaxSpeed, myAcceleration, myBrakeAcceleration, myAnimationFrame = ANIMATIONSPEED * 0.5f;
        private int myActiveWeapon, myScore;

        private Renderer.Text myGUIHealth, myGUIScore;

        public Player(Vector2 aPosition, MenuManager aMenuManager, PlayerSetup aSetup)
        {
            AccessMainPlayer = this;

            AccessHealth = aSetup.health;
            AccessGravity = true;
            AccessDynamic = true;
            AccessPosition = aPosition;

            myBodyRenderer = new Renderer.Sprite(Layer.Default, aSetup.sheet, aPosition, Vector2.One, Color.White, 0, new Vector2(16, 16));

            myMenuManager = aMenuManager;

            myWeapons = aSetup.weapons;
            myMaxSpeed = aSetup.maxSpeed;
            myAcceleration = aSetup.acceleration;
            myBrakeAcceleration = aSetup.brakeAcceleration;

            CreateHUD();

            UpdateRenderer();
        }

        protected override void Update(float aDeltaTime)
        {
            Move(aDeltaTime);

            if (GetActiveWeapon != null)
            {
                UpdateWeapon(aDeltaTime);
            }

            UpdateRenderer();
        }

        private void UpdateWeapon(float aDeltaTime)
        {
            GetActiveWeapon.Update(aDeltaTime);
            GetActiveWeapon.SetRotation((RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()) - AccessPosition).ToRadian());

            if (Input.Down(Control.Action1))
            {
                GetActiveWeapon.Fire();
            }
        }

        private void Move(float aDeltaTime)
        {
            float tempTargetXVelocity = myMaxSpeed * ((Input.Pressed(Control.Right) ? 1 : 0) + (Input.Pressed(Control.Left) ? -1 : 0));
            bool tempBrake = (AccessVelocity.X < 0 && AccessVelocity.X < tempTargetXVelocity) || (AccessVelocity.X > 0 && AccessVelocity.X > tempTargetXVelocity);
            float tempVelocityChange = (tempTargetXVelocity - AccessVelocity.X < 0 ? -1 : 1) * aDeltaTime * (tempBrake ? myBrakeAcceleration : myAcceleration);

            bool tempOnGround = true; // TODO: Implement ground check

            if (AccessPosition.Y > -1)
            {
                AccessPosition = new Vector2(AccessPosition.X, AccessPosition.Y.Max(-1));
                AccessVelocity = new Vector2(AccessVelocity.X, 0);
            }

            if (tempOnGround)
            {
                if (tempTargetXVelocity.Abs() - AccessVelocity.X.Abs() < tempVelocityChange.Abs())
                {
                    AccessVelocity = new Vector2(tempTargetXVelocity, AccessVelocity.Y);
                }
                else
                {
                    AccessVelocity = new Vector2(AccessVelocity.X + tempVelocityChange, AccessVelocity.Y);
                }

                myAnimationFrame += AccessVelocity.X * aDeltaTime * ANIMATIONSPEED;

                if (Input.Down(Control.Action2))
                {
                    AccessVelocity = new Vector2(AccessVelocity.X, AccessVelocity.Y - JUMPVELOCITY);
                }
            }
        }

        private void UpdateRenderer()
        {
            Animate();

            myGUIScore.AccessString = new StringBuilder("SC: " + myScore);
            myGUIHealth.AccessString = new StringBuilder("HP: " + AccessHealth);

            myBodyRenderer.AccessPosition = AccessPosition;

            PlaceWeapon();
        }

        private void PlaceWeapon()
        {
            if (GetActiveWeapon != null)
            {
                GetActiveWeapon.AccessRenderer.AccessPosition = GetWeaponOrigin;
            }
        }

        private void Animate()
        {
            myAnimationFrame = myAnimationFrame.Wrap(0, 4);

            myBodyRenderer.AccessSourceRectangle = new Rectangle(32 * (int)myAnimationFrame, 0, 32, 32);
        }

        private void CreateHUD()
        {
            myGUIHealth = new Renderer.Text(Layer.GUI, Font.Default, "HP: ", 4, 0, new Vector2(10, 10), Vector2.Zero, Color.White);
            myGUIScore= new Renderer.Text(Layer.GUI, Font.Default, "SC: ", 4, 0, new Vector2(10, 50), Vector2.Zero, Color.White);

            myMenuManager.myHUD = new GUI.Collection(true);
            myMenuManager.myHUD.Add(myGUIHealth, myGUIScore);
        }
    }
}
