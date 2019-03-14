using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class WeaponTeal : PlayerWeapon
    {
        const float
            DISTANCE = 1.0f,
            VELOCITY = 4.0f,
            FIREDELAY = 0.4f;

        const int
            DAMAGE = 10;

        private Player myPlayer;
        private float myCurrentCooldown;

        public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Teal;

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

                Bullet tempNewBullet = new Bullet(AccessRenderer.AccessPosition + tempRotatedVector * DISTANCE, tempRotatedVector * VELOCITY, Vector2.One * 2, Bullet.TargetType.Enemy, new Color(255, 243, 146), DAMAGE, false);

                myCurrentCooldown = FIREDELAY;
            }
        }
    }

    //class WeaponTeal : PlayerWeapon
    //{
    //    public override PlayerWeaponType GetWeaponType => PlayerWeaponType.Teal;

    //    public override void Init(Player aPlayer)
    //    {

    //    }

    //    public override void Update(float aDeltaTime)
    //    {

    //    }

    //    public override void Fire()
    //    {

    //    }
    //}
}
