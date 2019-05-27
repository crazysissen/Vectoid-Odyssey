using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    abstract partial class PlayerWeapon
    {
        public class Orange : PlayerWeapon
        {
            const float
                DISTANCE = 1.5f,
                ANIMATIONTIME = 0.05f,
                VARIABILITY = 0.2f;

            private Player myPlayer;
            private float myCurrentCooldown;

            private Random myR = new Random();
            private Texture2D myTexture;
            private int myDamage;
            private float myBulletVelocity, myFireDelay;

            public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Orange;

            public Orange()
            {
                Dictionary<string, object[]> tempDictionary = new Dictionary<string, object[]>()
                {
                    { "Damage: hp", new object[] { 1, 1, 1, 1, 1, 1 } },

                    { "Velocity:", new object[] { 3.5f, 4.5f, 4.5f, 4.5f, 5.5f, 7.0f } },

                    { "Delay: s", new object[] { 0.05f, 0.085f, 0.775f, 0.07f, 0.065f, 0.06f } },
                };

                myTexture = Load.Get<Texture2D>("Flames");
                myWeaponStats = new WeaponStats(new Currency(12, 4, 0), tempDictionary);

                SetStats(0);

                string[] temp = myWeaponStats.GetUpgradeLog(3);
            }

            public override void Init(Player aPlayer)
            {
                myPlayer = aPlayer;
            }

            public override void Update(float aDeltaTime)
            {
                UpdateSimpleAnimation(aDeltaTime);

                if (myCurrentCooldown > 0)
                {
                    myCurrentCooldown -= aDeltaTime;
                }
            }

            public override void Fire()
            {
                if (myCurrentCooldown <= 0)
                {
                    Sound.PlayEffect("Flamethrower", 0.6f);

                    for (int i = 0; i < 3; i++)
                    {
                        float tempLifespan = (float)myR.NextDouble() * 0.3f + 0.6f;

                        Vector2 tempRotatedVector = new Vector2(1, 0).Rotate(myRotation + MathV.SineD((float)myR.NextDouble()) * VARIABILITY * (myR.Next(2) == 0 ? 1 : -1));
                        Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * myBulletVelocity, Vector2.One * 2, Bullet.TargetType.Enemy, Color.White, myDamage, tempLifespan, true, aTexture: myTexture, aFrameSize: new Point(7, 7), aFrameDelay: tempLifespan / 11f, aRandomizeEffects: true, aFlameCollider: true);
                    }

                    myCurrentCooldown = myFireDelay;

                    SimpleAnimation(ANIMATIONTIME);
                }
            }

            protected override void SetStats(int aLevel)
            {
                myDamage = (int)myWeaponStats["Damage: hp", WeaponLevel];
                myBulletVelocity = (float)myWeaponStats["Velocity:", WeaponLevel];
                myFireDelay = (float)myWeaponStats["Delay: s", WeaponLevel];
            }
        }
    }
}
