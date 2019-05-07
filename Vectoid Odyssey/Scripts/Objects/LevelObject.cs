using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class LevelObject : WorldObject
    {
        public event Action<Player> OnPlayerTouch;

        public LevelObject()
        {

        }

        public void AddCollider(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            AccessBoundingBox = new HitDetector(aTopLeft, aBottomRight);
            AccessBoundingBox.OnEnter += Collide;
        }

        private void Collide(HitDetector aHitDetector)
        {
            if (aHitDetector.AccessTags.Contains("Player"))
            {
                OnPlayerTouch?.Invoke((Player)aHitDetector.AccessOwner);
            }
        }
    }
}
