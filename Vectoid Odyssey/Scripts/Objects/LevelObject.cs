using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class LevelObject : WorldObject
    {
        public event Action<Player> OnPlayerTouch;

        public LevelObject()
        {

        }

        public void AddCollider(Vector2 aTopLeft, Vector2 aBottomRight, bool aWorldCollider)
        {
            AccessHitDetector = new HitDetector(aTopLeft, aBottomRight);
            AccessHitDetector.AccessOwner = this;
            AccessHitDetector.OnEnter += Collide;

            if (aWorldCollider)
            {
                AccessHitDetector.AccessTags.Add("World");
                AccessHitDetector.AccessTags.Add("BulletTarget");
            }
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
