using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class PlayerInteraction
    {
        public event Action<Player> OnInteract;

        public bool AccessActive { get; set; }
        public string GetPrompt => myPrompt;

        private static List<PlayerInteraction> interactions;

        private readonly string myPrompt;
        private readonly Vector2[] myOrigins;

        public PlayerInteraction(string aPrompt, params Vector2[] someOrigins)
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

        public static PlayerInteraction Closest(Vector2 anOrigin)
        {
            if (interactions.Count == 0)
            {
                return null;
            }

            PlayerInteraction tempClosest = null;
            float tempDistance = float.MaxValue;

            foreach (PlayerInteraction interaction in interactions)
            {
                foreach (Vector2 origin in interaction.myOrigins)
                {
                    if ((origin - anOrigin).Length() < tempDistance)
                    {
                        tempClosest = interaction;
                    }
                }
            }

            return tempClosest;
        }
    }
}
