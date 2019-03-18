using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VectoidOdyssey
{
    public enum PlayerWeaponType
    {
        Green, LightBlue, Orange, Pink, Red, Teal
    }

    abstract partial class PlayerWeapon
    {
        public const int
            LEVELS = 6;

        public abstract PlayerWeaponType GetWeaponType { get; }

        public WeaponStats GetWeaponStats => myWeaponStats; 

        public Renderer.Sprite AccessRenderer { get; protected set; }

        public int WeaponLevel { get; protected set; }

        protected WeaponStats myWeaponStats;
        protected float myRotation;

        public PlayerWeapon()
        {
            AccessRenderer = new Renderer.Sprite(new Layer(MainLayer.Main, 1), Load.Get<Texture2D>(GetWeaponType.ToString()), Vector2.Zero, Vector2.One, Color.White, 0, new Vector2(16, 16));
            AccessRenderer.AccessSourceRectangle = new Rectangle(0, 0, 32, 32);
        }

        public abstract void Fire();
        public abstract void Init(Player aPlayer);
        public abstract void Update(float aDeltaTime);

        public virtual void SetRotation(float aRotation)
        {
            AccessRenderer.AccessRotation = aRotation;
            myRotation = aRotation;
        }

        protected virtual void SetStats(int aLevel)
        {

        }

        public void SetLevel(int aLevel)
        {

        }

        public static PlayerWeapon GetNew(PlayerWeaponType aType)
        {
            switch (aType)
            {
                case PlayerWeaponType.Green:
                    break;

                case PlayerWeaponType.LightBlue:
                    break;

                case PlayerWeaponType.Orange:
                    break;

                case PlayerWeaponType.Pink:
                    break;

                case PlayerWeaponType.Red:
                    break;

                default:
                    return new Teal();
            }

            return new Teal();
        }

    }
}
