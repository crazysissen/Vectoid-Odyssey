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

        public Currency GetUpgradeCost  => myUpgradeCost;

        private Dictionary<string, object[]> myDictionary;
        private string[] myTags;
        private Currency myUpgradeCost;

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
            myUpgradeCost = anUpgradeCost;
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
                int length = myTags.Length;

                List<string> tempLog = new List<string>();
                for (int i = 0; i < length; ++i)
                {
                    string[] tempSplitName = myTags[i].Split(':');

                    tempLog.Add(string.Format("{0}: {1}{2} -> {3}{4}", tempSplitName[0], this[i, aCurrentLevel].ToString(), tempSplitName[1], this[i, aCurrentLevel + 1].ToString(), tempSplitName[1]));
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
