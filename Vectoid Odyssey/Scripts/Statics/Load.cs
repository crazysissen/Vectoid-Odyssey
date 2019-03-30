using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace VectoidOdyssey
{
    /// <summary>
    /// The main content manager/importer.
    /// </summary>
    static class Load
    {
        static Dictionary<string, object> myContentDictionary;
        static Dictionary<string, object> myContentCollections;

        static string[] ignoredExtensions = { ".ttf", ".ogg" };

        static Load()
        {
            myContentDictionary = new Dictionary<string, object>();
            myContentCollections = new Dictionary<string, object>();
        }

        public static void ImportAll(ContentManager aContent)
        {
            ContentBundle tempBundle = AllFileNames(AppDomain.CurrentDomain.BaseDirectory + "/" + aContent.RootDirectory, "", "");

            foreach (ImportObject item in tempBundle.objects)
            {
                myContentDictionary.Add(item.name, aContent.Load<object>(item.path));
            }

            foreach (ImportCollection item in tempBundle.collections)
            {
                List<object> tempObjects = new List<object>();

                foreach (string tag in item.names)
                {
                    tempObjects.Add(myContentDictionary[tag]);
                }

                myContentCollections.Add(item.collectionName, tempObjects.ToArray());
            }
        }

        public static ContentBundle AllFileNames(string aBasePath, string anAdditionalPath, string anAppendableAdditionalPath)
        {
            List<ImportObject> tempAllFiles = new List<ImportObject>();
            List<ImportCollection> tempAllCollections = new List<ImportCollection>();

            DirectoryInfo tempDirectory = new DirectoryInfo(aBasePath + "/" + anAdditionalPath);
            DirectoryInfo[] tempDirectories = tempDirectory.GetDirectories();
            FileInfo[] tempFiles = tempDirectory.GetFiles();

            string tempCurrentCollection = anAppendableAdditionalPath.Length == 0 ? "Root" : anAppendableAdditionalPath.Remove(anAppendableAdditionalPath.Length - 1);

            foreach (FileInfo file in tempFiles)
            {
                string tempCurrentName = Path.GetFileNameWithoutExtension(file.FullName);

                if (!ignoredExtensions.Contains(file.Extension))
                {
                    tempAllFiles.Add(new ImportObject(tempCurrentName, anAppendableAdditionalPath + tempCurrentName));
                }
            }

            foreach (DirectoryInfo dir in tempDirectories)
            {
                ContentBundle tempDirImport = AllFileNames(aBasePath, anAdditionalPath + dir.Name + "/", anAppendableAdditionalPath + dir.Name + "/");

                tempAllFiles.AddRange(tempDirImport.objects);
                tempAllCollections.AddRange(tempDirImport.collections);
            }

            List<string> tempCurrentCollectionNames = new List<string>();

            foreach (ImportObject file in tempAllFiles)
            {
                tempCurrentCollectionNames.Add(file.name);
            }

            tempAllCollections.Add(new ImportCollection(tempCurrentCollectionNames.ToArray(), tempCurrentCollection));

            return new ContentBundle(tempAllFiles.ToArray(), tempAllCollections.ToArray());
        }

        public static T Get<T>(string aTag)
        {
            try
            {
                if (myContentDictionary[aTag] is T)
                {
                    return ((T)myContentDictionary[aTag]);
                }

                return default(T);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Tried to get unloaded content [" + aTag + ", " + typeof(T) + "]");
                return default(T);
            }
        }

        public static T[] GetRange<T>(params string[] someTags)
        {
            T[] tempReturnArray = new T[someTags.Length];

            for (int i = 0; i < tempReturnArray.Length; ++i)
            {
                tempReturnArray[i] = Get<T>(someTags[i]);
            }

            return tempReturnArray;
        }

        public static T[] GetCollection<T>(string aTag)
        {
            if (myContentCollections.ContainsKey(aTag))
            {
                if (myContentCollections[aTag] is T[])
                {
                    return myContentCollections[aTag] as T[];
                }

                if (myContentCollections[aTag] is object[] tempArray)
                {
                    T[] tempNewArray = new T[tempArray.Length];

                    for (int i = 0; i < tempNewArray.Length; i++)
                    {
                        tempNewArray[i] = (T)tempArray[i];
                    }

                    return tempNewArray;
                }

                System.Diagnostics.Debug.WriteLine("ERROR: Tried to get content bundle already loaded into another type [" + aTag + ", requested type: " + typeof(T[]) + ", loaded type: " + myContentCollections[aTag].GetType() + "]");
                return myContentCollections[aTag] as T[];
            }

            System.Diagnostics.Debug.WriteLine("ERROR: Tried to get unloaded content bundle [" + aTag + ", " + typeof(T[]) + "]");
            return null;
        }

        public static bool Exists(string aTag) => myContentDictionary.ContainsKey(aTag);

        public static void Add(string aTag, object anObj) => myContentDictionary.Add(aTag, anObj);

        // Merely data storage. As is stated elsewhere in this solution, application of code standard is unnecessary and redundant.

        public struct ContentBundle
        {
            public ImportObject[] objects;
            public ImportCollection[] collections;

            public ContentBundle(ImportObject[] objects, ImportCollection[] collections)
            {
                this.objects = objects;
                this.collections = collections;
            }
        }

        public struct ImportObject
        {
            public string name;
            public string path;

            public ImportObject(string name, string path)
            {
                this.name = name;
                this.path = path;
            }
        }

        public struct ImportCollection
        {
            public string[] names;
            public string collectionName;

            public ImportCollection(string[] names, string collectionName)
            {
                this.names = names;
                this.collectionName = collectionName;
            }
        }
    }
}