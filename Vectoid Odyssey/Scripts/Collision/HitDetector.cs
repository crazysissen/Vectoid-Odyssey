using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace VectoidOdyssey
{
    enum TagListType
    {
        Whitelist, Blacklist
    }

    class HitDetector
    {
        private static List<HitDetector> allColliders = new List<HitDetector>();
        private static Dictionary<HitDetector, List<HitDetector>> lastFrameCollisions = new Dictionary<HitDetector, List<HitDetector>>();

        public event Action<HitDetector> OnEnter, OnColliding;

        public Vector2 AccessTopLeft { get; set; }
        public Vector2 AccessBottomRight { get; set; }

        public List<string> AccessTags { get; private set; }

        public object AccessOwner { get; private set; }

        public HitDetector(Vector2 aTopLeft, Vector2 aBottomRight) : this(aTopLeft, aBottomRight, null)
        { }

        public HitDetector(Vector2 aTopLeft, Vector2 aBottomRight, params string[] someTags)
        {
            AccessTopLeft = aTopLeft;
            AccessBottomRight = aBottomRight;

            AccessTags = new List<string>(someTags);

            allColliders.Add(this);
        }

        public void Destroy()
        {
            allColliders.Remove(this);
        }

        static public void UpdateAll()
        {
            Dictionary<HitDetector, List<HitDetector>> tempCollisions = new Dictionary<HitDetector, List<HitDetector>>();
            
            for (int i = 0; i < allColliders.Count; ++i)
            {
                for (int j = i + 1; j < allColliders.Count; ++j)
                {
                    HitDetector tempA = allColliders[i], tempB = allColliders[j];

                    if (Overlapping(tempA, tempB))
                    {
                        tempA.OnColliding.Invoke(tempB);
                        tempB.OnColliding.Invoke(tempA);

                        if (lastFrameCollisions.ContainsKey(tempA))
                        {
                            if (lastFrameCollisions[tempA] != null && !lastFrameCollisions[tempA].Contains(tempB))
                            {
                                tempA.OnEnter.Invoke(tempB);
                                tempB.OnEnter.Invoke(tempA);
                            }
                        }
                    }

                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            tempCollisions[tempA] = new List<HitDetector>();
                        }

                        tempCollisions[tempB] = new List<HitDetector>();
                    }

                    tempCollisions[tempA].Add(tempB);
                    tempCollisions[tempB].Add(tempA);
                }
            }
        }

        static bool Overlapping(HitDetector aHitDetector1, HitDetector aHitDetector2)
        {
            float
                tempTop = aHitDetector1.AccessTopLeft.Y,
                tempRight = aHitDetector1.AccessBottomRight.X,
                tempBottom = aHitDetector1.AccessBottomRight.Y,
                tempLeft = aHitDetector1.AccessTopLeft.X,

                tempThatTop = aHitDetector1.AccessTopLeft.Y,
                tempThatRight = aHitDetector1.AccessBottomRight.X,
                tempThatBottom = aHitDetector1.AccessBottomRight.Y,
                tempThatLeft = aHitDetector1.AccessTopLeft.X;

            return ((tempTop >= tempThatTop && tempTop <= tempThatBottom) || (tempBottom <= tempThatBottom && tempBottom >= tempThatTop)) &&
                    ((tempLeft >= tempThatLeft && tempLeft <= tempThatRight) || (tempRight <= tempThatRight && tempRight >= tempThatLeft)) ||
                    ((tempThatTop >= tempTop && tempThatTop <= tempBottom) || (tempThatBottom <= tempBottom && tempThatBottom >= tempTop)) &&
                    ((tempThatLeft >= tempLeft && tempThatLeft <= tempRight) || (tempThatRight <= tempRight && tempThatRight >= tempLeft));
        }
    }
}
