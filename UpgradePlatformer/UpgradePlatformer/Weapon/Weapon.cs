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
        private AnimationFSM animation;
        private bool isActive;
        private float rotation;
        private Vector2 position;

        private List<Bullet> bullets;

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

            this.animation = AnimationManager.Instance.animations[2];
            this.isActive = false;

            this.effect = SpriteEffects.None;

            bullets = new List<Bullet>();
        }      

        //Methods
        
        public Vector2 FindDistance()
        {
            float distX = position.X - ms.X;
            float distY = position.Y - ms.Y;

            return new Vector2(distX, distY);
        }

        /// <summary>
        /// finds the current rotation of the weapon
        /// </summary>
        /// <returns></returns>
        public float FindRotation(Vector2 dist)
        {           

            float distance = (float)Math.Atan(dist.Y / dist.X);

            if(dist.X <= 0)
            {
                distance += MathF.PI;
            }

            return (float)Math.Atan(dist.Y / dist.X);
        }

        /// <summary>
        /// Draws the weapon to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                animation.Draw(sb, position.ToPoint(), rotation, new Vector2(14));
                for (int i = bullets.Count - 1; i > 0; i--)
                {
                    if (bullets[i].isActive)
                    {
                        bullets[i].Draw(sb);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the weapon
        /// </summary>
        public void Update()
        {

            for (int i = bullets.Count - 1; i > 0; i--)
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Update();
                }
                else
                {
                    bullets.Remove(bullets[i]);
                }
            }

            ms = Mouse.GetState();

            Vector2 path = FindDistance();

            rotation = FindRotation(path);

            if(ms.LeftButton == ButtonState.Pressed)
            {
                bullets.Add(new Bullet(path, Position));      
            }
        }

    }
}
