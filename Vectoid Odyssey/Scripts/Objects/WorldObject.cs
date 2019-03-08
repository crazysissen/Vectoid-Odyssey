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
            GRAVITY = 1.0f;

        public InGameManager AccessManager { get; set; }

        public float AccessDamping { get; set; } = 0.0f;
        public bool AccessActive { get; set; } = true;
        public bool AccessDynamic { get; set; } = false;
        public bool AccessGravity { get; set; } = false;

        public Vector2 AccessPosition { get; set; } = Vector2.Zero;
        public Vector2 AccessVelocity { get; set; } = Vector2.Zero;

        public WorldObject()
        {
            AccessManager = InGameManager.AccessMain;

            AccessManager.Add(this);
        }

        // Called from the game manager
        public void BaseUpdate(float aDeltaTime)
        {
            if (AccessDynamic)
            {
                if (AccessGravity)
                {
                    AccessVelocity += new Vector2(0, GRAVITY);
                }

                AccessPosition += AccessVelocity;
            }

            Update(aDeltaTime);
        }

        // Called upon the derived class
        protected virtual void Update(float aDeltaTime)
        {

        }
    }
}
