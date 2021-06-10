using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Levels;

//HEADER=============================================
//Name: Sami Chamberlain, Preston Precourt
//Date: 6/4/2021
//Purpose: Parent class for all projectiles
//====================================================

namespace UpgradePlatformer.Weapon
{
    class Projectile
    {
        //Fields 
        protected Sprite sprite;
        protected int cap;
        protected Vector2 path;
        protected Vector2 location;
        protected Vector2 speed;
        public bool isActive;
        protected Rectangle hitbox;

        public Vector2 initialLocation;

        protected Rectangle spriteBounds;
        protected float rotation;

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
        public Projectile(Vector2 path, Vector2 location, float rotation, int cap)
        {
            spriteBounds = new Rectangle(20, 7, 5, 5);
            UpdateSprite();
            this.path = path;
            this.location = location;
            this.initialLocation = location;
            this.rotation = rotation;
            this.cap = cap;
            this.speed = path / Vector2.Distance(path, new Vector2(0, 0)) * 2;

            //this.speed = new Vector2(path.X / 60, path.Y / 60);

            isActive = true;
            this.hitbox = new Rectangle(location.ToPoint(), new Point(7, 7));
        }

        public void UpdateSprite()
        {
            this.sprite = new Sprite(
               spriteBounds,
               new Vector2(2.5f, 2.5f),
               Color.White);
        }

        //Methods

        /// <summary>
        /// draws bullet to the screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sprite.Draw(sb, (location).ToPoint(), rotation, this.hitbox.Size.ToVector2());
            }
        }

        /// <summary>
        /// Checks for intersections
        /// </summary>
        public virtual void Intersects()
        {
            foreach (Enemy e in EntityManager.Instance.Enemies())
            {
                if (hitbox.Intersects(e.Hitbox))
                {
                    if (e.IsActive)
                    {
                        e.TakeDamage(EntityManager.Instance.Player().Damage);

                        isActive = false;
                        if (e.CurrentHP <= 0)
                        {
                            e.IsActive = false;
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///updates the location of the bullet
        /// </summary>
        public virtual void Update(GameTime gt)
        {
            if (isActive)
            {
                location -= speed * (float)(gt.ElapsedGameTime.TotalSeconds * 60);
                this.hitbox = new Rectangle(location.ToPoint(), new Point(10, 10));
                Intersects();

                Vector2 distance = FindDistance();

                if(distance.X > cap)
                {
                    this.isActive = false;
                    return;
                }

                if(distance.Y > cap)
                {
                    this.isActive = false;
                    return;
                }

                foreach (Tile t in LevelManager.Instance.GetCollisions(hitbox))
                {
                    if (t.CollisionKind == 9 || t.CollisionKind == 104 || t.CollisionKind == 105)
                        continue;
                    isActive = false;
                }

                if (location.X > Sprite.graphics.PreferredBackBufferWidth
                    || location.X < 0 || location.Y > Sprite.graphics.PreferredBackBufferHeight ||
                    location.Y < 0)
                {
                    isActive = false;
                }
            }
        }

        /// <summary>
        /// Finds the distance between the bullet's current and
        /// initial position
        /// </summary>
        private Vector2 FindDistance()
        {
            float distX = MathF.Abs(location.X - initialLocation.X);
            float distY = MathF.Abs(location.Y - (initialLocation.Y - (40 * Sprite.GetScale())));

            return new Vector2(distX, distY);
        }
    }
}
