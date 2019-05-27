using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace DCOdyssey
{
    abstract class LevelPickup : WorldObject
    {
        protected enum PickupAnimation { Instant, Bounce }

        const float
            FLOATTIME = 1.0f,
            FLOATDISTANCE = 0.3f,
            BOUNCEFORCE = 30.0f,
            FOLLOWFORCE = 60.0f,
            DIRECTFOLLOW = 20.0f,
            ROTATIONFORCE = 0.5f,
            SLOWDOWN = 0.7f,
            MINTIME = 0.1f;

        private static Random myR;

        protected bool myUseBounce;
        protected float myBounceMultiplier;
        protected PickupAnimation myAnimation;
        protected Renderer.Animator myRenderer;
        protected HitDetector myHitDetector;

        private float myTimer, myRotation, myAngularVelocity;
        private bool myAnimating;
        private string myEffect;
        private Vector2 myCurrentVelocity, myHalfWSize, myFloatOffset;
        private Player myPlayer;

        ///// <summary>
        ///// Static
        ///// </summary>
        //public LevelPickup(Vector2 aPosition, Texture2D aSprite, Color? aColor = null)
        //{
        //    myRenderer = new Renderer.Sprite(Layer.Default, aSprite, aPosition, Vector2.One, aColor ?? Color.White, 0, aSprite.Bounds.Size.ToVector2() * 0.5f);
        //}

        /// <summary>
        /// Animator
        /// </summary>
        public LevelPickup(Vector2 aFullCoordinate, Texture2D aSpriteSheet, float anInterval = 0, Point? aFrameSize = null, Color? aColor = null, string anEffect = null)
        {
            if (myR == null)
            {
                myR = new Random();
            }

            AccessPosition = aFullCoordinate * 2;
            myEffect = anEffect;

            Point tempFrameSize = aFrameSize ?? aSpriteSheet.Bounds.Size;

            myRenderer = new Renderer.Animator(Layer.Default, aSpriteSheet, tempFrameSize, aFullCoordinate * 2, Vector2.One, tempFrameSize.ToVector2() * 0.5f, 0, aColor ?? Color.White, anInterval, 0, true, SpriteEffects.None);

            myHalfWSize = (tempFrameSize.ToVector2() * 0.5f) * Camera.WORLDUNITMULTIPLIER * 0.6f;
            myHitDetector = new HitDetector(aFullCoordinate * 2 - myHalfWSize, aFullCoordinate * 2 + myHalfWSize, "Pickup");
            myHitDetector.AccessOwner = this;
            myHitDetector.OnEnter += Collide;
            AccessHitDetector = myHitDetector;
        }

        protected abstract void Touch(Player aPlayer);
        protected abstract void Activate(Player aPlayer);

        protected override void Update(float aDeltaTime)
        {
            if (myAnimating)
            {
                UpdateBounce(aDeltaTime);
            }
            else if (myUseBounce)
            {
                UpdateFloat(aDeltaTime);
            }

            myRenderer.AccessPosition = AccessPosition + myFloatOffset;
            myRenderer.AccessRotation = myRotation;
        }

        public override void UpdateHitDetector()
        {
            myHitDetector.Set(AccessPosition - myHalfWSize, AccessPosition + myHalfWSize);
        }

        protected override void BeforeDestroy()
        {
            myRenderer.Destroy();
            myHitDetector.Destroy();
        }

        private void UpdateFloat(float aDeltaTime)
        {
            myTimer += aDeltaTime;
            myTimer = (myTimer + aDeltaTime * myBounceMultiplier) % FLOATTIME;

            myFloatOffset = new Vector2(0, FLOATDISTANCE * MathV.Sine01(myTimer));
        }

        private void UpdateBounce(float aDeltaTime)
        {
            myTimer += aDeltaTime;

            Vector2 tempPlayerVector = (myPlayer.AccessPosition - AccessPosition).Normalized();
            myCurrentVelocity += tempPlayerVector * aDeltaTime * FOLLOWFORCE;

            myAngularVelocity *= (1 - aDeltaTime * SLOWDOWN);

            AccessPosition += (myCurrentVelocity * aDeltaTime + myFloatOffset) +
                tempPlayerVector * myTimer * aDeltaTime * DIRECTFOLLOW;
            myRotation += myAngularVelocity;
        }

        private void Collide(HitDetector aHitDetector)
        {
            if (aHitDetector == null || aHitDetector.AccessOwner == null)
            {
                return;
            }

            if (myAnimating)
            {
                if (myTimer > MINTIME)
                {
                    Activate(myPlayer);
                    Remove();
                }

                return;
            }

            if (aHitDetector.AccessOwner is Player tempPlayer)
            {
                Touch(tempPlayer);

                switch (myAnimation)
                {
                    case PickupAnimation.Instant:
                        Activate(tempPlayer);
                        Remove();
                        break;

                    case PickupAnimation.Bounce:
                        Touch(tempPlayer);
                        Bounce(tempPlayer);
                        break;

                    default:
                        goto case PickupAnimation.Instant;
                }
            }
        }

        private void Remove()
        {
            Destroy();
        }

        private void Bounce(Player aPlayer)
        {
            myFloatOffset = new Vector2();
            myPlayer = aPlayer;
            myAnimating = true;
            myTimer = 0;

            myCurrentVelocity = (AccessPosition - aPlayer.AccessPosition + new Vector2(0, -0.4f - (float)myR.NextDouble() * 0.8f)).Normalized() * BOUNCEFORCE * (0.85f + 0.3f * (float)myR.NextDouble());

            myAngularVelocity = (ROTATIONFORCE - (float)myR.NextDouble() * ROTATIONFORCE * 0.4f) * (myR.Next(2) == 1 ? 1 : -1);
        }
    }
}
