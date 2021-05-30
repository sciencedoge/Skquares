using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UpgradePlatformer.Weapon
{
    //HEADER===================================================
    //Names: Sami Chamberlain, Preston Precourt
    //Date: 5/30/2021
    //Purpose: Gives functionality for the player's weapon
    //=========================================================
    class Weapon
    {

        //Fields
        private Sprite sprite;
        private bool isActive;
        private float rotation;
        private Vector2 position;

        private SpriteEffects effect;

        private Rectangle spriteBounds;

        private MouseState ms; //using this for now (not very familiar with current input system)

        //Properties

        /// <summary>
        /// returns whether or not the weapon is active
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }

        }

        /// <summary>
        /// Gets or sets the position of the weapon
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Ctor

        /// <summary>
        /// Creates a new Weapon object
        /// </summary>
        public Weapon(Vector2 position)
        {
            this.position = position;

            spriteBounds = new Rectangle(17, 14, 14, 14);

            this.sprite = new Sprite(
               spriteBounds,
               new Vector2(spriteBounds.X - (spriteBounds.Width / 2),
               spriteBounds.Y - (spriteBounds.Height / 2)),
               Color.White);

            this.isActive = false;

            this.effect = SpriteEffects.None;
        }      

        //Methods
        
        /// <summary>
        /// finds the current rotation of the weapon
        /// </summary>
        /// <returns></returns>
        public float FindRotation()
        {

            float distX = position.X - ms.X;
            float distY = position.Y - ms.Y;

            float distance = (float)Math.Atan(distY / distX);

            if(distX <= 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }

            return (float)Math.Atan(distY / distX);
        }

        /// <summary>
        /// Draws the weapon to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sprite.Draw(sb, position.ToPoint(), rotation, effect);
            }
        }

        /// <summary>
        /// Updates the weapon
        /// </summary>
        public void Update()
        {
            ms = Mouse.GetState();
            rotation = FindRotation();

        }

    }
}
