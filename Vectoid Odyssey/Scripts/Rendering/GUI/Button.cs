﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DCOdyssey
{
    class Button : IGUIMember
    {
        Layer IGUIMember.AccessLayer => AccessLayer;
        Point IGUIMember.AccessOrigin { get => myOrigin; set => myOrigin = value; }

        private Point myOrigin = new Point();

        const float
            DEFAULTTRANSITIONTIME = 0.05f;

        public enum State { Idle, Hovered, Pressed }
        public enum Type { ColorSwitch, TextureSwitch, AnimatedSwitch }
        public enum Transition { Custom, Switch, LinearFade, DecelleratingFade, AcceleratingFade, EaseOutBack, EaseOutElastic, EaseOutCubic }

        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnMouseDown;
        public event Action OnClick;

        public State AccessCurrentState { get; private set; }
        public Type AccessDisplayType { get; private set; }
        public Transition AccessTransitionType { get; set; }
        public float AccessTransitionTime { get; set; }
        public Rectangle AccessTransform { get; set; }
        public Renderer.Text AccessText { get; set; }
        public Texture2D AccessTexture { get; set; }
        public Texture2D[] AccessTextureSwitch { get; private set; }
        public Color[] AccessColorSwitch { get; private set; }
        public SpriteEffects AccessSpriteEffects { get; set; }

        public bool AccessScaleEffect { get; set; } = true;
        public float AccessScaleEffectAmplitude { get; set; } = 1.0f;

        public Layer AccessLayer { get; set; }

        private Func<float, float> myTransition;
        private Color myTextBaseColor;
        private float myCurrentTime, myTargetTime, myTimeMultiplier, myStartScale, myTargetScale, myCurrentScale = 1;
        private bool myInTransition, myBeginHoldOnButton, myPressedLastFrame;
        private Color myStartColor, myTargetColor;
        private Texture2D myStartTexture, myTargetTexture;
        private State myStartState;

        private float[] myScaleSwitch = { 1.0f, 1.02f, 0.985f };

        /// <summary>Testing button</summary>
        public Button(Layer aLayer, Rectangle aTransform, Color aColor)
            : this(aLayer, aTransform, Load.Get<Texture2D>("Square"), aColor)
        { }

        /// <summary>Simple button with preset color multipliers, for testing primarily</summary>
        public Button(Layer aLayer, Rectangle aTransform, Texture2D aTexture)
            : this(aLayer, aTransform, aTexture, new Color(0.9f, 0.9f, 0.9f))
        { }

        /// <summary>Simple button, for testing primarily</summary>
        public Button(Layer aLayer, Rectangle aTransform, Texture2D aTexture, Color aColor)
            : this(aLayer, aTransform, aTexture, PseudoDefaultColors(aColor), Transition.LinearFade, DEFAULTTRANSITIONTIME)
        { }

        /// <summary>Simple button that changes color (color multiplier) when hovered/clicked according to a set transition type and time</summary>
        /// <param name="aColorSwitch">Color array in order [idle, hover, click]</param>
        public Button(Layer aLayer, Rectangle aTransform, Texture2D aTexture, Color[] aColorSwitch, Transition aTransitionType, float aTransitionTime)
        {
            AccessDisplayType = Type.ColorSwitch;

            AccessLayer = aLayer;

            AccessTransform = aTransform;
            AccessTexture = aTexture;
            AccessColorSwitch = aColorSwitch;
            AccessTransitionType = aTransitionType;
            AccessTransitionTime = aTransitionTime;

            SetTransitionType(aTransitionType);
        }

        /// <summary>Button that switches texture when hovered/clicked</summary>
        public Button(Layer aLayer, Rectangle aTransform, Texture2D anIdleTexture, Texture2D aHoverTexture, Texture2D aClickTexture)
            : this(aLayer, aTransform, anIdleTexture, aHoverTexture, aClickTexture, Transition.LinearFade, DEFAULTTRANSITIONTIME)
        { }

        /// <summary>Button that changes texture when hovered/clicked according to a set transition type and time</summary>
        public Button(Layer aLayer, Rectangle aTransform, Texture2D anIdleTexture, Texture2D aHoverTexture, Texture2D aClickTexture, Transition aTransitionType, float aTransitionTime)
        {
            AccessDisplayType = Type.TextureSwitch;

            AccessLayer = aLayer;

            AccessTransform = aTransform;
            AccessTextureSwitch = new Texture2D[] { anIdleTexture, aHoverTexture, aClickTexture };
            AccessTransitionType = aTransitionType;
            AccessTransitionTime = aTransitionTime;

            SetTransitionType(aTransitionType);
        }

        void IGUIMember.Draw(SpriteBatch spriteBatch, MouseState mouse, KeyboardState keyboard, float unscaledDeltaTime)
        {
            bool onButton = RendererFocus.OnArea(new Rectangle(AccessTransform.Location + myOrigin, AccessTransform.Size), AccessLayer);
            bool pressed = mouse.LeftButton == ButtonState.Pressed;

            Transfer(pressed, onButton);

            List<TAS> textures = new List<TAS>();
            Color color = Color.White;
            float scaledValue = myTransition.Invoke(myCurrentTime);

            if (myCurrentTime >= 1)
            {
                myInTransition = false;
                myCurrentTime = 0;
                myStartState = AccessCurrentState;
            }

            if (myInTransition)
            {
                myCurrentTime += unscaledDeltaTime / AccessTransitionTime;

                if (AccessScaleEffect)
                {
                    myCurrentScale = scaledValue.Lerp(myStartScale, myTargetScale);
                }
                else
                {
                    myCurrentScale = 1;
                }

                switch (AccessDisplayType)
                {
                    case Type.ColorSwitch:
                        textures.Add(new TAS(AccessTexture, 1));
                        color = Color.Lerp(myStartColor, myTargetColor, scaledValue);
                        break;

                    case Type.TextureSwitch:
                        textures.Add(new TAS(myStartTexture, 1));
                        textures.Add(new TAS(myTargetTexture, 1 - scaledValue));
                        color = Color.White;
                        break;

                        // TODO: Implement animated button
                }
            }

            if (!myInTransition)
            {
                if (AccessScaleEffect)
                {
                    myCurrentScale = myScaleSwitch[(int)AccessCurrentState];
                }

                switch (AccessDisplayType)
                {
                    case Type.ColorSwitch:
                        textures.Add(new TAS(AccessTexture, 255));
                        color = AccessColorSwitch[(int)AccessCurrentState];
                        break;

                    case Type.TextureSwitch:
                        textures.Add(new TAS(AccessTextureSwitch[(int)AccessCurrentState], 255));
                        color = Color.White;
                        break;
                }
            }

            Vector2
                halfSize = new Vector2(AccessTransform.Size.X * 0.5f, AccessTransform.Size.Y * 0.5f),
                middlePosition = AccessTransform.Location.ToVector2() + halfSize;

            Rectangle targetRectangle = new Rectangle(myOrigin + (middlePosition - halfSize * myCurrentScale).RoundToPoint(), (AccessTransform.Size.ToVector2() * myCurrentScale).RoundToPoint());

            for (int i = 0; i < textures.Count; ++i)
            {
                spriteBatch.Draw(textures[i].texture, targetRectangle, null, new Color(color, textures[i].alpha), 0, new Vector2(0.5f, 0.5f), AccessSpriteEffects, AccessLayer.GetDepth);
            }

            if (!pressed)
            {
                myBeginHoldOnButton = false;
            }

            myPressedLastFrame = pressed;
        }

        private void Transfer(bool pressed, bool onButton)
        {
            if (pressed && !myPressedLastFrame && onButton)
            {
                myBeginHoldOnButton = true;
            }

            if (AccessCurrentState != State.Idle && !onButton)
            {
                OnExit?.Invoke();
                ChangeState(State.Idle);
            }
            else
                switch (AccessCurrentState)
                {
                    case State.Idle:
                        if (onButton)
                        {
                            OnEnter?.Invoke();
                            if (pressed)
                            {
                                ChangeState(State.Pressed);
                                if (!myPressedLastFrame)
                                    OnMouseDown?.Invoke();
                            }
                            if (!pressed)
                                ChangeState(State.Hovered);
                        }
                        break;

                    case State.Hovered:
                        if (onButton && pressed && myBeginHoldOnButton)
                        {
                            OnMouseDown?.Invoke();
                            ChangeState(State.Pressed);
                        }
                        break;

                    case State.Pressed:
                        if (!pressed && onButton)
                        {
                            ChangeState(State.Hovered);
                            if (myBeginHoldOnButton)
                            {
                                OnClick?.Invoke();
                            }
                        }
                        break;
                }
        }

        private void ChangeState(State state)
        {
            State previousStartState = myStartState;

            myStartState = AccessCurrentState;
            AccessCurrentState = state;

            myInTransition = true;
            myTargetTime = (myInTransition && state == previousStartState) ? myTargetTime - myCurrentTime : 1;
            myCurrentTime = 0;

            myStartScale = (myScaleSwitch[(int)myStartState] - 1) * AccessScaleEffectAmplitude + 1;
            myTargetScale = (myScaleSwitch[(int)state] - 1) * AccessScaleEffectAmplitude + 1;

            switch (AccessDisplayType)
            {
                case Type.ColorSwitch:
                    myStartColor = AccessColorSwitch[(int)myStartState];
                    myTargetColor = AccessColorSwitch[(int)state];
                    break;

                case Type.TextureSwitch:
                    myStartTexture = AccessTextureSwitch[(int)myStartState];
                    myTargetTexture = AccessTextureSwitch[(int)state];
                    break;

                case Type.AnimatedSwitch:
                    break;
            }
        }

        public void SetTransitionType(Transition type)
        {
            if (type != Transition.Custom)
            {
                AccessTransitionType = type;
            }

            switch (type)
            {
                case Transition.Switch:
                    myTransition = o => (float)Math.Ceiling(o);
                    return;

                case Transition.LinearFade:
                    myTransition = o => o;
                    return;

                case Transition.DecelleratingFade:
                    myTransition = MathV.SineD;
                    return;

                case Transition.AcceleratingFade:
                    myTransition = MathV.SineA;
                    return;

                case Transition.EaseOutBack:
                    myTransition = Easing.EaseOutBack;
                    return;

                case Transition.EaseOutElastic:
                    myTransition = Easing.EaseOutElastic;
                    return;

                case Transition.EaseOutCubic:
                    myTransition = Easing.EaseOutCubic;
                    return;
            }
        }

        public void AddText(string text, float fontSize, bool centered, Color baseColor, SpriteFont font)
        {
            Vector2 measure = font.MeasureString(text);

            AccessText = new Renderer.Text(
                new Layer(AccessLayer.AccessMainLayer, AccessLayer.AccessSubLayer + 1), font, new StringBuilder(text), Vector2.One * fontSize, 0,
                centered ? new Vector2((AccessTransform.Left + AccessTransform.Right) * 0.5f, (AccessTransform.Top + AccessTransform.Bottom) * 0.5f) : new Vector2(AccessTransform.Left + 8, (AccessTransform.Top + AccessTransform.Bottom) * 0.5f),
                centered ? new Vector2(0.5f, 0.5f) * measure : new Vector2(0, 0.5f) * measure,
                baseColor, SpriteEffects.None);

            myTextBaseColor = baseColor;
        }

        public static Color[] DefaultColors()
            => new Color[] { Color.White, new Color(1.15f, 1.15f, 1.15f), new Color(0.85f, 0.85f, 0.85f) };

        public static Color[] PseudoDefaultColors(Color origin)
            => new Color[] { origin, origin * 1.15f, origin * 0.85f };

        public void SetPseudoDefaultColors(Color origin)
            => AccessColorSwitch = PseudoDefaultColors(origin);

        public void SetDefaultColors()
            => AccessColorSwitch = DefaultColors();

        public void SetTransitionExplicit(Func<float, float> function)
            => myTransition = function;

        public void SetTextureSwitch(Texture2D idle, Texture2D hover, Texture2D click)
            => AccessTextureSwitch = new Texture2D[] { idle, hover, click };

        public void SetColorSwitch(Color idle, Color hover, Color click)
            => AccessColorSwitch = new Color[] { idle, hover, click };

        public void SetColorSwitch(Color[] colors)
            => AccessColorSwitch = colors.Length == 3 ? colors : AccessColorSwitch;

        struct TAS
        {
            public Texture2D texture;
            public float alpha;

            public TAS(Texture2D texture, float alpha)
            {
                this.texture = texture;
                this.alpha = alpha;
            }
        }
    }
}

