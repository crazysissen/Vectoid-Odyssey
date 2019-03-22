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
        private SoundEffect
            myMusic,
            sAngry, sDestroy, sIntro, sPop, sPopBig, sScreech, sShoot;

        private Texture2D
            tBody, tHead, tWeakPoint,
            tCannonTop, tCannonBot, tCannonArm,
            tEye, tEyeHit, tEyeDestroyed;

        public Arkan()
        {
            myMusic = Load.Get<SoundEffect>("ArkanTemp");

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
            tCannonTop = Load.Get<Texture2D>("ArkanCannonTop");
            tCannonBot = Load.Get<Texture2D>("ArkanCannonBot");
            tCannonArm = Load.Get<Texture2D>("ArkanCannonArm");
            tEye = Load.Get<Texture2D>("ArkanEye");
            tEyeHit = Load.Get<Texture2D>("ArkanEyeHit");
            tEyeDestroyed = Load.Get<Texture2D>("ArkanEyeDestroyed");
        }

        protected override void Update(float aDeltaTime)
        {
            
        }

        protected override void BeforeDestroy()
        {
            
        }
    }
}
