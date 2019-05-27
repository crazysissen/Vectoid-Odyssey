using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    // Bigger heavier variant on the Sock-type enemy, that bounces the player away as opposed to the enemy itself
    class EnemyCrab : Enemy
    {
        enum State
        {
            Idle, Follow, Charge, Jump
        }

        const float
            SPEED = 0.8f,
            DETECTIONRANGE = 16.0f,
            FOLLOWRANGE = 20.0f,
            BEGINCHARGERANGE = 3.7f,
            ENDCHARGERANGE = 4.2f,
            CHARGETIME = 0.15f,
            MINJUMPTIME = 0.45f,
            BOUNCESPEED = 0.8f,
            BOUNCEFORCE = 2.0f,
            BLOCKTIME = 0.8f;

        const int
            HEALTH = 8,
            DAMAGE = 2,
            SCORE = 5000;

        private Vector2 GetJump => new Vector2(2.0f, -1.5f);

        private Renderer.Animator myRenderer;
        private State myState;
        private float myCurrentCharge;
        private Texture2D myIdleTexture, myFollowTexture, myChargeTexture, myAttackTexture;

        public EnemyCrab(Vector2 aPosition)
        {
            AccessHealth = HEALTH;
            AccessDynamic = true;
            AccessWorldCollide = true;
            AccessGravity = true;
            AccessPosition = aPosition;

            myFollowTexture = Load.Get<Texture2D>("Enemy3");
            myIdleTexture = Load.Get<Texture2D>("Enemy3-2");
            myAttackTexture = Load.Get<Texture2D>("Enemy3-3");
            myChargeTexture = Load.Get<Texture2D>("Enemy3-4");

            myRenderer = new Renderer.Animator(Layer.Default, myIdleTexture, new Point(16, 16), AccessPosition, Vector2.One, new Vector2(8, 8), 0, Color.White, 0.6f, 0, true, SpriteEffects.None);

            AccessHitDetector = new HitDetector(aPosition - Vector2.One, aPosition + Vector2.One, "BulletTarget", "Enemy");
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

            if (tempPlayer.GetDead && myState != State.Jump)
            {
                myState = State.Idle;

                myRenderer.AccessTexture = myIdleTexture;
                myRenderer.AccessTimeInterval = 0.6f;
            }

            switch (myState)
            {
                case State.Idle:

                    if (GetOnGround)
                    {
                        AccessVelocity = new Vector2(0, AccessVelocity.Y);
                    }

                    if (tempPlayerDistance < DETECTIONRANGE && !tempPlayer.GetDead)
                    {
                        myState = State.Follow;

                        myRenderer.AccessTexture = myFollowTexture;
                        myRenderer.AccessTimeInterval = 0.2f;
                    }

                    break;

                case State.Follow:

                    AccessVelocity = new Vector2(tempPlayer.AccessPosition.X < AccessPosition.X ? -SPEED : SPEED, AccessVelocity.Y);

                    if (tempPlayerDistance < BEGINCHARGERANGE)
                    {
                        myCurrentCharge = 0;

                        myState = State.Charge;

                        myRenderer.AccessTexture = myChargeTexture;
                        myRenderer.AccessTimeInterval = 0.1f;
                    }
                    else if (tempPlayerDistance > FOLLOWRANGE)
                    {
                        myState = State.Idle;

                        myRenderer.AccessTexture = myIdleTexture;
                        myRenderer.AccessTimeInterval = 0.6f;
                    }

                    break;

                case State.Charge:

                    myRenderer.AccessTime = 0;

                    myCurrentCharge += aDeltaTime;

                    AccessVelocity = new Vector2(0, AccessVelocity.Y);

                    if (tempPlayerDistance > ENDCHARGERANGE)
                    {
                        myState = State.Follow;

                        myRenderer.AccessTexture = myFollowTexture;
                        myRenderer.AccessTimeInterval = 0.2f;
                    }
                    else if (myCurrentCharge > CHARGETIME)
                    {
                        Jump();
                        myCurrentCharge = 0;
                        myState = State.Jump;

                        myRenderer.AccessTexture = myAttackTexture;
                        myRenderer.AccessTimeInterval = 0.05f;
                    }

                    break;

                case State.Jump:

                    myCurrentCharge += aDeltaTime;

                    break;
            }
        }

        public override void UpdateHitDetector()
        {
            AccessHitDetector.Set(AccessPosition - Vector2.One, AccessPosition + Vector2.One);
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

                Vector2 tempNVector = ((AccessPosition + new Vector2(0, 0.5f)) - tempPlayer.AccessPosition).Normalized();

                if (!tempPlayer.AccessInvincible)
                {
                    tempPlayer.ChangeHP(-DAMAGE);
                    tempPlayer.AccessVelocity = (-tempNVector * BOUNCEFORCE + new Vector2(tempNVector.X < 0 ? 1 : -1, -0.5f));
                    tempPlayer.BlockJump(BLOCKTIME);
                }

                myState = State.Jump;

                AccessVelocity = tempNVector * BOUNCESPEED + new Vector2(0, -1.3f);
            }
        }

        private void Correction(Vector2 aCorrection)
        {
            if (myState == State.Jump && myCurrentCharge > MINJUMPTIME)
            {
                myState = State.Follow;

                myRenderer.AccessTexture = myFollowTexture;
                myRenderer.AccessTimeInterval = 0.2f;
            }
        }

        private void Jump()
        {
            AccessVelocity = GetJump * new Vector2(Player.AccessMainPlayer.AccessPosition.X > AccessPosition.X ? 1 : -1, 1);
        }
    }
}
