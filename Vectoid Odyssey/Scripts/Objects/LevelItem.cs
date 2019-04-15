using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    class LevelItem : LevelPickup
    {
        public Item AccessItem { get; set; }

        public LevelItem(Item anItem, Vector2 aPosition, Texture2D aSpriteSheet, float anInterval, Point aFrameSize, Color? aColor = null) : base(aPosition, aSpriteSheet, anInterval, aFrameSize, aColor)
        {
            AccessItem = anItem;
        }

        protected override void Touch()
        {
        }

        protected override void Activate()
        {
        }
    }
}
