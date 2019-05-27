using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DCOdyssey
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

        public bool AccessActive { get; set; } = true;

        public Vector2 AccessTopLeft { get; private set; }
        public Vector2 AccessBottomRight { get; private set; }
        public List<string> AccessTags { get; private set; }
        public object AccessOwner { get; set; }

        private List<HitDetector> myTouched, myLastTouched;
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

            myTouched = new List<HitDetector>();
            myLastTouched = new List<HitDetector>();
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

        public bool Touched(HitDetector aDetector, bool anExclusiveBool = false)
        {
            if (myTouched.Contains(aDetector))
            {
                return anExclusiveBool ? myTouched.Count == 1 : true;
            }

            return false;
        }

        public bool TouchedWorld(HitDetector aDetector, bool anExclusiveBool = false)
        {
            if (!aDetector.AccessTags.Contains("World"))
            {
                return false;
            }

            List<HitDetector> tempDetectors = myTouched.Where(o => o.AccessTags.Contains("World")).ToList();

            if (tempDetectors.Contains(aDetector))
            {
                return anExclusiveBool ? tempDetectors.Count == 1 : true;
            }

            return false;
        }

        static public void UpdateAll()
        {
            Dictionary<HitDetector, List<HitDetector>> tempCollisions = new Dictionary<HitDetector, List<HitDetector>>();
            
            for (int i = 0; i < allColliders.Count; ++i)
            {
                HitDetector tempA = allColliders[i];

                tempA.myLastTouched = tempA.myTouched;
                tempA.myTouched.Clear();

                if (!tempA.AccessActive)
                {
                    continue;
                }

                //if (tempA.AccessOwner != null && tempA.AccessOwner is WorldObject worldObjectA)
                //{
                //    worldObjectA.UpdateHitDetector();
                //}

                bool
                    tempACollider = (tempA.AccessOwner != null) ? (((WorldObject)tempA.AccessOwner).AccessWorldCollide ? true : false) : false,
                    tempAWorld = tempA.AccessTags.Contains("World");

                if (!tempACollider)
                {
                    continue;
                }

                tempCollisions[tempA] = new List<HitDetector>();

                for (int j = 0/*i + 1*/; j < allColliders.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    HitDetector tempB = allColliders[j];

                    if (!tempB.AccessActive)
                        continue;

                    //if (tempB.AccessOwner != null && tempB.AccessOwner is WorldObject worldObjectB)
                    //{
                    //    worldObjectB.UpdateHitDetector();
                    //}

                    bool
                        tempBCollider = (tempB.AccessOwner != null) ? (((WorldObject)tempB.AccessOwner).AccessWorldCollide ? true : false) : false,
                        tempBWorld = tempB.AccessTags.Contains("World");

                    if (i == 0)
                    {
                        tempCollisions[tempB] = new List<HitDetector>();
                    }

                    if (Overlapping(tempA, tempB) && !tempA.myTouched.Contains(tempB))
                    {
                        tempA.OnColliding?.Invoke(tempB);
                        tempB.OnColliding?.Invoke(tempA);

                        tempA.myTouched.Add(tempB);
                        tempB.myTouched.Add(tempA);

                        if (!lastFrameCollisions.ContainsKey(tempA) || !(lastFrameCollisions[tempA] != null && lastFrameCollisions[tempA].Contains(tempB)))
                        {
                            tempA.OnEnter?.Invoke(tempB);
                            tempB.OnEnter?.Invoke(tempA);
                        }

                        if (tempA.AccessOwner != null || tempB.AccessOwner != null)
                        {

                        }

                        if (tempACollider && tempBWorld)
                        {
                            Eject(tempA, tempB);
                        }

                        if (tempBCollider && tempAWorld)
                        {
                            Eject(tempB, tempA);
                        }

                        if (tempCollisions.ContainsKey(tempA))
                            tempCollisions[tempA].Add(tempB);

                        if (tempCollisions.ContainsKey(tempB))
                            tempCollisions[tempB].Add(tempA);
                    }
                }
            }

            lastFrameCollisions = tempCollisions;
        }

        static void Eject(HitDetector aCollider, HitDetector aWorldCollider)
        {
            float[] tempDistances =
            {
                 aCollider.AccessBottomRight.Y - aWorldCollider.AccessTopLeft.Y,
                 aCollider.AccessBottomRight.X - aWorldCollider.AccessTopLeft.X,
                 aWorldCollider.AccessBottomRight.Y - aCollider.AccessTopLeft.Y,
                 aWorldCollider.AccessBottomRight.X - aCollider.AccessTopLeft.X
            };

            Vector2[] tempDirections =
            {
                new Vector2(0, -1),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(1, 0)
            };

            int tempLowestIndex = 0;
            for (int i = 1; i < 4; ++i)
            {
                if (tempDistances[i] < tempDistances[tempLowestIndex])
                {
                    tempLowestIndex = i;
                }
            }

            ((WorldObject)aCollider.AccessOwner).Correct(tempDirections[tempLowestIndex] * tempDistances[tempLowestIndex]);
            ((WorldObject)aCollider.AccessOwner).UpdateHitDetector();
        }

        static bool Overlapping(HitDetector aHitDetector1, HitDetector aHitDetector2)
        {
            // Return false if they are further apart than the maximum for their colliders to save performance
            if ((aHitDetector2.myCenter - aHitDetector1.myCenter).Length() > aHitDetector1.myMaxDistance + aHitDetector2.myMaxDistance)
                return false;

            // Check if the colliders line up vertically
            bool tempVerticalLineup = !(aHitDetector1.AccessBottomRight.Y <= aHitDetector2.AccessTopLeft.Y || aHitDetector1.AccessTopLeft.Y >= aHitDetector2.AccessBottomRight.Y);

            if (!tempVerticalLineup)
                return false;

            // Check if the colliders line up horizontally, if so, then they are overlapping
            bool tempHorizontalLineup = !(aHitDetector1.AccessBottomRight.X <= aHitDetector2.AccessTopLeft.X || aHitDetector1.AccessTopLeft.X >= aHitDetector2.AccessBottomRight.X);

            return tempHorizontalLineup;
        }
    }
}
