using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VectoidOdysseyJSONUtility
{
    class Program
    {
        const int
            TILESIZE = 16;

        private static void Main(string[] args)
        {
            Program main = new Program();
            main.Init(args);
        }

        private void Init(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            string tempPath = null;

            if (args.Length == 0)
            {
                string tempAlternativePath = AppDomain.CurrentDomain.BaseDirectory + "Map.json";

                if (File.Exists(tempAlternativePath))
                {
                    tempPath = tempAlternativePath;
                }
                else
                {
                    Write(ConsoleColor.Red, "Error: ");
                    Console.Write("No file found. Please open Tiled .json file with this application, alternatively place 'Map.json' in this directory.");
                    Console.ReadKey(true);
                    return;
                }
            }
            else
            {
                tempPath = args[0];
            }

            Write(ConsoleColor.Yellow, "File found: ");
            Console.WriteLine(tempPath);
            //Console.ReadKey(true);

            //try
            {
                string tempJSON = File.ReadAllText(tempPath);
                Dictionary<string, object> tempRawData = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempJSON);

                Dictionary<string, (float w, float h, float x, float y, string c)[]> tempExtractedContent = new Dictionary<string, (float w, float h, float x, float y, string c)[]>();

                List<object> tempLayers = ((IEnumerable<object>)tempRawData["layers"]).ToList();

                foreach (object layer in tempLayers)
                {
                    IEnumerable<object> tempCurrentLayer = (IEnumerable<object>)layer;

                    string tempName = ((JProperty)tempCurrentLayer.First()).Name;

                    if (tempName != "draworder" && tempName != "color")
                    {
                        WriteLine(ConsoleColor.DarkGray, "Irrelevant layer found.");
                        continue;
                    }

                    string layerName = "";
                    TiledObject[] tempObjects = new TiledObject[0];

                    foreach (JProperty topMember in tempCurrentLayer)
                    {
                        switch (topMember.Name)
                        {
                            case "name":
                                layerName = (string)topMember.Value;
                                WriteLine(ConsoleColor.White, "Object Layer: \"{0}\"", layerName);
                                break;

                            case "objects":
                                JEnumerable<JToken> tempMembers = topMember.First.Children();
                                tempObjects = GetObjects(tempMembers);

                                WriteLine(ConsoleColor.Gray, "\t{0} Objects extracted...", tempObjects.Length);
                                break;
                        }
                    }

                    if (tempObjects != null && tempObjects.Length > 0)
                    {
                        List<(float w, float h, float x, float y, string c)> tempCurrentContent = new List<(float w, float h, float x, float y, string c)>();

                        foreach (TiledObject current in tempObjects)
                        {
                            tempCurrentContent.Add((current.width, current.height, current.x, current.y, current.content));
                        }

                        tempExtractedContent.Add(layerName, tempCurrentContent.ToArray());
                    }
                }

                File.WriteAllBytes(tempPath.Split('.')[0] + ".dcomap", tempExtractedContent.ToBytes());

                Console.WriteLine("\nComplete!");
                Console.ReadKey(true);
                return;
            }
            //catch
            //{
            //    Write(ConsoleColor.Red, "Error: ");
            //    Console.Write("Could not read file.");
            //    Console.ReadKey(true);
            //    return;
            //}

            Console.ReadKey(true);
        }

        private TiledObject[] GetObjects(JEnumerable<JToken> someTokens)
        {
            try
            {
                List<TiledObject> tempObjectList = new List<TiledObject>();

                foreach (JToken token in someTokens)
                {
                    if (!token.Value<bool>("visible"))
                    {
                        continue;
                    }

                    tempObjectList.Add(new TiledObject()
                    {
                        height = token.Value<int>("height") / TILESIZE,
                        width = token.Value<int>("width") / TILESIZE,
                        x = token.Value<int>("x") / TILESIZE,
                        y = token.Value<int>("y") / TILESIZE,
                        content = token.Value<string>("name"),
                    });
                }

                return tempObjectList.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private void Write(ConsoleColor aColor, string aFormat, params object[] arg)
        {
            ConsoleColor tempColor = Console.ForegroundColor;

            Console.ForegroundColor = aColor;
            Console.Write(aFormat, arg);
            Console.ForegroundColor = tempColor;
        }

        private void WriteLine(ConsoleColor aColor, string aFormat, params object[] arg)
        {
            ConsoleColor tempColor = Console.ForegroundColor;

            Console.ForegroundColor = aColor;
            Console.WriteLine(aFormat, arg);
            Console.ForegroundColor = tempColor;
        }

        [Serializable]
        struct TiledObject
        {
            public string content;
            public float x, y, width, height;
        }
    }
}
