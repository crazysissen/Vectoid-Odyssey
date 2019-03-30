using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Bullet : WorldObject
    {
        public enum TargetType
        {
            Player, Enemy
        }

        private static List<Bullet> allBullets = new List<Bullet>();

        public TargetType AccessTargetType { get; set; }
        public int AccessDamage { get; set; }
        public bool AccessPiercing { get; set; }

        private Renderer.Sprite myRenderer;
        private HitDetector myHitDetector;

        public Bullet(Vector2 aPosition, Vector2 aVelocity, Vector2 aSize, TargetType aTargetType, Color aColor, int aDamage, bool aPiercingBool, Texture2D aTexture = null, bool aRotateAlongPathBool = false)
        {
            AccessTargetType = aTargetType;
            AccessPosition = aPosition;
            AccessVelocity = aVelocity;
            AccessDamage = aDamage;
            AccessPiercing = aPiercingBool;
            AccessDynamic = true;
            AccessKeepInBounds = true;

            Texture2D tempTexture = aTexture ?? Load.Get<Texture2D>("Square");

            myRenderer = new Renderer.Sprite(new Layer(MainLayer.Main, -1), tempTexture, aPosition, aSize, aColor, 0, new Vector2(tempTexture.Bounds.X, tempTexture.Bounds.Y) * 0.5f);

            myHitDetector = new HitDetector(aPosition - aSize * 0.5f, aPosition + aSize * 0.5f, "Bullet", "Bullet" + AccessTargetType.ToString());
            myHitDetector.OnEnter += Hit;
            myHitDetector.AccessOwner = this;

            AccessBoundingBox = myHitDetector;
            OnBoundCorrection += Correction;

            allBullets.Add(this);
        }

        protected override void Update(float aDeltaTime)
        {
            myRenderer.AccessPosition = AccessPosition.PixelPosition();
        }

        protected override void BeforeDestroy()
        {
            myHitDetector.Destroy();
            myRenderer.Destroy();

            allBullets.Remove(this);
        }

        protected override void UpdateHitDetector()
        {
            Vector2 tempCornerOffset = myRenderer.AccessSize * myRenderer.AccessTexture.Bounds.Size.ToVector2() / Camera.WORLDUNITPIXELS;
            myHitDetector.Set(AccessPosition - tempCornerOffset, AccessPosition + tempCornerOffset);
        }

        private void Correction(Vector2 aCorrection)
        {
            Destroy();
        }

        private void Hit(HitDetector aHitDetector)
        {
            if (!aHitDetector.AccessTags.Contains("BulletTarget"))
            {
                return;
            }

            if (aHitDetector.AccessTags.Contains("World"))
            {
                Destroy();
                return;
            }

            if (aHitDetector.AccessTags.Contains(AccessTargetType == TargetType.Player ? "Player" : "Enemy"))
            {
                Entity tempEntity = (Entity)aHitDetector.AccessOwner;

                if (!tempEntity.AccessInvincible)
                {
                    tempEntity.ChangeHP(-AccessDamage);
                }

                if (!AccessPiercing)
                {
                    Destroy();
                }
            }
        }

        public static void DestroyAll()
        {
            for (int i = allBullets.Count - 1; i >= 0; allBullets[i].Destroy())
            {
                allBullets[i].Destroy();
            }
        }
    }
}
