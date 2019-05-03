using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace VectoidOdyssey
{
    static class FileManager
    {
        private static decimal saveVersion = 0.0M;

        private static string PersistentPath => Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VectoidOdyssey").FullName;
        private static string PreferencesPath => PersistentPath + @"\UserPreferences.sav";
        private static string InfoPath => PersistentPath + @"\UserInfo.sav";
        private static string SaveFilePath(int aSaveIndex) => Directory.CreateDirectory(PersistentPath + @"\Saves").FullName + @"\File" + aSaveIndex + ".sav";

        public static void SavePreferences(UserPreferences aSaveFile)
        {
            File.WriteAllBytes(PreferencesPath, ToSaveFile(aSaveFile));
        }

        public static UserPreferences LoadPreferences()
        {
            try
            {
                byte[] tempLoadedBytes = File.ReadAllBytes(PreferencesPath);
                return ToSaveType<UserPreferences>(tempLoadedBytes);
            }
            catch { }

            Console.WriteLine("Preferences file unreadable or nonexistent, creating new.");
            return new UserPreferences();
        }

        /// <summary>Converts in order object -> dictionary -> bytes. Cannot contain reference types.</summary>
        public static byte[] ToSaveFile(object aSaveStruct)
        {
            Dictionary<string, object> tempDictionary = new Dictionary<string, object>();

            FieldInfo[] tempFields = aSaveStruct.GetType().GetFields();
            foreach (FieldInfo field in tempFields)
            {
                if (field.FieldType.IsClass)
                {
                    continue;
                }

                tempDictionary.Add(field.Name, field.GetValue(aSaveStruct));
            }

            return tempDictionary.ToBytes();
        }

        public static T ToSaveType<T>(byte[] aSaveFile) where T : class, new()
        {
            T tempType = new T();

            Dictionary<string, object> tempDictionary = aSaveFile.ToObject<Dictionary<string, object>>();

            FieldInfo[] tempFields = typeof(T).GetFields();
            foreach (FieldInfo field in tempFields)
            {
                if (tempDictionary.ContainsKey(field.Name))
                {
                    field.SetValue(tempType, tempDictionary[field.Name]);
                }
            }

            return tempType;
        }

        public abstract class SaveData
        {
            public int saveVersion;
        }
    }

    partial class UserPreferences : FileManager.SaveData { }
    partial class UserInfo : FileManager.SaveData { }
    partial class GameSave : FileManager.SaveData { }
}
