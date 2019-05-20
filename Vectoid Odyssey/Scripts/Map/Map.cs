using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    abstract class Map
    {
        protected List<RoomBounds> myBounds;
        protected RoomBounds[] myLastBounds;

        public Map()
        {
            myBounds = new List<RoomBounds>();
            myLastBounds = new RoomBounds[0x10000];
        }

        public abstract Player SpawnPlayer(PlayerSetup aPlayerSetup, MenuManager aMenuManager);

        public abstract void ActivateEnemies();

        public virtual void Update(float aDeltaTime)
        {

        }

        public virtual RoomBounds GetRoom(Vector2 aPosition)
        {
            if (myBounds.Count == 0)
                return null;

            if (myBounds.Count == 1)
                return myBounds[0];

            List<RoomProximity> tempProximity = new List<RoomProximity>();

            foreach (RoomBounds bounds in myBounds)
            {
                if (bounds.InRoom(aPosition))
                {
                    return bounds;
                }

                tempProximity.Add(new RoomProximity() { distance = (bounds.AccessCenter - aPosition).Length(), bounds = bounds });
            }

            tempProximity = tempProximity.OrderBy(o => o.distance).ToList();
            return tempProximity[0].bounds;
        }

        public virtual RoomBounds GetRoom(Vector2 aPosition, int anObjectIndex)
        {
            RoomBounds tempLast = myLastBounds[anObjectIndex];

            if (tempLast != null && tempLast.InRoom(aPosition))
                return tempLast;

            RoomBounds tempBounds = GetRoom(aPosition);

            myLastBounds[anObjectIndex] = tempBounds;
            return tempBounds;
        }

        private struct RoomProximity { public float distance; public RoomBounds bounds; }
    }
}
