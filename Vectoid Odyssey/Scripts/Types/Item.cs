using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    enum ItemType
    {
        Key, Other
    }

    class Item
    {
        public virtual ItemType GetItemType => myItemType;
        public virtual string GetName => myName;
        public virtual int GetIndex => myIndex;
        public virtual Texture2D GetTexture => myTexture;
        public virtual Color? AccessColor { get; set; }

        readonly Texture2D myTexture;
        readonly ItemType myItemType;
        readonly string myName;
        readonly int myIndex;

        public Item(string aName, int anIndex, ItemType anItemType, Texture2D aTexture, Color? aColor = null)
        {
            AccessColor = aColor;
            myTexture = aTexture;
            myName = aName;
            myIndex = anIndex;
            myItemType = anItemType;
        }
    }
}
