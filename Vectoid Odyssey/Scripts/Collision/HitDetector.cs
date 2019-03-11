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

        public Vector2 AccessTopLeft { get; private set; }
        public Vector2 AccessBottomRight { get; private set; }
        public List<string> AccessTags { get; private set; }
        public object AccessOwner { get; set; }

        private Vector2 myCenter;
        private float myMaxDistance;

        public HitDetector(Vector2 aTopLeft, Vector2 aBottomRight) : this(aTopLeft, aBottomRight, null)
        { }

        public HitDetector(Vector2 aTopLeft, Vector2 aBottomRight, params string[] someTags)
        {
            Set(aTopLeft, aBottomRight);

            AccessTags = new List<string>();

            if (someTags != null)
            {
                AccessTags.AddRange(someTags);
            }

            allColliders.Add(this);
        }

        public void Set(Vector2 aTopLeft, Vector2 aBottomRight)
        {
            AccessTopLeft = aTopLeft;
            AccessBottomRight = aBottomRight;

            myCenter = (aTopLeft + aBottomRight) * 0.5f;
            myMaxDistance = (myCenter - aTopLeft).Length();
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

                    if (i == 0)
                    {
                        if (j == i + 1)
                        {
                            tempCollisions[tempA] = new List<HitDetector>();
                        }

                        tempCollisions[tempB] = new List<HitDetector>();
                    }

                    if (Overlapping(tempA, tempB))
                    {
                        tempA.OnColliding?.Invoke(tempB);
                        tempB.OnColliding?.Invoke(tempA);

                        if (!lastFrameCollisions.ContainsKey(tempA) || !(lastFrameCollisions[tempA] != null && lastFrameCollisions[tempA].Contains(tempB)))
                        {
                            tempA.OnEnter?.Invoke(tempB);
                            tempB.OnEnter?.Invoke(tempA);
                        }

                        tempCollisions[tempA].Add(tempB);
                        tempCollisions[tempB].Add(tempA);
                    }
                }
            }

            lastFrameCollisions = tempCollisions;
        }

        static bool Overlapping(HitDetector aHitDetector1, HitDetector aHitDetector2)
        {
            // Return false if they are further apart than the maximum for their colliders to save performance
            if ((aHitDetector2.myCenter - aHitDetector1.myCenter).Length() > aHitDetector1.myMaxDistance + aHitDetector2.myMaxDistance)
                return false;

            // Check if the colliders line up vertically
            bool tempVerticalLineup = !(aHitDetector1.AccessBottomRight.Y < aHitDetector2.AccessTopLeft.Y || aHitDetector1.AccessTopLeft.Y > aHitDetector2.AccessBottomRight.Y);

            if (!tempVerticalLineup)
                return false;

            // Check if the colliders line up horizontally, if so, then they are overlapping
            bool tempHorizontalLineup = !(aHitDetector1.AccessBottomRight.X < aHitDetector2.AccessTopLeft.X || aHitDetector1.AccessTopLeft.X > aHitDetector2.AccessBottomRight.X);

            return tempHorizontalLineup;
        }
    }
}
