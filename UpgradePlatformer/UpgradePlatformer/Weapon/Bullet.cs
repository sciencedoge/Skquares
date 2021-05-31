using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Weapon
{
    //HEADER==========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/30/2021
    //Purpose: Creates bullets for the weapon
    //================================================
    class Bullet
    {
        //Fields 
        private Sprite sprite;

        private Vector2 path;
        private Vector2 location;
        private Vector2 speed;
        public bool isActive;

        private Rectangle hitbox;

        private Rectangle spriteBounds;

        //Ctor
        /// <summary>
        /// creates a bullet object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>
        public Bullet(Vector2 path, Vector2 location)
        {
            spriteBounds = new Rectangle(20, 7, 5, 5);
            this.path = path;
            this.location = location;
            this.speed = new Vector2(path.X / 60, path.Y / 60);

            this.sprite = new Sprite(
               spriteBounds,
               new Vector2(spriteBounds.X - (spriteBounds.Width / 2),
               spriteBounds.Y - (spriteBounds.Height / 2)),
               Color.White);

            isActive = true;
            this.hitbox = new Rectangle(location.ToPoint(), new Point(10, 10));
        }

        //Methods

        /// <summary>
        /// draws bullet to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sprite.Draw(sb, location.ToPoint(), 0f, new Vector2(10));
            }
        }

        /// <summary>
        /// Checks for intersections
        /// </summary>
        public void Intersects()
        {
            foreach(Enemy e in EntityManager.Instance.Enemies())
            {
                if (hitbox.Intersects(e.Hitbox))
                {
                    e.CurrentHP -= EntityManager.Instance.Player().Damage;
                    isActive = false;

                    if(e.CurrentHP <= 0)
                    {
                        e.IsActive = false;
                    }
                }
            }
        }

        /// <summary>
        ///updates the location of the bullet
        /// </summary>
        public void Update()
        {
            if (isActive)
            {
                location -= speed;
                this.hitbox = new Rectangle(location.ToPoint(), new Point(10, 10));
                Intersects();
                
                foreach(Tile t in LevelManager.Instance.ActiveLevel().Tiles)
                {
                    if(t.CollisionKind != 9 && !t.Spawner)
                    {
                        if (hitbox.Intersects(t.Position))
                        {
                            isActive = false;
                        }
                    }                   
                }

                if(location.X > Sprite.graphics.PreferredBackBufferWidth 
                    || location.X < 0 || location.Y > Sprite.graphics.PreferredBackBufferHeight ||
                    location.Y < 0)
                {
                    isActive = false;
                }
            }            
        }
    }
}
