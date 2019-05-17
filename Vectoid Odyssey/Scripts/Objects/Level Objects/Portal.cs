using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Portal : LevelObject
    {
        Vector2 myCentre;
        Renderer.Animator myRenderer;

        public Portal(Vector2 aCentrePosition)
        {
            myCentre = aCentrePosition /** 2*/;

            myRenderer = new Renderer.Animator(new Layer(MainLayer.Background, 100), Load.Get<Texture2D>("Portal"), new Point(32, 32), myCentre, Vector2.One, new Vector2(16, 16), 0, Color.White, 0.1f, 0, true, SpriteEffects.None);

            AddCollider(aCentrePosition - Vector2.One * 2, aCentrePosition + Vector2.One * 2, false);
            OnPlayerTouch += OnTouch;
        }

        private void OnTouch(Player aPlayer)
        {

        }
    }
}
