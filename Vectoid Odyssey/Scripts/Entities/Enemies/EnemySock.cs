using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class EnemySock : Enemy
    {
        enum State
        {
            Idle, Follow, Charge, Jump
        }

        const float
            SPEED = 0.5f,
            DETECTIONRANGE = 24.0f,
            FOLLOWRANGE = 26.0f,
            BEGINCHARGERANGE = 10f,
            ENDCHARGERANGE = 13f,
            CHARGETIME = 0.5f,
            MINJUMPTIME = 0.1f,
            BOUNCESPEED = 1.4f;

        const int
            HEALTH = 4,
            DAMAGE = 2,
            SCORE = 1000;

        private Vector2 GetJump => new Vector2(3.0f, -2.8f);

        private Renderer.Animator myRenderer;
        private State myState;
        private float myCurrentCharge;

        public EnemySock(Vector2 aPosition)
        {
            AccessHealth = HEALTH;
            AccessDynamic = true;
            AccessWorldCollide = true;
            AccessGravity = true;
            AccessPosition = aPosition;

            myRenderer = new Renderer.Animator(Layer.Default, Load.Get<Texture2D>("Enemy1"), new Point(8, 8), AccessPosition, Vector2.One, new Vector2(4, 4), 0, Color.White, 0.2f, 0, true, SpriteEffects.None);

            AccessHitDetector = new HitDetector(aPosition - Vector2.One * 0.5f, aPosition + Vector2.One * 0.5f, "BulletTarget", "Enemy");
            AccessHitDetector.AccessOwner = this;
            AccessHitDetector.OnEnter += Collide;
            OnBoundCorrection += Correction;

            myState = State.Idle;
        }

        protected override void Update(float aDeltaTime)
        {
            myRenderer.AccessPosition = AccessPosition.PixelPosition();

            if (myState != State.Idle || myState != State.Jump)
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

                    myRenderer.AccessTime = 0;

                    if (GetOnGround)
                    {
                        AccessVelocity = new Vector2(0, AccessVelocity.Y);
                    }

                    if (tempPlayerDistance < DETECTIONRANGE && !tempPlayer.GetDead)
                    {
                        myState = State.Follow;
                    }

                    break;

                case State.Follow:

                    AccessVelocity = new Vector2(tempPlayer.AccessPosition.X < AccessPosition.X ? -SPEED : SPEED, AccessVelocity.Y);

                    if (tempPlayerDistance < BEGINCHARGERANGE)
                    {
                        myCurrentCharge = 0;
                        myState = State.Charge;
                    }
                    else if (tempPlayerDistance > FOLLOWRANGE)
                    {
                        myState = State.Idle;
                    }

                    break;

                case State.Charge:

                    myRenderer.AccessTime = 0;

                    myCurrentCharge += aDeltaTime;

                    AccessVelocity = new Vector2(0, AccessVelocity.Y);

                    if (tempPlayerDistance > ENDCHARGERANGE)
                    {
                        myState = State.Follow;
                    }
                    else if (myCurrentCharge > CHARGETIME)
                    {
                        Jump();
                        myCurrentCharge = 0;
                        myState = State.Jump;
                    }

                    break;

                case State.Jump:

                    myCurrentCharge += aDeltaTime;

                    break;
            }
        }

        public override void UpdateHitDetector()
        {
            AccessHitDetector.Set(AccessPosition - Vector2.One * 0.5f, AccessPosition + Vector2.One * 0.5f);
        }

        protected override void Death()
        {
            Player.AccessMainPlayer.AddScore(SCORE);

            myRenderer.Destroy();
            AccessHitDetector.Destroy();

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

                AccessVelocity = (AccessPosition - tempPlayer.AccessPosition).Normalized() * BOUNCESPEED - new Vector2(0, 0.5f);
            }
        }

        private void Correction(Vector2 aCorrection)
        {
            if (myState == State.Jump && myCurrentCharge > MINJUMPTIME)
            {
                myState = State.Follow;
            }
        }

        private void Jump()
        {
            AccessVelocity = GetJump * new Vector2(Player.AccessMainPlayer.AccessPosition.X > AccessPosition.X ? 1 : -1, 1);
        }
    }
}
