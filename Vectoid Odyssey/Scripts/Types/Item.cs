using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    enum ItemType
    {
        Key
    }

    class Item
    {
        public virtual ItemType GetItemType => myItemType;
        public virtual string GetName => myName;
        public virtual int GetIndex => myIndex;

        readonly ItemType myItemType;
        readonly string myName;
        readonly int myIndex;

        public Item(string aName, int anIndex, ItemType anItemType)
        {
            myName = aName;
            myIndex = anIndex;
            myItemType = anItemType;
        }
    }
}
