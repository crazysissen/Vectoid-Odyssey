using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class SplashHandler
    {
        const int
            FRAMES = 22;
        
        const float
            NULLTIME = 0.2f,
            BLINKDELAY = 0.9f,
            ANIMTIME = 1.5f,
            WAITTIME = 0.8f,
            FADETIME = 0.5f,
            ENDTIME = 0.4f;

        private Renderer.SpriteScreenFloating myRenderer;
        private TimerTable myTimer;
        private Action myCallback;
        private bool myPlayed;

        public void InitAndPlay(Action aCallback)
        {
            float tempDesiredSize = DCOdyssey.AccessResolution.Y * 0.3f;
            Texture2D tempTexture = Load.Get<Texture2D>("LogoAnimation");

            myCallback = aCallback;
            myRenderer = new Renderer.SpriteScreenFloating(Layer.Default, tempTexture, DCOdyssey.AccessResolution.ToVector2() * 0.5f, (tempDesiredSize / tempTexture.Height) * Vector2.One, Color.White, 0, 0.5f * new Vector2(64, 46), SpriteEffects.None);
            myTimer = new TimerTable(new float[] { NULLTIME, ANIMTIME, WAITTIME, FADETIME, ENDTIME });

            myRenderer.AccessActive = false;
        }

        public void Update(float aDeltaTime)
        {
            int tempStep = myTimer.Update(aDeltaTime);
            float tempProgress = myTimer.AccessCurrentStepProgress;

            if (myTimer.AccessComplete)
            {
                myCallback.Invoke();
                return;
            }

            myRenderer.AccessActive = tempStep != 0 && tempStep != 4;

            switch (tempStep)
            {
                case 1:
                    SetFrame(tempProgress);
                    if (!myPlayed && tempProgress > BLINKDELAY)
                    {
                        myPlayed = true;
                        Sound.PlayEffect("Blink");
                    }
                    break;

                case 2:
                    SetFrame(1);
                    break;
                    
                case 3:
                    myRenderer.AccessColor = new Color(1.0f, 1.0f, 1.0f, 1.0f - MathV.SineD(tempProgress));
                    break;
            }
        }

        public void Destroy()
        {
            myTimer = null;
            myRenderer.Destroy();
            myCallback = null;
        }

        private void SetFrame(float aProgress)
        {
            myRenderer.AccessSourceRectangle = new Rectangle(64 * (int)(aProgress * 21), 0, 64, 46);
        }
    }
}
