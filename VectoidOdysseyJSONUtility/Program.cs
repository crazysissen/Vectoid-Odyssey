using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace VectoidOdysseyJSONUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Program main = new Program();
            main.Init(args);
        }

        void Init(string[] args)
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

            try
            {
                //string tempJSON = File.ReadAllText(tempPath);
                //Dictionary<string, object> tempRawData = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempJSON);

                //List<object> tempLayers = ((IEnumerable<object>)tempRawData["layers"]).ToList();

                //foreach (object layer in tempLayers)
                //{
                //    IEnumerable<object> tempCurrentLayer = (IEnumerable<object>)layer;

                //    if (((JProperty)tempCurrentLayer.First()).Name != "draworder")
                //    {
                //        WriteLine(ConsoleColor.DarkGray, "Irrelevant layer found.");
                //        continue;
                //    }

                //    string layerName = "";

                //    foreach (JProperty topMember in tempCurrentLayer)
                //    {
                //        switch (topMember.Name)
                //        {
                //            case "name":
                //                layerName = (string)topMember.Value;
                //                WriteLine(ConsoleColor.Gray, "Layer: {0}", layerName);
                //                break;


                //        }

                //    }
                //}

                Console.WriteLine("\nComplete!");
                Console.ReadKey(true);
                return;
            }
            catch
            {
                Write(ConsoleColor.Red, "Error: ");
                Console.Write("Could not read file.");
                Console.ReadKey(true);
                return;
            }
        }

        void Write(ConsoleColor aColor, string aFormat, params object[] arg)
        {
            ConsoleColor tempColor = Console.ForegroundColor;

            Console.ForegroundColor = aColor;
            Console.Write(aFormat, arg);
            Console.ForegroundColor = tempColor;
        }
        void WriteLine(ConsoleColor aColor, string aFormat, params object[] arg)
        {
            ConsoleColor tempColor = Console.ForegroundColor;

            Console.ForegroundColor = aColor;
            Console.WriteLine(aFormat, arg);
            Console.ForegroundColor = tempColor;
        }

        struct TiledObject
        {
            public string type, name;
            public float x, y, width, height;
        }
    }
}
