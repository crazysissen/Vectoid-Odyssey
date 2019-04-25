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
        private float myMaxSpeed, myAcceleration, myBrakeAcceleration, myAnimationFrame = ANIMATIONSPEED * 0.5f, myMaxJumpTime, myStartJumpSpeed, myEndJumpSpeed, myCurrentJumpTime, myJumpBlockTimer, myJumpBlockTime, myNonLinear;
        private int myActiveWeapon, myScore;
        private bool myOnGround, myJumping, myGoingToJump;

        private Renderer.Text myGUIHealth, myGUIScore;

        public Player(Vector2 aPosition, MenuManager aMenuManager, PlayerSetup aSetup)
        {
            AccessMainPlayer = this;

            AccessHealth = aSetup.health;
            AccessGravity = true;
            AccessGravityModifier = 1.7f;
            AccessDynamic = true;
            AccessPosition = aPosition;
            AccessKeepInBounds = true;
            AccessWorldCollide = true;

            myBodyRenderer = new Renderer.Sprite(Layer.Default, aSetup.sheet, aPosition, Vector2.One, Color.White, 0, new Vector2(16, 16));

            myHitDetector = new HitDetector(AccessPosition - new Vector2(2, 2), AccessPosition + new Vector2(2, 2), "Player", "BulletTarget");
            myHitDetector.AccessOwner = this;

            AccessBoundingBox = myHitDetector;
            OnBoundCorrection += UpdateCorrection;

            myMenuManager = aMenuManager;

            myActiveWeapon = 1;

            myWeapons = aSetup.weapons;
            myMaxSpeed = aSetup.maxSpeed;
            myAcceleration = aSetup.acceleration;
            myBrakeAcceleration = aSetup.brakeAcceleration;
            myStartJumpSpeed = aSetup.jumpStartAcceleration;
            myEndJumpSpeed = aSetup.jumpEndAcceleration;
            myMaxJumpTime = aSetup.maxJumpTime;
            myNonLinear = aSetup.nonLinear;

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

        public override void UpdateHitDetector()
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

            if (aCorrection.Y > 0 && myJumping)
            {
                myJumping = false;
            }
        }

        private void UpdateWeapon(float aDeltaTime)
        {
            if (!GetDead)
            {
                int tempScrollWheel = Input.GetScrollWheelChange;

                if (tempScrollWheel != 0)
                {
                    myActiveWeapon += (tempScrollWheel > 0) ? 1 : -1;
                    myActiveWeapon = (myActiveWeapon + myWeapons.Length) % myWeapons.Length;
                }

                for (int i = 0; i < myWeapons.Length; i++)
                {
                    myWeapons[i].AccessRenderer.AccessActive = i == myActiveWeapon;
                }

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
            float tempTargetXVelocity;

            if (GetDead)
            {
                tempTargetXVelocity = 0;
            }
            else
            {
                tempTargetXVelocity = myMaxSpeed * ((Input.Pressed(Control.Right) ? 1 : 0) + (Input.Pressed(Control.Left) ? -1 : 0));
            }

            bool tempBrake = (AccessVelocity.X < 0 && AccessVelocity.X < tempTargetXVelocity) || (AccessVelocity.X > 0 && AccessVelocity.X > tempTargetXVelocity);

            float 
                tempControlMultiplier = 1,
                tempNonLinearAcceleration = tempBrake ? 1 + myNonLinear : 1 + myNonLinear * (1 - AccessVelocity.X.Abs() / myMaxSpeed);

            if (myJumpBlockTimer > 0)
            {
                myJumpBlockTimer -= aDeltaTime;
                tempControlMultiplier = 1 - myJumpBlockTimer / myJumpBlockTime;
            }

            float
                tempMaxMovementDistance = aDeltaTime * (tempBrake ? myBrakeAcceleration : myAcceleration) * tempControlMultiplier * tempNonLinearAcceleration,
                tempVelocityChange = (tempTargetXVelocity - AccessVelocity.X).Clamp(-tempMaxMovementDistance, tempMaxMovementDistance, out bool tempClamped);
                

            if (myJumping)
            {
                myGoingToJump = false;

                if (Input.Pressed(Control.Action2) && myCurrentJumpTime < myMaxJumpTime)
                {
                    myCurrentJumpTime += aDeltaTime;
                    AccessVelocity = new Vector2(AccessVelocity.X, (myCurrentJumpTime / myMaxJumpTime).Lerp(-myStartJumpSpeed, -myEndJumpSpeed));
                }
                else
                {
                    myJumping = false;
                }
            }

            if (myOnGround)
            {
                //AccessVelocity = new Vector2(AccessVelocity.X, 0);

                if (myGoingToJump || Input.Down(Control.Action2))
                {
                    Jump();
                }

                myGoingToJump = false;
            }
            else
            {
                if (!myJumping && Input.Down(Control.Action2))
                {
                    myGoingToJump = true;
                }

                if (myGoingToJump && !Input.Pressed(Control.Action2))
                {
                    myGoingToJump = false;
                }
            }
                //AccessPosition = new Vector2(AccessPosition.X, AccessPosition.Y.Max(-2));

            if (!tempClamped)
            {
                AccessVelocity = new Vector2(tempTargetXVelocity, AccessVelocity.Y);
            }
            else
            {
                AccessVelocity = new Vector2(AccessVelocity.X + tempVelocityChange, AccessVelocity.Y);
            }

            myAnimationFrame += AccessVelocity.X * aDeltaTime * ANIMATIONSPEED;

            myOnGround = false;
        }

        private void Jump()
        {
            myJumping = true;
            myCurrentJumpTime = 0.0f;

            AccessVelocity = new Vector2(AccessVelocity.X, -myStartJumpSpeed);
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

        public void BlockControls(float aTime)
        {
            myJumpBlockTimer = aTime;
            myJumpBlockTime = aTime;
        }
    }
}
