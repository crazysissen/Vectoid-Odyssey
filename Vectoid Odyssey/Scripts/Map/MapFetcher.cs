using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    class MapFetcher
    {
        public enum MapType
        {
            Sewer1
        }

        public Map GetNew(MapType aType, object[] var)
        {
            Func<object[], Map>[] tempMapSwitch =
            {
                GetNewSewer1
            };

            try
            {
                return tempMapSwitch[(int)aType].Invoke(var);
            }
            catch
            {
                Console.WriteLine("Could not get new map from enum.");
                return null;
            }
        }

        public Map GetNewSewer1(object[] var)
        {
            RoomBounds[] tempBounds =
            {
                new RoomBounds(4, 16, 16, 28, 16, 48, 28, 24),
                new RoomBounds(48, 28, 16, 28, 24, 52, 28, 12),
                new RoomBounds(52, 28, 24, 28, 12, 80, 28, 24),
                new RoomBounds(80, 28, 12, 28, 24, 84, 28, 4),
                new RoomBounds(84, 28, 24, 28, 4, 112, 4, 4)
            };

            Square.DoubleAll = true;
            Square[] tempWorldColliders =
            {
                new Square(64, 26, 4, 2),
                new Square(88, 27, 4, 1),
                new Square(104, 27, 4, 1),
                new Square(90, 25, 2, 2),
                new Square(104, 25, 2, 2),
                new Square(94, 22, 8, 2),
                new Square(84, 20, 4, 1),
                new Square(108, 20, 4, 1)
            };
            Square.DoubleAll = false;

            Enemy[] tempEnemies =
            {
                new EnemySock(new Vector2(66, 55)),
                new EnemySock(new Vector2(38, 55)),

                new EnemySkull(new Vector2(130, 34)),
                new EnemySkull(new Vector2(134, 34)),
                new EnemySock(new Vector2(126, 54)),
                new EnemySock(new Vector2(118, 54)),
                new EnemySock(new Vector2(138, 54)),
                new EnemySock(new Vector2(146, 54)),

                new EnemyCrab(new Vector2(200, 42)),
                new EnemyCrab(new Vector2(192, 42))
            };

            return new CompleteMap(Load.Get<Texture2D>("Sewer"), tempBounds, tempWorldColliders, tempEnemies, new Vector2(52, 52));
        }

        public Map GetNewSewer2(object[] var)
        {
            RoomBounds[] tempBounds =
            {
                new RoomBounds(104, 28, 24, 34, 20, 128, 28, 24), // Centre
                new RoomBounds(88, 50, 4, 28, 24, 104, 34, 20), // Corridor Left
                new RoomBounds(76, 50, 6, 50, 4, 88, 28, 24), // Left Room
                new RoomBounds(-8, 0, 0, 80, 39, 76, 50, 4), // Outside
                new RoomBounds(52, 3, 3, 10, 3, 76, 50, 4) // Top Left Room
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
                new Square(76, 8, 7, 1),
                new Square(76, 9, 4, 1),
                new Square(76, 10, 2, 2),
                // Bottom
                new Square(76, 30, 1, 9),
                new Square(77, 34, 2, 1),
                new Square(77, 36, 1, 1),
                new Square(77, 37, 2, 1),
                new Square(77, 38, 4, 1),
                new Square(84, 28, 4, 1),
                new Square(85, 39, 3, 1),
                new Square(80, 42, 8, 1),
                new Square(82, 43, 6, 1),
                new Square(76, 46, 3, 2),
                new Square(84, 49, 4, 1),

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
                new Square(60, 8, 16, 2),
                new Square(52, 3, 2, 1),
                new Square(52, 4, 1, 1),
                new Square(58, 3, 3, 1),
                new Square(59, 4, 2, 1),
                new Square(61, 4, 15, 2)
            };
            Square.DoubleAll = false;

            Enemy[] tempEnemies =
            {
                
            };

            return new CompleteMap(Load.Get<Texture2D>("Sewer2"), tempBounds, tempWorldColliders, tempEnemies, new Vector2(232, 62));
        }
    }
}
