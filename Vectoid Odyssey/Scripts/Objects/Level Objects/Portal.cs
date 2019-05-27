using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class Portal : LevelObject
    {
        public bool PortalActive { get; set; } = true;

        Vector2 myCentre;
        Renderer.Animator myRenderer;
        PlayerInteraction myInteraction;

        public Portal(Vector2 aCentrePosition)
        {
            myCentre = aCentrePosition /** 2*/;

            myRenderer = new Renderer.Animator(new Layer(MainLayer.Background, 100), Load.Get<Texture2D>("Portal"), new Point(32, 32), myCentre, Vector2.One, new Vector2(16, 16), 0, Color.White, 0.1f, 0, true, SpriteEffects.None);

            myInteraction = new PlayerInteraction("Press UP to exit.", PortalActive, aCentrePosition);

            AddCollider(aCentrePosition - Vector2.One * 2, aCentrePosition + Vector2.One * 2, false);
            OnPlayerTouch += OnTouch;
        }

        protected override void Update(float aDeltaTime)
        {
            myInteraction.AccessActive = PortalActive;
            myRenderer.AccessActive = PortalActive;
        }

        private void OnTouch(Player aPlayer)
        {

        }
    }
}
