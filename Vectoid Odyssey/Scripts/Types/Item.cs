using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    class Item
    {
        public string GetName => myName;
        public int GetIndex => myIndex;

        readonly string myName;
        readonly int myIndex;

        public Item(string aName, int anIndex)
        {
            myName = aName;
            myIndex = anIndex;
        }
    }
}
