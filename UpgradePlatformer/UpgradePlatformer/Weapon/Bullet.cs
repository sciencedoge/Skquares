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
        private float rotation;

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        //Ctor
        /// <summary>
        /// creates a bullet object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>
        public Bullet(Vector2 path, Vector2 location, float rotation)
        {
            spriteBounds = new Rectangle(20, 7, 5, 5);
            this.path = path;
            this.location = location;
            this.rotation = rotation;

            this.speed = path / Vector2.Distance(path, new Vector2(0, 0)) * 2;

            //this.speed = new Vector2(path.X / 60, path.Y / 60);

            this.sprite = new Sprite(
               spriteBounds,
               new Vector2(2.5f, 2.5f),
               Color.White);

            isActive = true;
            this.hitbox = new Rectangle(location.ToPoint(), new Point(7, 7));
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
                sprite.Draw(sb, (location).ToPoint(), rotation, this.hitbox.Size.ToVector2());
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

                    if (e.IsActive)
                    {
                        isActive = false;
                    }                   
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
                    if (t.CollisionKind == 9 || t.CollisionKind == 101 || t.CollisionKind == 102 || t.CollisionKind == 105)
                        continue;
                    if (hitbox.Intersects(t.Position))
                    {
                        isActive = false;
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
