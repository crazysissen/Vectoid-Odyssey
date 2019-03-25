using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace VectoidOdyssey
{
    sealed class Arkan : Boss
    {
        const float
            ARMBOUNCESPEED = 0.5f;

        private enum Pattern
        {
            ShootA, ShootB, Ram, Laser, Weak
        }

        private Renderer.Sprite
            myHead, myEyeL, myEyeR, myBody,
            myCannonL1, myCannonL2, myCannonR1, myCannonR2,
            myArmL1, myArmL2, myArmR1, myArmR2;

        private SoundEffect
            myMusic,
            sAngry, sDestroy, sIntro, sPop, sPopBig, sScreech, sShoot;

        private Texture2D
            tBody, tHead, tWeakPoint, tFireball,
            tCannonTop, tCannonBot, tCannonArm,
            tEye, tEyeHit, tEyeDestroyed;

        public Arkan()
        {


            SetRendererPositions(0);

            #region Import

            myMusic = Load.Get<SoundEffect>("SongArkan");

            sAngry = Load.Get<SoundEffect>("ArkanAngry");
            sDestroy = Load.Get<SoundEffect>("ArkanDestroy");
            sIntro = Load.Get<SoundEffect>("ArkanIntro");
            sPop = Load.Get<SoundEffect>("ArkanPop");
            sPopBig = Load.Get<SoundEffect>("ArkanPopBig");
            sScreech = Load.Get<SoundEffect>("ArkanScreech");
            sShoot = Load.Get<SoundEffect>("ArkanShoot");

            tBody = Load.Get<Texture2D>("ArkanBody");
            tHead = Load.Get<Texture2D>("ArkanHead");
            tWeakPoint = Load.Get<Texture2D>("ArkanWeakPoint");
            tFireball = Load.Get<Texture2D>("ArkanFireball");
            tCannonTop = Load.Get<Texture2D>("ArkanCannonTop");
            tCannonBot = Load.Get<Texture2D>("ArkanCannonBot");
            tCannonArm = Load.Get<Texture2D>("ArkanCannonArm");
            tEye = Load.Get<Texture2D>("ArkanEye");
            tEyeHit = Load.Get<Texture2D>("ArkanEyeHit");
            tEyeDestroyed = Load.Get<Texture2D>("ArkanEyeDestroyed");

            #endregion
        }

        protected override void Update(float aDeltaTime)
        {


            SetRendererPositions(aDeltaTime);
        }

        protected override void BeforeDestroy()
        {
            
        }

        private void SetRendererPositions(float aDeltaTime)
        {

        }
    }
}
