using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class Pitfall : LevelObject
    {
        public Pitfall(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            AddCollider(aTopLeft, aBottomRight, false);
            OnPlayerTouch += Collide;
        }

        protected override void Update(float aDeltaTime)
        {
            
        }

        private void Collide(Player aPlayer)
        {
            aPlayer.PitfallDeath();
        }
    }
}
