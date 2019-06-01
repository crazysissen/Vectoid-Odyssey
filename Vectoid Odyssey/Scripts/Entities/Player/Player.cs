using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class Player : Entity
    {
        const float
            ANIMATIONSPEED = 16.0f;

        static public Player AccessMainPlayer { get; private set; }

        public Vector2 GetWeaponOrigin => AccessPosition + new Vector2(0.0625f, -0.5625f) * 2;
        public bool GetDead => AccessHealth <= 0;

        public PlayerWeapon GetActiveWeapon => myWeapons[myActiveWeapon];

        private MenuManager myMenuManager;
        private HitDetector myHitDetector, myTopDetector;
        //private Texture2D myTexture, myCrouchTexture;
        private Renderer.Sprite myBodyRenderer;
        private PlayerWeapon[] myWeapons;
        private List<Item> myItems;
        private float myMaxSpeed, myAcceleration, myBrakeAcceleration, myAnimationFrame = ANIMATIONSPEED * 0.5f, myMaxJumpTime, myStartJumpSpeed, myEndJumpSpeed, myCurrentJumpTime, myJumpBlockTimer, myJumpBlockTime, myNonLinear;
        private int[] myAmmo;
        private int myActiveWeapon, myScore, myMaxHP, myShield, myMaxShield;
        private bool myOnGround, myJumping, myGoingToJump, myBlockedControls, myPaused/*, myCrouching*/;

        public Player(Vector2 aPosition, MenuManager aMenuManager, PlayerSetup aSetup)
        {
            AccessMainPlayer = this;

            AccessHealth = aSetup.health;
            AccessGravity = true;
            AccessGravityModifier = 2.3f;
            AccessDynamic = true;
            AccessPosition = aPosition;
            AccessWorldCollide = true;

            myAmmo = new int[6];

            myBodyRenderer = new Renderer.Sprite(Layer.Default, aSetup.sheet, aPosition, Vector2.One, Color.White, 0, new Vector2(16, 16));

            myHitDetector = new HitDetector(AccessPosition - new Vector2(1.875f, 1.5f), AccessPosition + new Vector2(1.875f, 2), "Player", "BulletTarget");
            myHitDetector.AccessOwner = this;

            AccessHitDetector = myHitDetector;
            OnBoundCorrection += UpdateCorrection;

            myMenuManager = aMenuManager;
            myItems = new List<Item>();
            myActiveWeapon = 1;

            //myTexture = aSetup.sheet;
            //myCrouchTexture = aSetup.crouchSheet;
            myMaxHP = aSetup.health;
            myWeapons = aSetup.weapons;
            myMaxSpeed = aSetup.maxSpeed;
            myAcceleration = aSetup.acceleration;
            myBrakeAcceleration = aSetup.brakeAcceleration;
            myStartJumpSpeed = aSetup.jumpStartAcceleration;
            myEndJumpSpeed = aSetup.jumpEndAcceleration;
            myMaxJumpTime = aSetup.maxJumpTime;
            myNonLinear = aSetup.nonLinear;

            UpdateRenderer();
        }

        protected override void Update(float aDeltaTime)
        {
            if (Input.Down(Control.Menu1))
            {
                TogglePause();
            }

            myMenuManager.UpdateItems(myItems.ToArray(), GetActiveWeapon.AccessRenderer.AccessTexture);

            Move(aDeltaTime);

            Interaction();

            if (GetActiveWeapon != null)
            {
                UpdateWeapon(aDeltaTime);
            }

            UpdateRenderer();
        }

        public override void UpdateHitDetector()
        {
            myHitDetector.Set(AccessPosition - new Vector2(1.875f, 1.5f), AccessPosition + new Vector2(1.875f, 2));
        }

        protected override void Death()
        {
            AccessVelocity = new Vector2(0, AccessVelocity.Y);

            myMenuManager.SetStats(0, myMaxHP, GetActiveWeapon.AccessAmmo == -1 ? "-" : GetActiveWeapon.AccessAmmo.ToString());
        }

        public void AddAmmo(int aWeapon, int anAmmount)
        {
            myWeapons[aWeapon].AddAmmo(anAmmount);
        }

        public void TogglePause()
        {
            if (myPaused)
            {
                myMenuManager.ClosePauseMenu();
                myPaused = false;
            }
            else
            {
                myMenuManager.OpenPauseMenu();
                myPaused = true;
            }
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

            if (myBlockedControls)
            {
                return;
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
                //if (Input.Down(Keys.Down))
                //{
                //    Crouch(true);
                //}
                //else if (myCrouching)
                //{
                //    Crouch(false);
                //}

                if (/*!myCrouching &&*/ (myGoingToJump || Input.Down(Control.Action2)))
                {
                    Jump();
                }

                myGoingToJump = false;
            }
            else
            {
                //if (myCrouching)
                //{
                //    Crouch(false);
                //}

                if (!myJumping && Input.Down(Control.Action2))
                {
                    myGoingToJump = true;
                }

                if (myGoingToJump && !Input.Pressed(Control.Action2))
                {
                    myGoingToJump = false;
                }
            }

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

        //private void TryCrouch()
        //{

        //}
        
        //private void Crouch(bool anActiveBool)
        //{
        //    myBodyRenderer.AccessTexture = anActiveBool ? myCrouchTexture : myTexture;
        //    myCrouching = true;
        //}

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

                myMenuManager.SetStats(AccessHealth, myMaxHP, GetActiveWeapon.AccessAmmo == -1 ? "-" : GetActiveWeapon.AccessAmmo.ToString());
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

        public void PitfallDeath()
        {
            InGameManager.AccessMain.StopCamera();
            ChangeHP(-AccessHealth);
        }

        public void BlockJump(float aTime)
        {
            myJumpBlockTimer = aTime;
            myJumpBlockTime = aTime;
        }

        public void BlockMovement(bool aBlockControls, bool aBlockGravity)
        {
            myBlockedControls = aBlockControls;
            AccessGravity = !aBlockGravity;
        }

        public void PickupItem(Item anItem)
        {
            myItems.Add(anItem);
        }

        public void PipeViable(Pipe aPipe, Vector2 pipeDirection)
        {
            Console.WriteLine("Pipe viable: " + pipeDirection);

            if (pipeDirection.X == 1 && Input.Pressed(Control.Right) ||
                pipeDirection.X == -1 && Input.Pressed(Control.Left) ||
                pipeDirection.Y == 1 && Input.Pressed(Control.Down) ||
                pipeDirection.Y == -1 && Input.Pressed(Control.Up))

            {
                ActivatePipe(aPipe, pipeDirection);
                return;
            }
        }

        private void ActivatePipe(Pipe aPipe, Vector2 aDirection)
        {
            aPipe.StartTransfer(this, aDirection);
            BlockMovement(true, true);
        }

        public void DeactivatePipe()
        {
            BlockMovement(false, false);
        }

        public bool HasItem(ItemType aType, int anIndex, bool aRemoveBool = false)
        {
            Item foundItem = null;
            for (int i = 0; i < myItems.Count && foundItem == null; ++i)
            {
                if (myItems[i].GetItemType == aType && myItems[i].GetIndex == anIndex)
                {
                    foundItem = myItems[i];
                }
            }

            if (aRemoveBool && foundItem != null)
            {
                myItems.Remove(foundItem);
            }

            return foundItem != null;
        }

        private void Interaction()
        {
            PlayerInteraction tempInteraction = PlayerInteraction.Closest(AccessPosition);

            myMenuManager.SetPopup(tempInteraction?.GetPrompt ?? "");

            if (tempInteraction == null)
            {
                return;
            }


        }
    }
}
