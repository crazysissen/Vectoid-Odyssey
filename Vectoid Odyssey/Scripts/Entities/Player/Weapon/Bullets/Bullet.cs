using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class Bullet : WorldObject
    {
        const float MAXDISTANCE = 38.0f;

        public enum TargetType
        {
            Player, Enemy
        }

        private static List<Bullet> allBullets = new List<Bullet>();

        public TargetType AccessTargetType { get; set; }
        public int AccessDamage { get; set; }
        public bool AccessPiercing { get; set; }

        private bool myFlameCollider;
        private Vector2 myWorldSize;
        private List<Entity> myHitEntities;
        private float myLifetime, myMaxLifetime;
        private Renderer.Animator myRenderer;
        private HitDetector myHitDetector;

        public Bullet(Vector2 aPosition, Vector2 aVelocity, Vector2 aSize, TargetType aTargetType, Color aColor, int aDamage, float aLifetime, bool aPiercingBool, bool aGravityBool = false, Texture2D aTexture = null, float anAngle = 0, Point? aFrameSize = null, float aFrameDelay = 0.1f, bool aRepeat = false, bool aRandomizeEffects = false, bool aFlameCollider = false)
        {
            AccessTargetType = aTargetType;
            AccessPosition = aPosition;
            AccessVelocity = aVelocity;
            AccessDamage = aDamage;
            AccessPiercing = aPiercingBool;
            AccessGravity = aGravityBool;
            AccessGravityModifier = 0.7f;
            AccessDynamic = true;
            AccessWorldCollide = true;

            myLifetime = aLifetime;
            myMaxLifetime = aLifetime;
            myHitEntities = new List<Entity>();
            myFlameCollider = aFlameCollider;

            Texture2D tempTexture = aTexture ?? Load.Get<Texture2D>("Square");

            myWorldSize = aFrameSize != null ? aSize * aFrameSize.Value.ToVector2() / Camera.WORLDUNITPIXELS : aSize * new Vector2(tempTexture.Width, tempTexture.Height) / Camera.WORLDUNITPIXELS;

            myRenderer = new Renderer.Animator(new Layer(MainLayer.Main, -1), tempTexture, aFrameSize ?? new Point(tempTexture.Width, tempTexture.Height), aPosition, aSize, aFrameSize == null ? new Vector2(tempTexture.Bounds.X, tempTexture.Bounds.Y) * 0.5f : aFrameSize.Value.ToVector2() * 0.5f, anAngle, aColor, aFrameDelay, 0, aRepeat, aRandomizeEffects ? (SpriteEffects)new Random().Next(4) : SpriteEffects.None);
            myRenderer.AccessStopAtCompletion = false;

            myHitDetector = new HitDetector(aPosition - aSize * 0.5f, aPosition + aSize * 0.5f, "Bullet", "Bullet" + AccessTargetType.ToString());
            myHitDetector.OnEnter += Hit;
            myHitDetector.AccessOwner = this;

            AccessHitDetector = myHitDetector;
            OnBoundCorrection += Correction;

            allBullets.Add(this);
        }

        protected override void Update(float aDeltaTime)
        {
            myLifetime -= aDeltaTime;
            myRenderer.AccessPosition = AccessPosition.PixelPosition();

            if (myLifetime <= 0 || (Player.AccessMainPlayer.AccessPosition - AccessPosition).Length() >= MAXDISTANCE)
            {
                Destroy();
            }
        }

        protected override void BeforeDestroy()
        {
            myHitDetector.Destroy();
            myRenderer.Destroy();
            myHitEntities?.Clear();
            myHitEntities = null;

            allBullets.Remove(this);
        }

        public override void UpdateHitDetector()
        {
            float tempFraction = (myLifetime / myMaxLifetime);
            Vector2 tempCornerOffset = myFlameCollider ? Vector2.Zero.Lerp(myWorldSize * 0.5f, 1 - tempFraction * tempFraction) : myWorldSize * 0.5f;
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

                if (!tempEntity.AccessInvincible && !myHitEntities.Contains(tempEntity))
                {
                    tempEntity.ChangeHP(-AccessDamage);
                }

                if (!AccessPiercing)
                {
                    Destroy();
                }
                else
                {
                    myHitEntities.Add(tempEntity);
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
