using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VectoidOdyssey
{
    partial class GUI : GUIContainer
    {
        const MainLayer
            LAYER = MainLayer.GUI;

        public void Init()
        {
            AccessMembers = new List<IGUIMember>();
        }

        public static IGUIMember[] GetMembers(GUIContainer aContainer, MouseState aMouseState, KeyboardState aKeyboardState, float aDeltaTime, Point anAdditiveOrigin, bool aDrawThisBool = false)
        {
            // Recursive method to retrieve all 

            List<IGUIMember> myNewMembers = new List<IGUIMember>();

            foreach (IGUIMember member in aContainer.AccessMembers)
            {
                if (member is GUIContainer)
                {
                    if (!(member as GUIContainer).AccessActive)
                    {
                        continue;
                    }

                    myNewMembers.AddRange(GetMembers((member as GUIContainer), aMouseState, aKeyboardState, aDeltaTime, anAdditiveOrigin + (member as GUIContainer).AccessOrigin));
                }

                if (member != null)
                {
                    member.AccessOrigin = anAdditiveOrigin;
                    myNewMembers.Add(member);
                }
            }

            return myNewMembers.ToArray();
        }

        public static GUI operator +(GUI gui, IGUIMember member)
        {
            gui.Add(member);
            return gui;
        }

        public static GUI operator +(GUI gui, IGUIMember[] members)
        {
            gui.Add(members);
            return gui;
        }

        public static GUI operator -(GUI gui, IGUIMember member)
        {
            gui.Remove(member);
            return gui;
        }
    }

    public abstract class GUIContainer
    {
        public bool AccessActive { get; set; } = true;
        public virtual Point AccessOrigin { get; set; } = new Point(0, 0);
        public virtual List<IGUIMember> AccessMembers { get; protected set; } = new List<IGUIMember>();

        public virtual void Add(params IGUIMember[] members)
        {
            foreach (IGUIMember member in members)
            {
                AccessMembers.Add(member);

                if (member is Renderer)
                {
                    (member as Renderer).AccessAutomaticDraw = false;
                }

                if (member is GUI.Button && (member as GUI.Button).Text != null)
                {
                    Add((member as GUI.Button).Text);
                }
            }
        }

        public virtual void Remove(IGUIMember member)
        {
            if (AccessMembers.Contains(member))
            {
                AccessMembers.Remove(member);
                AccessMembers.TrimExcess();
            }
        }

        public static GUIContainer operator +(GUIContainer container, IGUIMember member)
        {
            container.Add(member);
            return container;
        }

        public static GUIContainer operator +(GUIContainer container, IGUIMember[] members)
        {
            container.Add(members);
            return container;
        }

        public static GUIContainer operator -(GUIContainer container, IGUIMember member)
        {
            container.Remove(member);
            return container;
        }
    }

    public interface IGUIMember
    {
        Layer AccessLayer { get; }

        void Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float deltaTime);

        Point AccessOrigin { get; set; }
    }
}
