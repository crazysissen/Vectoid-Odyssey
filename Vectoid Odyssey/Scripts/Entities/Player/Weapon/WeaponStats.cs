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

        private Dictionary<string, object[]> myDictionary;
        private string[] myTags;

        public object this[string aTag, int aLevel]
        {
            get
            {
                try
                {
                    return myDictionary[aTag][aLevel];
                }
                catch
                {
                    return null;
                }
            }
        }

        public object this[int anIndex, int aLevel]
        {
            get
            {
                try
                {
                    return myDictionary[myTags[anIndex]][aLevel];
                }
                catch
                {
                    return null;
                }
            }
        }

        public WeaponStats(Currency anUpgradeCost, Dictionary<string, object[]> someValues)
        {
            AccessUpgradeCost = anUpgradeCost;
            myDictionary = someValues;

            List<string> tempTags = new List<string>();
            foreach (KeyValuePair<string, object[]> item in someValues)
            {
                tempTags.Add(item.Key);
            }

            myTags = tempTags.ToArray();
        }

        public string[] GetUpgradeLog(int aCurrentLevel)
        {
            try
            {
                int length = PlayerWeapon.LEVELS;

                List<string> tempLog = new List<string>();
                for (int i = 0; i < length; ++i)
                {
                    tempLog.Add(string.Format("{0}: {1}{2} -> {3}{4}", myTags[i], this[i, aCurrentLevel].ToString(), this[i, aCurrentLevel + 1].ToString()));
                }

                return tempLog.ToArray();
            }
            catch
            {
                Console.WriteLine("ERROR: Could not get upgrade log.");
                return null;
            }
        }
    }
}
