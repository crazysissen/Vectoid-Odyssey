using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class EnemySkull : Enemy
    {
        Renderer.Animator myRenderer;

        public EnemySkull()
        {
            AccessHealth = 6;
        }

        protected override void Update(float aDeltaTime)
        {
            
        }

        protected override void Death()
        {
            Destroy();
        }
    }
}
