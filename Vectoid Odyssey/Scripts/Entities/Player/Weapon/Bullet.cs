using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    class Bullet : WorldObject
    {
        private HitDetector myHitDetector;

        public Bullet()
        {
            myHitDetector = new HitDetector
        }

        private void Hit(HitDetector aHitDetector)
        {

        }
    }
}
