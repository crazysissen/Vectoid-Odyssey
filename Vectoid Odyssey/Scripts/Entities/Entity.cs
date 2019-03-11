using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    abstract class Entity : WorldObject
    {
        public int AccessHealth { get; protected set; }
        public bool AccessInvincible { get; protected set; }

        public virtual void ChangeHP(int aChange)
        {
            AccessHealth += aChange;

            if (AccessHealth <= 0)
            {
                Death();
            }
        }

        protected virtual void Death()
        {

        }
    }
}
