using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class LevelItem : LevelPickup
    {
        public Item AccessItem { get; set; }

        public LevelItem(Item anItem, Vector2 aPosition, float anInterval = 0, Point? aFrameSize = null) : base(aPosition, anItem.GetTexture, anInterval, aFrameSize, anItem.AccessColor)
        {
            myUseBounce = true;
            myAnimation = PickupAnimation.Bounce;
            AccessItem = anItem;

            if (anItem.GetItemType == ItemType.Key)
            {
                Color tempColor = Door.KeyColor(anItem.GetIndex);
                myRenderer.AccessColor = tempColor;
                anItem.AccessColor = tempColor;
            }
        }

        protected override void Touch(Player aPlayer)
        {

        }

        protected override void Activate(Player aPlayer)
        {
            aPlayer.PickupItem(AccessItem);
        }
    }
}
