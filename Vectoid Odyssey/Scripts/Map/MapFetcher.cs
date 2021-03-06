﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class MapFetcher
    {
        public enum MapType
        {
            Sewer1, Sewer2
        }

        private readonly Dictionary<MapType, Func<object[], Map>> myFetchMethods;

        public MapFetcher()
        {
            myFetchMethods = new Dictionary<MapType, Func<object[], Map>>()
            {
                { MapType.Sewer1, GetNewSewer1 },
                { MapType.Sewer2, GetNewSewer2 }
            };
        }

        public Map Get(MapType aType, object[] arg)
        {
            //try
            {
                return myFetchMethods[aType].Invoke(arg);
            }
            //catch
            //{
            //    return GetNewSewer1(null);
            //}
        }

        //public Map GetNewSewer1(object[] var)
        //{
        //    RoomBounds[] tempBounds =
        //    {
        //        new RoomBounds(4, 16, 16, 28, 16, 48, 28, 24),
        //        new RoomBounds(48, 28, 16, 28, 24, 52, 28, 12),
        //        new RoomBounds(52, 28, 24, 28, 12, 80, 28, 24),
        //        new RoomBounds(80, 28, 12, 28, 24, 84, 28, 4),
        //        new RoomBounds(84, 28, 24, 28, 4, 112, 4, 4)
        //    };

        //    Square.DoubleAll = true;
        //    Square[] tempWorldColliders =
        //    {
        //        new Square(64, 26, 4, 2),
        //        new Square(88, 27, 4, 1),
        //        new Square(104, 27, 4, 1),
        //        new Square(90, 25, 2, 2),
        //        new Square(104, 25, 2, 2),
        //        new Square(94, 22, 8, 2),
        //        new Square(84, 20, 4, 1),
        //        new Square(108, 20, 4, 1)
        //    };
        //    Square.DoubleAll = false;

        //    WorldObject[] tempObjects =
        //    {
        //        new EnemySock(new Vector2(66, 55)),
        //        new EnemySock(new Vector2(38, 55)),

        //        new EnemySkull(new Vector2(130, 34)),
        //        new EnemySkull(new Vector2(134, 34)),
        //        new EnemySock(new Vector2(126, 54)),
        //        new EnemySock(new Vector2(118, 54)),
        //        new EnemySock(new Vector2(138, 54)),
        //        new EnemySock(new Vector2(146, 54)),

        //        new EnemyCrab(new Vector2(200, 42)),
        //        new EnemyCrab(new Vector2(192, 42))
        //    };

        //    return new CompleteMap(Load.Get<Texture2D>("Sewer"), tempBounds, tempWorldColliders, tempObjects, new Vector2(52, 52));
        //}

        public Map GetNewSewer1(object[] var)
        {
            RoomBounds[] tempBounds =
            {
                new RoomBounds(104, 28, 24, 34, 20, 128, 28, 24), // Centre
                //new RoomBounds(88, 50, 4, 28, 24, 104, 34, 20), // Corridor Left
                new RoomBounds(76, 51, 6, 51, 4, 88, 28, 24), // Left Room
                new RoomBounds(-8, 0, 0, 80, 0, 70, 51, 47), // Outside Left
                new RoomBounds(52, 3, 3, 10, 3, 76, 50, 4), // Top Left Room
                new RoomBounds(168, 28, 24, 28, 0, 204, 0, 0) // Outside right
            };

            Square.DoubleAll = true;
            Square[] tempWorldColliders =
            {
                // Middle room
                new Square(108, 32, 16, 2),
                new Square(104, 28, 2, 1),
                new Square(126, 28, 2, 1),

                // Left room
                new Square(71, 12, 5, 36),
                new Square(82, 28, 6, 2),
                // Upwards platforms
                new Square(76, 24, 4, 1),
                new Square(84, 21, 4, 1),
                new Square(76, 18, 4, 1),
                new Square(84, 15, 4, 1),
                new Square(86, 11, 2, 1),
                // Top platform
                new Square(80, 8, 3, 1),
                new Square(76, 10, 2, 2),
                // Bottom
                new Square(76, 30, 1, 9),
                new Square(77, 34, 2, 1),
                new Square(77, 36, 1, 1),
                new Square(77, 37, 2, 1),
                new Square(77, 38, 4, 1),
                new Square(84, 38, 4, 1),
                new Square(85, 39, 3, 1),
                new Square(80, 42, 8, 1),
                new Square(82, 43, 6, 1),
                new Square(76, 46, 3, 2),
                new Square(84, 49, 4, 1),
                new Square(65, 50, 19, 1),

                // Outside
                new Square(65, 50, 14, 1),
                new Square(70, 44, 1, 4),
                new Square(69, 47, 1, 1),
                new Square(60, 50, 3, 1),
                new Square(55, 48, 3, 1),
                new Square(50, 46, 3, 1),
                new Square(48, 47, 2, 1),
                new Square(42, 50, 2, 1),
                new Square(35, 48, 3, 1),
                new Square(27, 47, 3, 1),
                new Square(26, 48, 1, 1),
                new Square(16, 46, 5, 1),
                new Square(7, 46, 6, 1),

                // Top left room
                new Square(60, 8, 20, 2),
                new Square(52, 3, 2, 1),
                new Square(52, 4, 1, 1),
                new Square(58, 3, 3, 1),
                new Square(59, 4, 2, 1),
                new Square(61, 4, 15, 2)
            };
            Square.DoubleAll = false;

            Texture2D tempKeyTexture = Load.Get<Texture2D>("Key");

            WorldObject[] tempObjects =
            {
                // -- Enemies

                // Items
                new LevelItem(new Item("Penthouse Key", 0, ItemType.Key, tempKeyTexture), new Vector2(10, 44.5f), 0.1f, new Point(8, 8)),
                new LevelItem(new Item("E Hallway Key", 1, ItemType.Key, tempKeyTexture), new Vector2(56, 8.5f), 0.1f, new Point(8, 8)),

                // Doors
                new Door(new Vector2(128, 24)),
                new Door(new Vector2(80, 4), 0),

                // Misc
                new Portal(new Vector2(372, 54)),
                new Pitfall(new Vector2(-32, 128), new Vector2(160, 160))
            };

            return new CompleteMap(Load.Get<Texture2D>("Sewer1"), tempBounds, tempWorldColliders, tempObjects, new Vector2(232, 62));
        }

        public Map GetNewSewer2(object[] var)
        {
            Dictionary<string, Load.MapObject[]> tempMapObjects = Load.GetMap("Sewer2");

            WorldObject[] tempObjects = GetObjects(tempMapObjects["Objects"]).Concat(GetPipes(tempMapObjects["Pipes"])).Concat(GetEntities(tempMapObjects["Entities"])).ToArray();
            tempObjects = tempObjects.Concat(new Portal[] { new Portal(new Vector2(638, 50)) } ).ToArray();

            return new CompleteMap(Load.Get<Texture2D>("Sewer2"), null, GetColliders(tempMapObjects["Collision"]), tempObjects, new Vector2(462, 407));
        }

        public WorldObject[] GetObjects(Load.MapObject[] someMapObjects)
        {
            WorldObject[] tempObjects = new WorldObject[someMapObjects.Length];

            for (int i = 0; i < tempObjects.Length; ++i)
            {
                Load.MapObject tempObject = someMapObjects[i];

                string[] tempContent = tempObject.content.Split(':');
                Vector2 tempPosition = new Vector2(tempObject.x, tempObject.y);

                Texture2D tempKeyTexture = Load.Get<Texture2D>("Key");

                switch (tempContent[0])
                {
                    case "Item":
                        tempObjects[i] = GetItem(tempContent, tempPosition);
                        break;

                    case "Door":
                        tempObjects[i] = new Door(tempPosition, tempContent.Length > 1 ? (int?)int.Parse(tempContent[1]) : null);
                        break;

                    case "Pitfall":
                        tempObjects[i] = new Pitfall(tempPosition * 2, (tempPosition + new Vector2(tempObject.width, tempObject.height)) * 2);
                        break;

                    default:
                        break;
                }
            }

            return tempObjects;
        }

        public Entity[] GetEntities(Load.MapObject[] someMapObjects)
        {
            Entity[] tempEntities = new Entity[someMapObjects.Length];

            for (int i = 0; i < tempEntities.Length; ++i)
            {
                Load.MapObject tempObject = someMapObjects[i];
                string[] tempContent = tempObject.content.Split(':');
                Vector2 tempPosition = 2 * new Vector2(tempObject.x, tempObject.y);

                if (tempContent[0] == "Enemy")
                {
                    switch (tempContent[1])
                    {
                        case "Skull":
                            tempEntities[i] = new EnemySkull(tempPosition);
                            break;

                        case "Sock":
                            tempEntities[i] = new EnemySock(tempPosition);
                            break;

                        case "Crab":
                            tempEntities[i] = new EnemyCrab(tempPosition);
                            break;

                        default:
                            break;
                    }
                }
            }

            return tempEntities;
        }

        public WorldObject[] GetPipes(Load.MapObject[] someMapObjects)
        {
            WorldObject[] tempPipes = new WorldObject[someMapObjects.Length];

            for (int i = 0; i < tempPipes.Length; ++i)
            {
                Load.MapObject tempObject = someMapObjects[i];

                tempPipes[i] = new Pipe(new Vector2(tempObject.x, tempObject.y), new Vector2(tempObject.x + tempObject.width, tempObject.y + tempObject.height));
            }

            return tempPipes;
        }

        public Square[] GetColliders(Load.MapObject[] someMapObjects)
        {
            Square[] tempSquares = new Square[someMapObjects.Length];

            Square.DoubleAll = true;
            for (int i = 0; i < tempSquares.Length; ++i)
            {
                Load.MapObject tempObject = someMapObjects[i];

                tempSquares[i] = new Square(tempObject.x, tempObject.y, tempObject.width, tempObject.height);
            }
            Square.DoubleAll = false;

            return tempSquares;
        }

        private LevelPickup GetItem(string[] someContent, Vector2 aPosition)
        {
            switch (someContent[1])
            {
                case "Key":
                    return new LevelItem(new Item("Key " + someContent[2], int.Parse(someContent[2]), ItemType.Key, Load.Get<Texture2D>("Key")), aPosition, 0.1f, new Point(8, 8));

                case "Weapon":
                    return new LevelAmmo(aPosition, int.Parse(someContent[2]), int.Parse(someContent[3]));

                default:
                    return new LevelItem(new Item("FAILED", 0, ItemType.Other, Load.Get<Texture2D>("Square"), Color.Red), aPosition);
            }
        }
    }
}
