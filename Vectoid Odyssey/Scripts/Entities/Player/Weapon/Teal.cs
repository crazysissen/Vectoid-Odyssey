using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    abstract partial class PlayerWeapon
    {
        public class Teal : PlayerWeapon
        {
            const float
                DISTANCE = 1.0f,
                ANIMATIONTIME = 0.08f;

            private float myCurrentCooldown;

            private int myDamage;
            private float myBulletVelocity, myFireDelay;

            public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Teal;

            public Teal()
            {
                Dictionary<string, object[]> tempDictionary = new Dictionary<string, object[]>()
                {
                    { "Damage: hp", new object[] { 1, 1, 2, 2, 3, 4 } },

                    { "Velocity:", new object[] { 5.0f, 4.5f, 4.5f, 4.5f, 5.5f, 7.0f } },

                    { "Delay: s", new object[] { 0.3f, 0.6f, 0.55f, 0.5f, 0.5f, 0.4f } },
                };

                myWeaponStats = new WeaponStats(new Currency(12, 4, 0), tempDictionary);

                SetStats(0);

                string[] temp = myWeaponStats.GetUpgradeLog(3);
            }

            public override void Init(Player aPlayer)
            {
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
                    Sound.PlayEffect("Shoot5");

                    Vector2 tempRotatedVector = new Vector2(1, 0).Rotate(myRotation);

                    Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * myBulletVelocity, Vector2.One * 2, Bullet.TargetType.Enemy, new Color(255, 243, 146), myDamage, 0.7f, false);

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
