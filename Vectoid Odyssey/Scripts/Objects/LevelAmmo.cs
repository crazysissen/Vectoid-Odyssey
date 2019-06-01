using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class LevelAmmo : LevelPickup
    {
        private int myAmmo, myWeaponType;

        public LevelAmmo(Vector2 aPosition, int aWeaponType, int someAmmo) : base(aPosition, Load.Get<Texture2D>((new string[] { "", "RedAmmo", "GreenAmmo", "PinkAmmo", "LightBlueAmmo", "OrangeAmmo" })[aWeaponType]))
        {
            myUseBounce = true;
            myAnimation = PickupAnimation.Bounce;

            myAmmo = someAmmo;
            myWeaponType = aWeaponType;
        }

        protected override void Touch(Player aPlayer)
        {
        }

        protected override void Activate(Player aPlayer)
        {
            aPlayer.AddAmmo(myWeaponType, myAmmo);
        }
    }
}
