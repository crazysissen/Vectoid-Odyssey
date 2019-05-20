using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DCOdyssey
{
    class CompleteMap : Map
    {
        public HitDetector[] AccessColliders { get; set; }

        private Texture2D myTexture;
        private Vector2 mySpawnPosition;
        private WorldObject[] myWorldObjects;

        private Renderer.Sprite myRenderer;

        public CompleteMap(Texture2D aTexture, RoomBounds[] someRoomBounds, Square[] someColliders, WorldObject[] someObjects, Vector2 aSpawnPosition)
        {
            myTexture = aTexture;
            mySpawnPosition = aSpawnPosition;
            myWorldObjects = someObjects;
            myBounds.AddRange(someRoomBounds);

            HitDetector[] tempDetectors = new HitDetector[someColliders.Length];
            for (int i = 0; i < tempDetectors.Length; i++)
            {
                tempDetectors[i] = new HitDetector(someColliders[i].GetPosition, someColliders[i].GetBRPosition, "World", "BulletTarget");
            }

            foreach (WorldObject obj in myWorldObjects)
            {
                obj.AccessActive = false;
            }

            myRenderer = new Renderer.Sprite(new Layer(MainLayer.Background, -1), myTexture, Vector2.Zero, Vector2.One, Color.White, 0, Vector2.Zero /*new Vector2(aTexture.Width, aTexture.Height) * 0.5f*/);
        }

        public override Player SpawnPlayer(PlayerSetup aPlayerSetup, MenuManager aMenuManager)
            => new Player(mySpawnPosition, aMenuManager, aPlayerSetup);

        public override void ActivateEnemies()
        {
            foreach (WorldObject obj in myWorldObjects)
            {
                obj.AccessActive = true;
            }
        }
    }
}
