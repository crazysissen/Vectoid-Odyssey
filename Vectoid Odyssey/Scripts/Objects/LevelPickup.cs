using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    abstract class LevelPickup : WorldObject
    {
        const float
            BOUNCETIME = 1,
            BOUNCEDISTANCE = 0.4f;

        private Renderer myRenderer;
        private HitDetector myHitDetector;

        ///// <summary>
        ///// Static
        ///// </summary>
        //public LevelPickup(Vector2 aPosition, Texture2D aSprite, Color? aColor = null)
        //{
        //    myRenderer = new Renderer.Sprite(Layer.Default, aSprite, aPosition, Vector2.One, aColor ?? Color.White, 0, aSprite.Bounds.Size.ToVector2() * 0.5f);
        //}

        /// <summary>
        /// Animator
        /// </summary>
        public LevelPickup(Vector2 aPosition, Texture2D aSpriteSheet, float anInterval, Point aFrameSize, Color? aColor = null)
        {
            myRenderer = new Renderer.Animator(Layer.Default, aSpriteSheet, aFrameSize, aPosition, Vector2.One, aFrameSize.ToVector2() * 0.5f, 0, aColor ?? Color.White, anInterval, 0, true, SpriteEffects.None);
        }

        protected abstract void Touch();
        protected abstract void Activate();

        private void Init()
        {

        }

        private void Collide(HitDetector aHitDetector)
        {

        }
    }
}
