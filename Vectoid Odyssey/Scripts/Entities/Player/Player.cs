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
            ANIMATIONSPEED = 16.0f;

        static public Player AccessMainPlayer { get; private set; }

        public Vector2 GetWeaponOrigin => AccessPosition + new Vector2(0.0625f, -0.5625f) * 2;
        public bool GetDead => AccessHealth <= 0;

        private PlayerWeapon GetActiveWeapon => myWeapons[myActiveWeapon];

        private MenuManager myMenuManager;
        private HitDetector myHitDetector;
        private Renderer.Sprite myBodyRenderer;
        private PlayerWeapon[] myWeapons;
        private float myMaxSpeed, myAcceleration, myBrakeAcceleration, myAnimationFrame = ANIMATIONSPEED * 0.5f, myJumpSpeed, myMaxJumpSpeed;
        private int myActiveWeapon, myScore;
        private bool myOnGround;

        private Renderer.Text myGUIHealth, myGUIScore;

        public Player(Vector2 aPosition, MenuManager aMenuManager, PlayerSetup aSetup)
        {
            AccessMainPlayer = this;

            AccessHealth = aSetup.health;
            AccessGravity = true;
            AccessDynamic = true;
            AccessPosition = aPosition;
            AccessKeepInBounds = true;

            myBodyRenderer = new Renderer.Sprite(Layer.Default, aSetup.sheet, aPosition, Vector2.One, Color.White, 0, new Vector2(16, 16));

            myHitDetector = new HitDetector(AccessPosition - new Vector2(2, 2), AccessPosition + new Vector2(2, 2), "Player", "BulletTarget");
            myHitDetector.AccessOwner = this;

            AccessBoundingBox = myHitDetector;
            OnBoundCorrection += UpdateCorrection;

            myMenuManager = aMenuManager;

            myWeapons = aSetup.weapons;
            myMaxSpeed = aSetup.maxSpeed;
            myAcceleration = aSetup.acceleration;
            myBrakeAcceleration = aSetup.brakeAcceleration;
            myJumpSpeed = aSetup.jumpSpeed;
            myMaxJumpSpeed = aSetup.maxJumpSpeed;

            CreateHUD();

            UpdateRenderer();
        }

        protected override void Update(float aDeltaTime)
        {
            if (!GetDead)
            {
                Move(aDeltaTime);
            }

            if (GetActiveWeapon != null)
            {
                UpdateWeapon(aDeltaTime);
            }

            UpdateRenderer();
        }

        protected override void UpdateHitDetector()
        {
            myHitDetector.Set(AccessPosition - new Vector2(2, 2), AccessPosition + new Vector2(2, 2));
        }

        protected override void Death()
        {
            AccessVelocity = new Vector2(0, AccessVelocity.Y);

            myGUIHealth.AccessString = new StringBuilder("DEAD");
            myGUIHealth.AccessFont = Font.Bold;

            myGUIScore.AccessString = new StringBuilder("FINAL SCORE: " + myScore);
            myGUIScore.AccessFont = Font.Bold;
        }

        public void AddScore(int aScore)
        {
            myScore += aScore;
        }

        private void UpdateCorrection(Vector2 aCorrection)
        {
            if (aCorrection.Y < 0)
            {
                myOnGround = true;
            }
        }

        private void UpdateWeapon(float aDeltaTime)
        {
            if (!GetDead)
            {
                GetActiveWeapon.Update(aDeltaTime);
                GetActiveWeapon.SetRotation((RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()) - GetWeaponOrigin.PixelPosition()).ToRadian());

                if (Input.Pressed(Control.Action1))
                {
                    GetActiveWeapon.Fire();
                }
            }
        }

        private void Move(float aDeltaTime)
        {
            float tempTargetXVelocity = myMaxSpeed * ((Input.Pressed(Control.Right) ? 1 : 0) + (Input.Pressed(Control.Left) ? -1 : 0));
            bool tempBrake = (AccessVelocity.X < 0 && AccessVelocity.X < tempTargetXVelocity) || (AccessVelocity.X > 0 && AccessVelocity.X > tempTargetXVelocity);

            float maxMovementDistance = aDeltaTime * (tempBrake ? myBrakeAcceleration : myAcceleration), 
                tempVelocityChange = (tempTargetXVelocity - AccessVelocity.X).Clamp(-maxMovementDistance, maxMovementDistance, out bool tempClamped);

            bool tempOnGround = false; // TODO: Implement ground check

            if (myOnGround)
            {
                AccessPosition = new Vector2(AccessPosition.X, AccessPosition.Y.Max(-2));
                AccessVelocity = new Vector2(AccessVelocity.X, 0);

                myOnGround = false;
                tempOnGround = true;
            }

            if (tempOnGround)
            {
                if (!tempClamped)
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
                    AccessVelocity = new Vector2(AccessVelocity.X, AccessVelocity.Y - (AccessVelocity.X / myMaxSpeed).Abs().Lerp(myJumpSpeed, myMaxJumpSpeed));
                }
            }
        }

        private void UpdateRenderer()
        {
            myBodyRenderer.AccessPosition = AccessPosition.PixelPosition();

            PlaceWeapon();

            if (!GetDead)
            {
                Animate();

                myGUIScore.AccessString = new StringBuilder("SC: " + myScore);
                myGUIHealth.AccessString = new StringBuilder("HP: " + AccessHealth);
            }
        }

        private void PlaceWeapon()
        {
            if (GetActiveWeapon != null)
            {
                GetActiveWeapon.AccessRenderer.AccessPosition = GetWeaponOrigin.PixelPosition();
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
            myGUIScore = new Renderer.Text(Layer.GUI, Font.Default, "SC: ", 4, 0, new Vector2(10, 50), Vector2.Zero, Color.White);

            myMenuManager.myHUD = new GUI.Collection(true);
            myMenuManager.myHUD.Add(myGUIHealth, myGUIScore);
        }
    }
}
