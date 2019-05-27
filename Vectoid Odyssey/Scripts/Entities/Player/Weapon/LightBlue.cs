using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    abstract partial class PlayerWeapon
    {
        public class LightBlue : PlayerWeapon
        {
            const float
                DISTANCE = 2.5f,
                ANIMATIONTIME = 0.05f;

            private Player myPlayer;
            private float myCurrentCooldown;

            private int myDamage;
            private float myBulletVelocity, myFireDelay;
            private Texture2D myTexture;

            public override PlayerWeaponType GetWeaponType => PlayerWeaponType.LightBlue;

            public LightBlue()
            {
                Dictionary<string, object[]> tempDictionary = new Dictionary<string, object[]>()
                {
                    { "Damage: hp", new object[] { 5, 1, 1, 1, 1, 1 } },

                    { "Velocity:", new object[] { 5.0f, 4.5f, 4.5f, 4.5f, 5.5f, 7.0f } },

                    { "Delay: s", new object[] { 0.7f, 0.085f, 0.775f, 0.07f, 0.065f, 0.06f } },
                };

                myTexture = Load.Get<Texture2D>("Laser");
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
                    Sound.PlayEffect("Shoot3");

                    Vector2 tempRotatedVector = new Vector2(1, 0).Rotate(myRotation);

                    Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * myBulletVelocity, Vector2.One, Bullet.TargetType.Enemy, Color.White, myDamage, 10, true, aTexture: myTexture, anAngle: myRotation);

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
