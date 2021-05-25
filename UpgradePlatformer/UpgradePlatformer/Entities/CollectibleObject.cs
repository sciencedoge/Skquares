using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace UpgradePlatformer.Entities
{
    //HEADER==========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for collectibles
    //================================================
    class CollectibleObject
    {
        //Fields

        private Rectangle SpriteBounds = new Rectangle(0, 7, 5, 5);

        protected int value;
        protected Graphics.Sprite sprite;
        protected Rectangle hitbox;
        protected bool isActive;

        protected Point spriteSize;

        //properties
        /// <summary>
        /// returns the value of the
        /// collectible
        /// </summary>
        public int Value
        {
            get { return value; }
        }

        /// <summary>
        /// returns the hitbox of the object
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        /// <summary>
        /// returns whether or not the 
        /// collectible is active 
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }


        //Const
        /// <summary>
        /// Creates a CollectibleObject object
        /// </summary>
        public CollectibleObject(int value, Texture2D sprite, Rectangle hitbox)
        {
            this.value = value;
            this.hitbox = hitbox;
            this.isActive = true;

            this.sprite = new Graphics.Sprite(
                sprite, SpriteBounds,
                new Vector2(SpriteBounds.X - (SpriteBounds.Width / 2),
                SpriteBounds.Y - (SpriteBounds.Height / 2)),
                Color.White);
        }

        /// <summary>
        /// Draws the sprite to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public void Draw(SpriteBatch sb, GameTime gt)
        {
            //if the sprite is active, draws it to the screen
            if (isActive)
            {
                Vector2 Bob = new Vector2(0, 3 * (float)Math.Cos(gt.TotalGameTime.TotalMilliseconds / 100));
                sprite.Draw(sb, hitbox.Location + Bob.ToPoint(), 0, spriteSize.ToVector2());
            }
        }

        /// <summary>
        /// Checks if an intersection happened with a LivingObject
        /// (will be changed to player when implemented)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Intersects(LivingObject obj)
        {
            if (IsActive)
            {
                if (this.hitbox.Intersects(obj.Hitbox))
                {
                    //the coin is no longer active
                    isActive = false;
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
