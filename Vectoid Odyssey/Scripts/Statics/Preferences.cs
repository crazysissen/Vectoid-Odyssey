using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCOdyssey
{
    static class Preferences
    {
        public enum Volume { Main, Music, SFX, UI }

        public static float GetVolume(Volume aCategory, bool aPreMultipliedBool = true) => myPreferences.volume[(int)aCategory] * ((aPreMultipliedBool && aCategory != Volume.Main) ? myPreferences.volume[0] : 1);
        public static void SetVolume(Volume aCategory, float aValue) => myPreferences.volume[(int)aCategory] = aValue;

        private static UserPreferences myPreferences;

        public static void Init()
        {
            myPreferences = FileManager.LoadPreferences();
        }

        public static void Save()
        {
            FileManager.SavePreferences(myPreferences);
        }

        //public float GetSettings();
    }

    partial class UserPreferences
    {
        public float[] volume = { 1, 1, 1, 1 };
    }
}
