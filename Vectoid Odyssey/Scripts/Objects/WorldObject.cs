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
            GRAVITY = 3.0f;

        public event Action<Vector2> OnBoundCorrection;

        public InGameManager AccessManager { get; set; }

        public readonly int index;

        public float AccessDamping { get; set; } = 0.0f;
        public bool AccessActive { get; set; } = true;
        public bool AccessDynamic { get; set; } = false;
        public bool AccessGravity { get; set; } = false;

        public Vector2 AccessPosition { get; set; } = Vector2.Zero;
        public Vector2 AccessVelocity { get; set; } = Vector2.Zero;

        public bool AccessKeepInBounds { get; protected set; }
        public HitDetector AccessBoundingBox { get; protected set; }

        protected bool GetOnGround { get; private set; }

        public WorldObject()
        {
            AccessManager = InGameManager.AccessMain;
            AccessManager.Add(this);

            index = AccessManager.GetNewIndex();
        }

        // Called from the game manager
        public void BaseUpdate(float aDeltaTime)
        {
            if (AccessDynamic)
            {
                if (AccessGravity)
                {
                    AccessVelocity += new Vector2(0, GRAVITY) * aDeltaTime;
                }

                AccessPosition += AccessVelocity * aDeltaTime * Camera.WORLDUNITPIXELS;
            }

            UpdateHitDetector();

            if (AccessKeepInBounds && AccessBoundingBox != null)
            {
                Vector2 tempCorrection = AccessManager.GetCurrentBounds.Correction(AccessBoundingBox.AccessTopLeft, AccessBoundingBox.AccessBottomRight);

                if (tempCorrection != Vector2.Zero)
                {
                    if (tempCorrection.Y < 0)
                    {
                        GetOnGround = true;
                    }
                    else
                    {
                        GetOnGround = false;
                    }

                    if ((tempCorrection.X < 0 && AccessVelocity.X > 0) || (tempCorrection.X > 0 && AccessVelocity.X < 0))
                        AccessVelocity = new Vector2(0, AccessVelocity.Y);

                    if ((tempCorrection.Y < 0 && AccessVelocity.Y > 0) || (tempCorrection.Y > 0 && AccessVelocity.Y < 0))
                        AccessVelocity = new Vector2(AccessVelocity.X, 0);

                    AccessPosition += tempCorrection;

                    OnBoundCorrection?.Invoke(tempCorrection);
                }
            }

            Update(aDeltaTime);
        }

        // Called upon the derived class
        protected virtual void Update(float aDeltaTime)
        {

        }

        public void Destroy()
        {
            BeforeDestroy();

            AccessManager.Remove(this);
        }

        protected virtual void UpdateHitDetector()
        {

        }

        protected virtual void BeforeDestroy()
        {

        }
    }
}
