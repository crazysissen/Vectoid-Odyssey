using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace DCOdyssey
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
            if (File.Exists(PreferencesPath))
            {
                try
                {
                    byte[] tempLoadedBytes = File.ReadAllBytes(PreferencesPath);
                    return ToSaveType<UserPreferences>(tempLoadedBytes);
                }
                catch { }
            }

            Console.WriteLine("Preferences file unreadable or nonexistent, creating new.");
            return new UserPreferences();
        }

        /// <summary>Converts in order object -> dictionary -> bytes. Cannot contain reference types. This is done to safeguard future save file versions, if a new value is added or if one is removed, the byte converter will not fail and the value will be defaulted.</summary>
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

        /// <summary>Converts in order bytes -> dictionary -> bytes. Cannot contain reference types, and all values of the save type should and must be set in the respective constructor (this provides a default value if the save file doesn't contain the value)</summary>
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
