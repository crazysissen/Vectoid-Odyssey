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
        enum State
        {
            Idle, Follow, Fire
        }

        const float
            SPEED = 0.55f,
            DETECTIONRANGE = 32.0f,
            FOLLOWRANGE = 35.0f,
            BEGINFIRERANGE = 16.0f,
            ENDFIRERANGE = 24.0f,
            CHARGETIME = 1.5f,
            BULLETSPEED = 2.5f,
            BOUNCESPEED = 2.8f,
            ANIMATIONTIME = 0.2f,
            DAMPENSTRENGTH = 1.6f;

        const int
            HEALTH = 6,
            DAMAGE = 1,
            BUMPDAMAGE = 1,
            SCORE = 2000;

        private Vector2 GetJump => new Vector2(2.0f, -1.5f);

        private List<Bullet> myBullets = new List<Bullet>();
        private Renderer.Sprite myRenderer;
        private State myState;
        private float myCurrentCharge;

        public EnemySkull(Vector2 aPosition)
        {
            AccessHealth = HEALTH;
            AccessDynamic = true;
            AccessKeepInBounds = true;
            AccessWorldCollide = true;
            AccessPosition = aPosition;

            myRenderer = new Renderer.Sprite(Layer.Default, Load.Get<Texture2D>("Enemy2"), AccessPosition, Vector2.One, Color.White, 0, new Vector2(4, 4));
            myRenderer.AccessSourceRectangle = new Rectangle(0, 0, 8, 8);

            AccessBoundingBox = new HitDetector(aPosition - Vector2.One * 0.5f, aPosition + Vector2.One * 0.5f, "BulletTarget", "Enemy");
            AccessBoundingBox.AccessOwner = this;
            AccessBoundingBox.OnEnter += Collide;

            myState = State.Idle;
        }

        protected override void Update(float aDeltaTime)
        {
            myRenderer.AccessPosition = AccessPosition.PixelPosition();

            if (myState != State.Idle)
            {
                myRenderer.AccessEffects = Player.AccessMainPlayer.AccessPosition.X < AccessPosition.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }

            Player tempPlayer = Player.AccessMainPlayer;

            float tempPlayerDistance = (tempPlayer.AccessPosition - AccessPosition).Length();

            if (tempPlayer.GetDead)
            {
                myState = State.Idle;
            }

            switch (myState)
            {
                case State.Idle:

                    myRenderer.AccessSourceRectangle = new Rectangle(0, 0, 8, 8);

                    if (tempPlayerDistance < DETECTIONRANGE && !tempPlayer.GetDead)
                    {
                        myState = State.Follow;
                    }

                    break;

                case State.Follow:

                    myRenderer.AccessSourceRectangle = new Rectangle(0, 0, 8, 8);

                    AccessVelocity += (Player.AccessMainPlayer.AccessPosition - AccessPosition).Normalized() * aDeltaTime * SPEED;

                    if (tempPlayerDistance < BEGINFIRERANGE)
                    {
                        myCurrentCharge = ANIMATIONTIME;
                        myState = State.Fire;
                    }
                    else if (tempPlayerDistance > FOLLOWRANGE)
                    {
                        myState = State.Idle;
                    }

                    break;

                case State.Fire:

                    myRenderer.AccessSourceRectangle = new Rectangle(myCurrentCharge < ANIMATIONTIME ? 8 : 0, 0, 8, 8);

                    myCurrentCharge += aDeltaTime;

                    if (tempPlayerDistance > ENDFIRERANGE)
                    {
                        myState = State.Follow;
                    }
                    else if (myCurrentCharge > CHARGETIME)
                    {
                        Fire();
                        myCurrentCharge = 0;
                    }

                    break;
            }

            AccessVelocity -= AccessVelocity * DAMPENSTRENGTH * aDeltaTime;
        }

        protected override void UpdateHitDetector()
        {
            AccessBoundingBox.Set(AccessPosition - Vector2.One * 0.5f, AccessPosition + Vector2.One * 0.5f);
        }

        protected override void Death()
        {
            Player.AccessMainPlayer.AddScore(SCORE);

            myRenderer.Destroy();
            AccessBoundingBox.Destroy();

            Destroy();
        }

        private void Collide(HitDetector aHitDetector)
        {
            if (aHitDetector.AccessTags.Contains("Player"))
            {
                Player tempPlayer = (Player)aHitDetector.AccessOwner;

                if (!tempPlayer.AccessInvincible)
                {
                    tempPlayer.ChangeHP(-DAMAGE);
                }

                AccessVelocity = (AccessPosition - tempPlayer.AccessPosition + new Vector2(0, -0.8f)).Normalized() * BOUNCESPEED;
            }
        }

        private void Fire()
        {
            Vector2 tempDirection = (Player.AccessMainPlayer.AccessPosition - AccessPosition).Normalized();

            Bullet tempNewBullet = new Bullet(AccessPosition + tempDirection * 0.6f, tempDirection * BULLETSPEED, new Vector2(2, 2), Bullet.TargetType.Player, new Color(255, 121, 48), DAMAGE, false);
            myBullets.Add(tempNewBullet);
        }
    }
}
