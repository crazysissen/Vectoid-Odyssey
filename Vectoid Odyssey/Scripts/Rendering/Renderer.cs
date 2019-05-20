using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DCOdyssey
{
    abstract class Renderer
    {
        public const float
            DEGTORAD = (2 * (float)Math.PI) / 360,
            FONTSIZEMULTIPLIER = 1.0f / 4;

        /// <summary>Whether or not the object should be drawn automatically</summary>
        public virtual bool AccessActive { get; set; } = true;
        public virtual bool AccessAutomaticDraw { get; set; } = true;
        /// <summary>A Layer class to represent what depth it should be drawn at</summary>
        public virtual Layer AccessLayer { get; set; }

        public abstract void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime);

        public Renderer()
        {
            RendererController.AddRenderer(this);
        }

        public void Destroy()
        {
            RendererController.RemoveRenderer(this);
        }

        public class Sprite : Renderer
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D AccessTexture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Vector2 AccessPosition { get; set; }

            /// <summary>The width/height of the object</summary>
            public virtual Vector2 AccessSize { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float AccessRotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the sprite is rotated
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 AccessOrigin { get; set; }

            /// <summary>What part of the sprite to render. Useful for custom animations.</summary>
            public virtual Rectangle? AccessSourceRectangle { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color AccessColor { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects AccessEffects { get; set; }

            public Sprite(Layer aLayer, Texture2D aTexture, Vector2 aPosition, Vector2 aSize, Color aColor, float aRotation, Vector2 aRotationOrigin)
            {
                AccessLayer = aLayer;
                AccessTexture = aTexture;
                AccessPosition = aPosition;
                AccessSize = aSize;
                AccessRotation = aRotation;
                AccessOrigin = aRotationOrigin;
                AccessColor = aColor;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                Vector2 tempPosition = aCamera.WorldToScreenPosition(AccessPosition);
                aSpriteBatch.Draw(AccessTexture, tempPosition, AccessSourceRectangle, AccessColor, AccessRotation, AccessOrigin, aCamera.WorldToScreenSize(AccessSize), AccessEffects, AccessLayer.GetDepth);
            }

            public Rectangle GetArea()
            {
                Vector2
                    tempPosition = RendererController.AccessCamera.WorldToScreenPosition(AccessPosition),
                    tempTexture = new Vector2(AccessTexture.Width, AccessTexture.Height),
                    tempScale = RendererController.AccessCamera.WorldToScreenSize(AccessSize) * tempTexture;

                Point tempTopLeft = (tempPosition - (AccessOrigin / tempTexture) * tempScale).RoundToPoint();

                return new Rectangle(tempTopLeft, tempScale.RoundToPoint());
            }
        }

        public class SpriteScreen : RendererIGUI
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D AccessTexture { get; set; }

            /// <summary>The x & y coordinates of the object in world spacesummary>
            public virtual Rectangle AccessTransform { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float AccessRotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 AccessOrigin { get; set; }

            /// <summary>What part of the sprite to render. Useful for custom animations.</summary>
            public virtual Rectangle? AccessSourceRectangle { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color AccessColor { get; set; }

            /// <summary> Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects AccessEffects { get; set; }

            public SpriteScreen(Layer aLayer, Texture2D aTexture, Rectangle aTransform, Color aColor) : this(aLayer, aTexture, aTransform, aColor, 0, Vector2.Zero, SpriteEffects.None) { }

            public SpriteScreen(Layer aLayer, Texture2D aTexture, Rectangle aTransform, Color aColor, float aRotation, Vector2 aRotationOrigin, SpriteEffects someEffects)
            {
                AccessLayer = aLayer;
                AccessTexture = aTexture;
                AccessTransform = aTransform;
                AccessRotation = aRotation;
                AccessOrigin = aRotationOrigin;
                AccessColor = aColor;
                AccessEffects = someEffects;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                aSpriteBatch.Draw(AccessTexture, new Rectangle(AccessTransform.Location + AccessOffset, AccessTransform.Size), AccessSourceRectangle, AccessColor, AccessRotation, AccessOrigin, AccessEffects, AccessLayer.GetDepth);
            }
        }

        public class SpriteScreenFloating : RendererIGUI
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D AccessTexture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Vector2 AccessPosition { get; set; }

            /// <summary>The width/height of the object</summary>
            public virtual Vector2 AccessSize { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float AccessRotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the sprite is rotated
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 AccessOrigin { get; set; }

            /// <summary>What part of the sprite to render. Useful for custom animations.</summary>
            public virtual Rectangle? AccessSourceRectangle { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color AccessColor { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects AccessEffects { get; set; }

            public SpriteScreenFloating(Layer aLayer, Texture2D aTexture, Vector2 aPosition, Vector2 aSize, Color aColor, float aRotation, Vector2 anOrigin, SpriteEffects someEffects)
            {
                AccessLayer = aLayer;
                AccessTexture = aTexture;
                AccessPosition = aPosition;
                AccessSize = aSize;
                AccessRotation = aRotation;
                AccessOrigin = anOrigin;
                AccessColor = aColor;
                AccessEffects = someEffects;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                Vector2 tempPosition = AccessPosition;
                aSpriteBatch.Draw(AccessTexture, tempPosition + AccessOffset.ToVector2(), AccessSourceRectangle, AccessColor, AccessRotation, AccessOrigin, AccessSize, AccessEffects, AccessLayer.GetDepth);
            }
        }

        public class Animator : Renderer
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D AccessTexture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Vector2 AccessPosition { get; set; }

            /// <summary>The x & y size multiplier of the object</summary>
            public virtual Vector2 AccessSize { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float AccessRotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 AccessOrigin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color AccessColor { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects AccessEffects { get; set; }

            public Point AccessFrameDimensions { get; private set; }
            public Point AccessCountDimensions { get; private set; }
            public float AccessTime { get; set; }
            public float AccessTimeInterval { get; set; }
            public bool AccessRepeat { get; set; }
            public int AccessCurrentFrame => (int)(AccessTime / AccessTimeInterval);
            public int AccessFrameCount { get; private set; }
            public bool AccesComplete { get; private set; }

            public Animator(Layer aLayer, Texture2D aSheet, Point someFrameDimensions, Vector2 aPosition, Vector2 aSize, Vector2 anOrigin, float aRotation, Color aColor, float anInterval, float aStartTime, bool aRepeat, SpriteEffects someSpriteEffects)
            {
                if (aSheet.Width % someFrameDimensions.X != 0 || aSheet.Height % someFrameDimensions.Y != 0)
                {
                    throw new Exception("Tried to create AnimatorScreen where the format image was not proportional to the frame size.");
                }

                AccessLayer = aLayer;

                AccessFrameDimensions = someFrameDimensions;
                AccessTime = aStartTime;
                AccessTimeInterval = anInterval;
                AccessRepeat = aRepeat;
                AccessCountDimensions = new Point(aSheet.Width / someFrameDimensions.X, aSheet.Height / someFrameDimensions.Y);
                AccessFrameCount = AccessCountDimensions.X * AccessCountDimensions.Y;

                AccessTexture = aSheet;
                AccessPosition = aPosition;
                AccessSize = aSize;
                AccessRotation = aRotation;
                AccessOrigin = anOrigin;
                AccessColor = aColor;
                AccessEffects = someSpriteEffects;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                AccessTime += aDeltaTime;
                if (AccessTime > AccessFrameCount * AccessTimeInterval)
                {
                    if (AccessRepeat)
                    {
                        AccessTime %= (AccessFrameCount * AccessTimeInterval);
                    }
                    else
                    {
                        AccesComplete = true;
                        return;
                    }
                }

                int tempCurrentFrame = AccessCurrentFrame;
                Rectangle tempDestinationRectangle = new Rectangle()
                {
                    X = AccessFrameDimensions.X * (tempCurrentFrame % AccessCountDimensions.X),
                    Y = AccessFrameDimensions.Y * (int)((float)tempCurrentFrame / AccessCountDimensions.X),
                    Width = AccessFrameDimensions.X,
                    Height = AccessFrameDimensions.Y
                };

                aSpriteBatch.Draw(AccessTexture, aCamera.WorldToScreenPosition(AccessPosition), tempDestinationRectangle, AccessColor, AccessRotation, AccessOrigin, aCamera.WorldToScreenSize(AccessSize), AccessEffects, AccessLayer.GetDepth);
            }
        }

        public class AnimatorScreen : RendererIGUI
        {
            /// <summary>The texture of the object</summary>
            public virtual Texture2D AccessTexture { get; set; }

            /// <summary>The x & y coordinates of the object in world space</summary>
            public virtual Rectangle AccessTransform { get; set; }

            /// <summary>The rotation angle of the object measured in degrees (0-360)</summary>
            public virtual float AccessRotation { get; set; }

            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public virtual Vector2 AccessOrigin { get; set; }

            /// <summary>The color multiplier of the object</summary>
            public virtual Color AccessColor { get; set; }

            /// <summary>Wether or not the sprite is flipped somehow, stack using binary OR operator (|)</summary>
            public virtual SpriteEffects AccessEffects { get; set; }

            public Point AccessFrameDimensions { get; private set; }
            public Point AccessCountDimensions { get; private set; }
            public float AccessTime { get; set; }
            public float AccessTimeInterval { get; set; }
            public bool AccessRepeat { get; set; }
            public int AccessCurrentFrame => (int)(AccessTime / AccessTimeInterval);
            public int AccessFrameCount { get; private set; }

            public AnimatorScreen(Layer aLayer, Texture2D aSheet, Point someFrameDimensions, Rectangle aTransform, Vector2 anOrigin, float aRotation, Color aColor, float anInterval, float aStartTime, bool aRepeatBool, SpriteEffects someSpriteEffects)
            {
                if (aSheet.Width % someFrameDimensions.X != 0 || aSheet.Height % someFrameDimensions.Y != 0)
                {
                    throw new Exception("Tried to create AnimatorScreen where the format image was not proportional to the frame size.");
                }

                AccessLayer = aLayer;

                AccessFrameDimensions = someFrameDimensions;
                AccessTime = aStartTime;
                AccessTimeInterval = anInterval;
                AccessRepeat = aRepeatBool;
                AccessCountDimensions = new Point(aSheet.Width / someFrameDimensions.X, aSheet.Height / someFrameDimensions.Y);
                AccessFrameCount = AccessCountDimensions.X * AccessCountDimensions.Y;

                AccessTexture = aSheet;
                AccessTransform = aTransform;
                AccessRotation = aRotation;
                AccessOrigin = anOrigin;
                AccessColor = aColor;
                AccessEffects = someSpriteEffects;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                AccessTime += aDeltaTime;
                if (AccessTime > AccessFrameCount * AccessTimeInterval)
                {
                    AccessTime %= (AccessFrameCount * AccessTimeInterval);
                }

                int tempCurrentFrame = AccessCurrentFrame;
                Rectangle tempDestinationRectangle = new Rectangle()
                {
                    X = AccessFrameDimensions.X * (tempCurrentFrame % AccessCountDimensions.X),
                    Y = AccessFrameDimensions.Y * (int)((float)tempCurrentFrame / AccessCountDimensions.X),
                    Width = AccessFrameDimensions.X,
                    Height = AccessFrameDimensions.Y
                };

                Rectangle tempTransform = new Rectangle(AccessTransform.Location + AccessOffset, AccessTransform.Size);

                aSpriteBatch.Draw(AccessTexture, tempTransform, tempDestinationRectangle, AccessColor, AccessRotation, AccessOrigin, AccessEffects, AccessLayer.GetDepth);
            }
        }

        public class Text : RendererIGUI
        {
            public SpriteFont AccessFont { get; set; }
            /// <summary>A StringBuilder class to represent the text shown</summary>
            public StringBuilder AccessString { get; set; }
            public Vector2 AccessPosition { get; set; }
            public Vector2 AccessScale { get; set; }
            /// <summary>A vector between (0,0) and (1,1) to represent the pivot around which the object rotates 
            /// and what point will line up to the Vector2 position</summary>
            public Vector2 AccessOrigin { get; set; }
            public Color AccessColor { get; set; }
            public float AccessRotation { get; set; }
            public SpriteEffects AccessSpriteEffects { get; set; }

            public Text(Layer aLayer, SpriteFont aFont, string aText, float aFontSize, float aRotation, Vector2 aPosition, Vector2 anOrigin, Color aColor)
                : this(aLayer, aFont, new StringBuilder(aText), Vector2.One * aFontSize * FONTSIZEMULTIPLIER, aRotation, aPosition, anOrigin, aColor, SpriteEffects.None)
            { }

            public Text(Layer aLayer, SpriteFont aFont, StringBuilder aText, Vector2 aScale, float aRotation, Vector2 aPosition, Vector2 anOrigin, Color aColor, SpriteEffects someSpriteEffects)
            {
                AccessLayer = aLayer;
                AccessFont = aFont;
                AccessString = aText;
                AccessScale = aScale;
                AccessRotation = aRotation;
                AccessPosition = aPosition;
                AccessOrigin = anOrigin;
                AccessColor = aColor;
                AccessSpriteEffects = someSpriteEffects;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                aSpriteBatch.DrawString(AccessFont, AccessString, AccessPosition + AccessOffset.ToVector2(), AccessColor, AccessRotation, AccessOrigin, AccessScale, AccessSpriteEffects, AccessLayer.GetDepth);
            }
        }

        public class Custom : Renderer
        {
            public DrawCommand AccessCommand { get; private set; }

            public Custom(DrawCommand aDrawCommand, Layer aLayer)
            {
                AccessCommand = aDrawCommand;
                AccessLayer = aLayer;
            }

            public override void Draw(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime)
            {
                AccessCommand.Invoke(aSpriteBatch, aCamera, aDeltaTime, AccessLayer.GetDepth);
            }

            public void SetCommand(DrawCommand aDrawCommand) => AccessCommand = aDrawCommand;
        }

        public delegate void DrawCommand(SpriteBatch aSpriteBatch, Camera aCamera, float aDeltaTime, float aManagedLayer);
    }

    abstract class RendererIGUI : Renderer, IGUIMember
    {
        public Point AccessOffset { get; set; }
        Point IGUIMember.AccessOrigin { get => AccessOffset; set => AccessOffset = value; }

        void IGUIMember.Draw(SpriteBatch aSpriteBatch, MouseState aMouseState, KeyboardState aKeyboardState, float anUnscaledDeltaTime)
        {
            Draw(aSpriteBatch, null, anUnscaledDeltaTime);
        }
    }

    //class Renderer
    //{
    //    static private RendererController controller;

    //    public event Action<SpriteBatch, RendererController, float> OnRender;

    //    public float GetDepth => myLayer.GetDepth;

    //    private Layer myLayer;

    //    public Renderer() : this(Layer.Default, null) { }

    //    public Renderer(Layer aLayer) : this(aLayer, null) { }

    //    public Renderer(Action<SpriteBatch, RendererController, float> aQueue) : this(Layer.Default, aQueue) { }

    //    public Renderer(Layer aLayer, Action<SpriteBatch, RendererController, float> aQueue)
    //    {
    //        myLayer = aLayer;

    //        if (aQueue != null)
    //        {
    //            OnRender += aQueue;
    //        }

    //        controller?.AddRenderer(this);
    //    }

    //    public void Render(SpriteBatch aSpriteBatch, float aDeltaTime)
    //    {
    //        OnRender?.Invoke(aSpriteBatch, controller, aDeltaTime);
    //        Console.WriteLine("Rendering");
    //    }

    //    public void SetLayer(Layer aLayer)
    //    {
    //        myLayer = aLayer;
    //        controller.ChangedState();
    //    }

    //    public void Remove() 
    //        => controller.RemoveRenderer(this);

    //    // Static initialization setting up a static reference to the renderer controller
    //    public static void StaticInit(RendererController aController) 
    //        => controller = aController;
    //}
}
