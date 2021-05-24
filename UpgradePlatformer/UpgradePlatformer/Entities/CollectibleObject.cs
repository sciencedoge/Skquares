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
        protected int value;
        protected Graphics.Sprite sprite;
        protected Rectangle hitbox;
        protected bool isActive;

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

            this.sprite = new Graphics.Sprite(
                sprite, hitbox,
                new Vector2(hitbox.X - (hitbox.Width / 2),
                hitbox.Y - (hitbox.Height / 2)),
                Color.White);
        }

        /// <summary>
        /// Draws the sprite to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public void Draw(SpriteBatch sb)
        {
            //if the sprite is active, draws it to the screen
            if (isActive)
            {
                sprite.Draw(sb, hitbox.Location, 0);
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
    }
}
