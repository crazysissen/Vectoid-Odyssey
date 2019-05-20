using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DCOdyssey
{
    public enum Control
    {
        Up, Left, Down, Right, Action1, Action2, Menu1, Menu2
    }

    static class Input
    {
        // Example:
        // Up, Left, Down, Right, A, B, Start, Select
        // W, A, S, D, LeftMouse, Space, Escape, Tab

        public enum ControlScheme
        {
            Standard/*, Controller*/
        }

        public static KeyboardState GetKeyboardState => myKState;
        public static MouseState GetMouseState => myMState;
        public static GamePadState GetGamePadState => myGState;

        public static Point GetMousePosition => myMState.Position;
        public static bool GetLeftMouse => myMState.LeftButton == ButtonState.Pressed;
        public static bool GetRightMouse => myMState.RightButton == ButtonState.Pressed;
        public static bool GetLeftMouseDown => myMState.LeftButton == ButtonState.Pressed && myLastMState.LeftButton == ButtonState.Released;
        public static bool GetRightMouseDown => myMState.RightButton == ButtonState.Pressed && myLastMState.RightButton == ButtonState.Released;
        public static int GetScrollWheelChange => myScrollWheelState == myLastScrollWheelState ? 0 : (myScrollWheelState > myLastScrollWheelState ? 1 : -1);

        private static KeyboardState myKState, myLastKState;
        private static MouseState myMState, myLastMState;
        private static GamePadState myGState, myLastGState;

        private static bool[] myActiveControls, myLastActiveControls;
        private static ControlScheme myScheme;
        private static int myScrollWheelState, myLastScrollWheelState;

        public static void Init()
        {
            myKState = new KeyboardState();
            myMState = new MouseState();
            myGState = new GamePadState();

            myLastKState = myKState;
            myLastMState = myMState;
            myLastGState = myGState;

            myActiveControls = new bool[8];
            myLastActiveControls = new bool[8];

            myScrollWheelState = myMState.ScrollWheelValue;
        }

        /// <summary>
        /// Called at the start of every frame
        /// </summary>
        public static void Update()
        {
            myLastKState = myKState;
            myLastMState = myMState;
            myLastGState = myGState;

            myLastScrollWheelState = myScrollWheelState;

            myKState = Keyboard.GetState();
            myMState = Mouse.GetState();
            myGState = GamePad.GetState(1);

            myScrollWheelState = myMState.ScrollWheelValue;

            myLastActiveControls = myActiveControls;
            UpdateControls();
        }

        public static bool Pressed(Keys aKey)
            => myKState.IsKeyDown(aKey);

        public static bool Pressed(Control aControl)
            => myActiveControls[(int)aControl];

        public static bool Down(Keys aKey)
            => myKState.IsKeyDown(aKey) && myLastKState.IsKeyUp(aKey);

        public static bool Down(Control aControl)
            => myActiveControls[(int)aControl] && !myLastActiveControls[(int)aControl];

        public static void SetControls(ControlScheme aControlScheme)
        {
            myScheme = aControlScheme;

            UpdateControls();
        }

        private static void UpdateControls()
        {
            switch (myScheme)
            {
                case ControlScheme.Standard:

                    myActiveControls = new bool[]
                    {
                        Pressed(Keys.W) || myGState.DPad.Up == ButtonState.Pressed,
                        Pressed(Keys.A) || myGState.DPad.Left == ButtonState.Pressed,
                        Pressed(Keys.S) || myGState.DPad.Down == ButtonState.Pressed,
                        Pressed(Keys.D) || myGState.DPad.Right == ButtonState.Pressed,
                        GetLeftMouse || myGState.Buttons.A == ButtonState.Pressed,
                        Pressed(Keys.Space) || myGState.Buttons.X == ButtonState.Pressed,
                        Pressed(Keys.Escape) || myGState.Buttons.Start == ButtonState.Pressed,
                        Pressed(Keys.Tab) || myGState.Buttons.Back == ButtonState.Pressed
                    };

                    break;

                default:
                    break;
            }
        }
    }
}
