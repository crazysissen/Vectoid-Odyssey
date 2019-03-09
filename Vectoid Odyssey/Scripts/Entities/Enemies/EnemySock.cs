using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class EnemySock : Enemy
    {
        const float
            SPEED = 0.6F;
         

        Renderer.Animator myRenderer;

        public EnemySock()
        {
            AccessHealth = 4;

            myRenderer = new Renderer.Animator(Layer.Default, Load.Get<Texture2D>("Enemy1"), new Point(8, 8), AccessPosition, Vector2.One, new Vector2(4, 4), 0, Color.White, 0.5f, 0, true, SpriteEffects.None);
        }

        protected override void Update(float aDeltaTime)
        {
            myRenderer.AccessPosition = AccessPosition;
            myRenderer.AccessEffects = Player.AccessMainPlayer.AccessPosition.X < AccessPosition.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        protected override void Death()
        {
            Destroy();
        }
    }
}
