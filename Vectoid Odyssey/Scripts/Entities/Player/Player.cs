using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class Player : WorldObject
    {
        const float
            ANIMATIONSPEED = 2.0f;

        public Vector2 GetWeaponOrigin => AccessPosition + new Vector2(0.0625f, -0.5625f) * 2;

        private PlayerWeapon GetActiveWeapon => myWeapons[myActiveWeapon];

        private Renderer.Sprite myBodyRenderer;

        private PlayerWeapon[] myWeapons;

        private float myMaxSpeed, myAcceleration, myBrakeAcceleration, myAnimationFrame = ANIMATIONSPEED * 0.5f;
        private int myActiveWeapon;

        public Player(Vector2 aPosition, PlayerSetup aSetup)
        {
            AccessDynamic = true;
            AccessPosition = aPosition;

            myBodyRenderer = new Renderer.Sprite(Layer.Default, aSetup.sheet, aPosition, Vector2.One, Color.White, 0, new Vector2(16, 16));

            myWeapons = aSetup.weapons;
            myMaxSpeed = aSetup.maxSpeed;
            myAcceleration = aSetup.acceleration;
            myBrakeAcceleration = aSetup.brakeAcceleration;

            UpdateRenderer();
        }

        protected override void Update(float aDeltaTime)
        {
            float tempTargetXVelocity = myMaxSpeed * ((Input.Pressed(Control.Right) ? 1 : 0) + (Input.Pressed(Control.Left) ? -1 : 0));
            bool tempBrake = (AccessVelocity.X < 0 && AccessVelocity.X < tempTargetXVelocity) || (AccessVelocity.X > 0 && AccessVelocity.X > tempTargetXVelocity);
            float tempVelocityChange = (tempTargetXVelocity - AccessVelocity.X < 0 ? -1 : 1 ) * aDeltaTime * (tempBrake ? myBrakeAcceleration : myAcceleration);

            bool tempOnGround = true; // TODO: Implement ground check

            if (tempOnGround)
            {
                AccessVelocity = new Vector2(AccessVelocity.X + tempVelocityChange, AccessVelocity.Y);
            }

            UpdateRenderer();
        }

        private void UpdateRenderer()
        {
            Animate();

            myBodyRenderer.AccessPosition = AccessPosition;

            PlaceWeapon();
        }

        private void PlaceWeapon()
        {
            if (GetActiveWeapon != null)
            {
                GetActiveWeapon.AccessRenderer.AccessPosition = GetWeaponOrigin;
                GetActiveWeapon.SetRotation((RendererController.AccessCamera.ScreenToWorldPosition(Input.GetMousePosition.ToVector2()) - GetWeaponOrigin).ToRadian());
            }
        }

        private void Animate()
        {
            myAnimationFrame = myAnimationFrame.Wrap(0, 4);

            myBodyRenderer.AccessSourceRectangle = new Rectangle(32 * (int)myAnimationFrame, 0, 32, 32);
        }
    }
}
