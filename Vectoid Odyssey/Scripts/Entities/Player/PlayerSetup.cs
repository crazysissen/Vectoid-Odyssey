using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    // Since this struct is solely made to store information conveniently, and never interacts with it's own members internally, I think the implied code standard is redundant and unnecessary.

    [Serializable]
    struct PlayerSetup
    {
        public Texture2D sheet, crouchSheet;
        public int health;
        public float maxSpeed, acceleration, brakeAcceleration, crouchSpeedModifier, maxJumpTime, jumpStartAcceleration, jumpEndAcceleration, nonLinear;
        public PlayerWeapon[] weapons;
    }
}
