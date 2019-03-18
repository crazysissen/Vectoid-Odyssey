using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    abstract partial class PlayerWeapon
    {
        public class Teal : PlayerWeapon
        {
            const float
                DISTANCE = 1.0f;
                
            private Player myPlayer;
            private float myCurrentCooldown;

            private int myDamage;
            private float myBulletVelocity, myFireDelay;

            public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Teal;

            public Teal()
            {
                Dictionary<string, object[]> tempDictionary = new Dictionary<string, object[]>()
                {
                    { "Damage:power", new object[] { 1, 1, 2, 2, 3, 4 } },

                    { "Velocity:", new object[] { 4.0f, 4.5f, 4.5f, 4.5f, 5.5f, 7.0f } },

                    { "Delay:seconds", new object[] { 0.4f, 0.6f, 0.6f, 0.7f, 0.8f, 0.8f } },
                };

                myWeaponStats = new WeaponStats(new Currency(12, 4, 0), tempDictionary);

                SetStats(0);
            }

            public override void Init(Player aPlayer)
            {
                myPlayer = aPlayer;
            }

            public override void Update(float aDeltaTime)
            {
                if (myCurrentCooldown > 0)
                {
                    myCurrentCooldown -= aDeltaTime;
                }
            }

            public override void Fire()
            {
                if (myCurrentCooldown <= 0)
                {
                    Vector2 tempRotatedVector = new Vector2(1, 0).Rotate(myRotation);

                    Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * myBulletVelocity, Vector2.One * 2, Bullet.TargetType.Enemy, new Color(255, 243, 146), myDamage, false);

                    myCurrentCooldown = myFireDelay;
                }
            }

            protected override void SetStats(int aLevel)
            {
                myDamage = (int)myWeaponStats["Damage:power", WeaponLevel];
                myBulletVelocity = (float)myWeaponStats["Velocity:", WeaponLevel];
                myFireDelay = (float)myWeaponStats["Delay:seconds", WeaponLevel];
            }
        }
    }
}
