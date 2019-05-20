using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class PauseManager
    {
        public bool GetPaused => myPaused;

        private GUI.Collection myCollection;
        private Renderer.SpriteScreen myMenuPanel;
        private GUI.Button myResume, myOptions, myExit;
        private bool myPaused;

        public PauseManager()
        {
            myCollection = new GUI.Collection(true);

            myMenuPanel = new Renderer.SpriteScreen(Layer.GUI, Load.Get<Texture2D>("InGameMenu"), new Rectangle(100, 0, 280, 60), Color.Black)
            {
                AccessActive = false
            };

            myCollection.Add(myMenuPanel);
        }

        public void Update(float aDeltaTime, ref float aTimeMultiplier)
        {

        }

        public void Open()
        {

        }

        public void Close()
        {

        }

        void Resume()
        {

        }

        void Options()
        {

        }

        void Exit()
        {

        }
    }
}
