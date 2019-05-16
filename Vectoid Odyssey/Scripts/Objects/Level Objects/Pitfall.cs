using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class Pitfall : LevelObject
    {
        public Pitfall(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            AddCollider(aTopLeft, aBottomRight, false);
            AccessBoundingBox.OnEnter += Collide;
        }

        protected override void Update(float aDeltaTime)
        {
            
        }

        private void Collide(HitDetector aHitDetector)
        {
            if (aHitDetector.AccessOwner != null && aHitDetector.AccessOwner is Player player)
            {
                InGameManager.AccessMain.
            }
        }
    }
}
