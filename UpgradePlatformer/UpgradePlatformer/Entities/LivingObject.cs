using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Entities
{
    //HEADER================================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Provides the basis for all living entities in
    //the game
    //======================================================
    class LivingObject : Interfaces.IDamageable, Interfaces.IHostile
    {
        //fields
        protected bool isActive;
        protected int currentHp;
        protected int maxHp;
        protected int damage;

        //Gravity and movement
        protected Vector2 gravity;
        protected Vector2 velocity;
        protected float speedX;
        protected Vector2 position;

        //sprite info
        protected Sprite sprite;

        protected Rectangle hitbox;

        //properties

        /// <summary>
        /// gets or sets whether a
        /// living object is active
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// gets or sets the current
        /// hp of a living object
        /// </summary>
        public int CurrentHP
        {
            get { return currentHp; }
            set { currentHp = value; }
        }

        /// <summary>
        /// Returns the max HP of
        /// a living object
        /// </summary>
        public int MaxHP
        {
            get { return maxHp; }
            set { maxHp = value; }
        }

        /// <summary>
        /// Gets or sets the amount of
        /// damage this object does
        /// </summary>
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        /// <summary>
        /// gets or sets the hitbox of an object
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        //const

        /// <summary>
        /// Creates a new LivingObject object
        /// </summary>
        /// <param name="maxHp">max hp of the object</param>
        public LivingObject(int maxHp, int damage, Rectangle hitbox,
            Texture2D texture)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
            this.damage = damage;
            isActive = true;

            this.hitbox = hitbox;

            this.sprite = new Sprite(
                texture, hitbox, 
                new Vector2(hitbox.X - (hitbox.Width / 2),
                hitbox.Y - (hitbox.Height / 2)),
                Color.White);

            gravity = new Vector2(0, 1);
            velocity = new Vector2(0, 0);
            speedX = 2f;

            position = new Vector2(hitbox.X, hitbox.Y);
        }

        //methods
        /// <summary>
        /// reduces the amount of current HP
        /// </summary>
        /// <param name="amount">amount of damage dealt</param>
        public void TakeDamage(int amount)
        {
            this.currentHp -= amount;
        }

        /// <summary>
        /// Draws sprites to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public void Draw(SpriteBatch sb, GameTime gt)
        {
            if (isActive)
            {
                sprite.Draw(sb, hitbox.Location, 0);
            }
        }

        /// <summary>
        /// Updates entities every frame
        /// </summary>
        /// <param name="gt">gameTime</param>
        public virtual void Update(GameTime gt)
        {

        }

        /// <summary>
        /// Processes input
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="keys"></param>
        public virtual void Update(GameTime gt, Keys keys)
        {
            
        }

        public void CollisionResolve()
        {

        }
    }
}
