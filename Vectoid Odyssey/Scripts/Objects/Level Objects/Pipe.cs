using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DCOdyssey
{
    class Pipe : LevelObject
    {
        const float
            VERTICALLEEWAY = 0.8f,
            CORRECTTIME = 0.05f,
            TRANSFERTIME = 0.2f,
            NULLTIME = 0.1f;

        private bool myHorizontal;
        private Renderer.Sprite myOpening1, myOpening2, mySegment;
        private Vector2 myTopLeft, myBottomRight;

        private HitDetector myHasCollided;

        private Renderer.SpriteScreen myScreenCover;
        private Player myActivePlayer;
        private Vector2[] myPositionTable;
        private TimerTable myTable;
        private bool myTransferring;

        public Pipe(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            myTopLeft = aTopLeft * 2;
            myBottomRight = aBottomRight * 2;

            Texture2D tempEndTexture = Load.Get<Texture2D>("Tube-Opening"),
                tempSegmentTexture = Load.Get<Texture2D>("Tube-Segment");

            myHorizontal = true;

            if (aBottomRight.X - aTopLeft.X == 2)
            {
                myHorizontal = false;
            }

            myOpening1 = new Renderer.Sprite(new Layer(MainLayer.Main, 10), tempEndTexture, aTopLeft * 2, Vector2.One, Color.White, myHorizontal ? 0 : (float)Math.PI * 0.5f, myHorizontal ? Vector2.Zero : new Vector2(0, 32));
            myOpening2 = new Renderer.Sprite(new Layer(MainLayer.Main, 10), tempEndTexture, aBottomRight * 2, Vector2.One, Color.White, myHorizontal ? 0 : (float)Math.PI * 0.5f, myHorizontal ? new Vector2(32, 32) : new Vector2(32, 0)) { AccessEffects = SpriteEffects.FlipHorizontally };
            mySegment = new Renderer.Sprite(new Layer(MainLayer.Main, 10), tempSegmentTexture, aTopLeft + aBottomRight, myHorizontal ? new Vector2((aBottomRight.X - aTopLeft.X - 4) * 0.5f, 1) : new Vector2((aBottomRight.Y - aTopLeft.Y - 4) * 0.5f, 1), Color.White, myHorizontal ? 0 : (float)Math.PI * 0.5f, new Vector2(16, 16));

            myScreenCover = new Renderer.SpriteScreen(new Layer(MainLayer.GUI, -50), Load.Get<Texture2D>("Square"), new Rectangle(Point.Zero, DCOdyssey.AccessResolution), Color.Black) { AccessActive = false };

            AddCollider(aTopLeft * 2, aBottomRight * 2, true);
            AccessHitDetector.OnColliding += Collide;
        }

        protected override void Update(float aDeltaTime)
        {
            base.Update(aDeltaTime);

            if (myTransferring)
            {
                UpdateTransfer(aDeltaTime);

                return;
            }

            if (myHasCollided != null)
            {
                CheckCollision(myHasCollided);
                myHasCollided = null;
            }
        }

        protected override void BeforeDestroy()
        {
            myOpening1.Destroy();
            myOpening2.Destroy();
            mySegment.Destroy();

            myOpening1 = null;
            myOpening2 = null;
            mySegment = null;

            myScreenCover.Destroy();
            myScreenCover = null;
        }

        private void UpdateTransfer(float aDeltaTime)
        {
            int tempState = myTable.Update(aDeltaTime);

            if (myTable.AccessComplete)
            {
                myTransferring = false;
                myScreenCover.AccessActive = false;
                myActivePlayer.AccessPosition = myPositionTable[4];
                myActivePlayer.DeactivatePipe();
            }

            float tempCurrentProgress = myTable.AccessCurrentStepProgress;

            switch (tempState)
            {
                case 0:
                    myActivePlayer.AccessPosition = Vector2.Lerp(myPositionTable[0], myPositionTable[1], tempCurrentProgress);
                    break;

                case 1:
                    myActivePlayer.AccessPosition = Vector2.Lerp(myPositionTable[1], myPositionTable[2], tempCurrentProgress);
                    myScreenCover.AccessColor = new Color(0f, 0f, 0f, tempCurrentProgress);
                    break;

                case 2:
                    myScreenCover.AccessColor = new Color(0f, 0f, 0f, 1f);
                    break;

                case 3:
                    myActivePlayer.AccessPosition = Vector2.Lerp(myPositionTable[3], myPositionTable[4], tempCurrentProgress);
                    myScreenCover.AccessColor = new Color(0f, 0f, 0f, 1 - tempCurrentProgress);
                    break;
            }
        }

        private void Collide(HitDetector aHitDetector)
        {
            myHasCollided = aHitDetector;
        }

        private void CheckCollision(HitDetector aHitDetector)
        {
            if (aHitDetector.AccessOwner != null && aHitDetector.AccessOwner is Player player)
            {
                if (aHitDetector.TouchedWorld(AccessHitDetector, false))
                {
                    if (myHorizontal)
                    {
                        if (aHitDetector.AccessTopLeft.Y >= myTopLeft.Y && aHitDetector.AccessBottomRight.Y <= myBottomRight.Y)
                        {
                            if (player.AccessPosition.X < myTopLeft.X)
                            {
                                player.PipeViable(this, new Vector2(1, 0));
                            }
                            else if (player.AccessPosition.X > myBottomRight.X)
                            {
                                player.PipeViable(this, new Vector2(-1, 0));
                            }
                        }

                        return;
                    }
                    else
                    {
                        if (aHitDetector.AccessTopLeft.X + VERTICALLEEWAY >= myTopLeft.X && aHitDetector.AccessBottomRight.X - VERTICALLEEWAY <= myBottomRight.X)
                        {
                            if (player.AccessPosition.Y < myTopLeft.Y)
                            {
                                player.PipeViable(this, new Vector2(0, 1));
                            }
                            else if (player.AccessPosition.Y > myBottomRight.Y)
                            {
                                player.PipeViable(this, new Vector2(0, -1));
                            }
                        }

                        return;
                    }
                }
            }
        }

        public void StartTransfer(Player aPlayer, Vector2 aDirection)
        {
            Sound.PlayEffect("Transfer");

            myTable = new TimerTable(new float[] { myHorizontal ? 0 : CORRECTTIME, TRANSFERTIME, NULLTIME, TRANSFERTIME }, 0);
            myPositionTable = new Vector2[]
            {
                aPlayer.AccessPosition,

                myHorizontal ? 
                    (aDirection == new Vector2(1, 0) ? myTopLeft + new Vector2(-2, 2) : myBottomRight + new Vector2(2, -2)) :
                    (aDirection == new Vector2(0, 1) ? myTopLeft + new Vector2(2, -2) : myBottomRight + new Vector2(-2, 2)),

                aDirection == new Vector2(1, 0) || aDirection == new Vector2(0, 1) ? myTopLeft + new Vector2(2, 2) : myBottomRight + new Vector2(-2, -2),

                aDirection == new Vector2(1, 0) || aDirection == new Vector2(0, 1) ? myBottomRight + new Vector2(-2, -2) : myTopLeft + new Vector2(2, 2),

                myHorizontal ?
                    (aDirection == new Vector2(1, 0) ? myBottomRight + new Vector2(2, -2) : myTopLeft + new Vector2(-2, 2)) :
                    (aDirection == new Vector2(0, 1) ? myBottomRight + new Vector2(-2, 2) : myTopLeft + new Vector2(2, -2))
            };

            myActivePlayer = aPlayer;
            myTransferring = true;

            myScreenCover.AccessActive = true;
            myScreenCover.AccessColor = new Color(0, 0, 0, 0);
        }
    }
}
