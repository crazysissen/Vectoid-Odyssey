using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    abstract class WorldObject
    {
        public const float 
            GRAVITY = 8.0f,
            TERMINALVELOCITY = 7.0f;

        public event Action<Vector2> OnBoundCorrection;

        public InGameManager AccessManager { get; set; }

        public readonly int index;

        public float AccessDamping { get; set; } = 0.0f;
        public float AccessGravityModifier { get; set; } = 1.0f;
        public float AccessTerminalGravity { get; set; } = TERMINALVELOCITY;
        public bool AccessActive { get; set; } = true;
        public bool AccessDynamic { get; set; } = false;
        public bool AccessGravity { get; set; } = false;
        public bool GetDestroyed { get; private set; }

        public Vector2 AccessPosition { get; set; } = Vector2.Zero;
        public Vector2 AccessVelocity { get; set; } = Vector2.Zero;

        public bool AccessKeepInBounds { get; protected set; }
        public bool AccessWorldCollide { get; protected set; }
        public HitDetector AccessBoundingBox { get; protected set; }

        protected bool GetOnGround { get; private set; }

        public WorldObject()
        {
            AccessManager = InGameManager.AccessMain;
            AccessManager.Add(this);

            index = AccessManager.GetNewIndex();
        }

        public void UpdateDynamic(float aDeltaTime)
        {
            if (AccessDynamic)
            {
                if (AccessGravity && (AccessTerminalGravity <= 0 || AccessVelocity.Y < AccessTerminalGravity))
                {
                    AccessVelocity += new Vector2(0, GRAVITY * AccessGravityModifier) * aDeltaTime;
                }

                AccessPosition += AccessVelocity * aDeltaTime * Camera.WORLDUNITPIXELS;
            }

            UpdateHitDetector();
        }

        public void SimpleCollision(float aDeltaTime, bool anUpwardsBool)
        {
            if (AccessKeepInBounds && AccessBoundingBox != null)
            {
                Vector2 tempCorrection = AccessManager.GetCurrentBounds(AccessPosition, index).Correction(AccessBoundingBox.AccessTopLeft, AccessBoundingBox.AccessBottomRight, anUpwardsBool);

                if (tempCorrection != Vector2.Zero)
                {
                    Correct(tempCorrection);
                }
            }
        }

        // Called from the game manager
        public void BaseUpdate(float aDeltaTime)
        {
            Update(aDeltaTime);
        }

        public void Correct(Vector2 aCorrection)
        {
            if (aCorrection.Y < 0)
            {
                GetOnGround = true;
            }
            else
            {
                GetOnGround = false;
            }

            if ((aCorrection.X < 0 && AccessVelocity.X > 0) || (aCorrection.X > 0 && AccessVelocity.X < 0))
                  AccessVelocity = new Vector2(0, AccessVelocity.Y);

            if ((aCorrection.Y < 0 && AccessVelocity.Y > 0) || (aCorrection.Y > 0 && AccessVelocity.Y < 0))
                AccessVelocity = new Vector2(AccessVelocity.X, 0);

            AccessPosition += aCorrection;

            OnBoundCorrection?.Invoke(aCorrection);
        }

        // Called upon the derived class
        protected virtual void Update(float aDeltaTime)
        {

        }

        public void Destroy()
        {
            BeforeDestroy();

            GetDestroyed = true;
            AccessManager.Remove(this);
        }

        public virtual void UpdateHitDetector()
        {

        }

        protected virtual void BeforeDestroy()
        {

        }
    }
}
