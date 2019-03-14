using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    struct WeaponStats
    {
        public int GetCount => myTags.Length;

        public Currency AccessUpgradeCost { get; private set; }

        private Dictionary<string, object> myDictionary;
        private string[] myTags;

        public object this[int anIndex]
        {

        }

        public WeaponStats(Dictionary<string, object> someValues)
        {

        }
    }
}
