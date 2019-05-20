using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class PlayerInteraction
    {
        const float MAXDISTANCE = 5.0f;

        public event Action<Player> OnInteract;

        public bool AccessActive { get; set; } 
        public string GetPrompt => myPrompt;

        private static List<PlayerInteraction> interactions;

        private readonly string myPrompt;
        private readonly Vector2[] myOrigins;

        public PlayerInteraction(string aPrompt, bool anActiveBool, params Vector2[] someOrigins)
        {
            myPrompt = aPrompt;
            myOrigins = someOrigins;

            if (interactions == null)
            {
                interactions = new List<PlayerInteraction>();
            }

            interactions.Add(this);
        }

        public void Activate(Player aPlayer)
        {
            OnInteract?.Invoke(aPlayer);
        }

        public void Destroy()
        {
            interactions.Remove(this);
        }

        public static PlayerInteraction Closest(Vector2 anOrigin)
        {
            if (interactions.Count == 0)
            {
                return null;
            }

            PlayerInteraction tempClosest = null;
            float tempDistance = MAXDISTANCE;

            foreach (PlayerInteraction interaction in interactions)
            {
                if (!interaction.AccessActive)
                {
                    continue;
                }

                foreach (Vector2 origin in interaction.myOrigins)
                {
                    if ((origin - anOrigin).Length() < tempDistance )
                    {
                        tempClosest = interaction;
                    }
                }
            }

            return tempClosest;
        }
    }
}
