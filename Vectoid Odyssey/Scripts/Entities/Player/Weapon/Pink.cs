﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    abstract partial class PlayerWeapon
    {
        public class Pink : PlayerWeapon
        {
            const float
                DISTANCE = 2.5f,
                ANIMATIONTIME = 0.05f;

            private Player myPlayer;
            private float myCurrentCooldown;

            private Texture2D myTexture;
            private int myDamage;
            private float myBulletVelocity, myFireDelay;

            public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Pink;

            public Pink()
            {
                Dictionary<string, object[]> tempDictionary = new Dictionary<string, object[]>()
                {
                    { "Damage: hp", new object[] { 10, 1, 1, 1, 1, 1 } },

                    { "Velocity:", new object[] { 3.5f, 4.5f, 4.5f, 4.5f, 5.5f, 7.0f } },

                    { "Delay: s", new object[] { 0.8f, 0.085f, 0.775f, 0.07f, 0.065f, 0.06f } },
                };

                myTexture = Load.Get<Texture2D>("PinkBomb");
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
                if (myCurrentCooldown <= 0 && FireAmmo())
                {
                    Sound.PlayEffect("Shoot2");

                    Vector2 tempRotatedVector = new Vector2(1, 0).Rotate(myRotation);

                    Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * myBulletVelocity, Vector2.One, Bullet.TargetType.Enemy, Color.White, myDamage, 2.0f, false, aTexture: myTexture, aGravityBool: true);

                    myCurrentCooldown = myFireDelay;

                    SimpleAnimation(ANIMATIONTIME);
                }
            }

            protected override void SetStats(int aLevel)
            {
                myDamage = (int)myWeaponStats["Damage: hp", GetWeaponLevel];
                myBulletVelocity = (float)myWeaponStats["Velocity:", GetWeaponLevel];
                myFireDelay = (float)myWeaponStats["Delay: s", GetWeaponLevel];
            }
        }
    }
}
