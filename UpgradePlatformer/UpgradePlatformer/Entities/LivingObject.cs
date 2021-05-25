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
    abstract class LivingObject : EntityObject, Interfaces.IDamageable, Interfaces.IHostile
    {
        private const int cooldownTime = 120;
        private Rectangle SpriteBounds = new Rectangle(17, 14, 14, 14);

        //fields
        protected bool isActive;
        protected int currentHp;
        protected int maxHp;
        protected int damage;

        protected int jumpsLeft;
        protected int cooldown;

        //Gravity and movement
        protected Vector2 gravity;
        protected Vector2 velocity;
        protected float speedX;
        protected Vector2 position;
        protected Vector2 jumpVelocity;
        protected Point spriteSize;
        protected Point spawn;

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

        /// <summary>
        /// gets or sets the x value of the 
        /// hitbox
        /// </summary>
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y value of the
        /// hitbox
        /// </summary>
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        /// <summary>
        /// Gets or sets the velocity of the
        /// objecf
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }      
        }

        /// <summary>
        /// returns the position of a living object
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Returns the jump velocity of
        /// the object
        /// </summary>
        public Vector2 JumpVelocity
        {
            get { return jumpVelocity; }
        }

        /// <summary>
        /// returns the number of jumps left
        /// for the entity
        /// </summary>
        public int JumpsLeft
        {
            get { return jumpsLeft; }
            set { jumpsLeft = value; }
        }

        //const

        /// <summary>
        /// Creates a new LivingObject object
        /// </summary>
        /// <param name="maxHp">max hp of the object</param>
        public LivingObject(int maxHp, int damage, Rectangle hitbox,
            Texture2D texture, int jumpsLeft)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
            this.damage = damage;
            isActive = true;

            this.hitbox = hitbox;
            this.jumpsLeft = jumpsLeft;

            this.sprite = new Sprite(
                texture, SpriteBounds, 
                new Vector2(SpriteBounds.X - (SpriteBounds.Width / 2),
                SpriteBounds.Y - (SpriteBounds.Height / 2)),
                Color.White);
            this.spawn = hitbox.Location;

            gravity = new Vector2(0, 0.1f);
            velocity = new Vector2(0, 0);
            speedX = 0.5f;
            jumpVelocity = new Vector2(0, -1.8f);

            position = new Vector2(hitbox.X, hitbox.Y);
        }

        //methods
        /// <summary>
        /// reduces the amount of current HP
        /// </summary>
        /// <param name="amount">amount of damage dealt</param>
        public void TakeDamage(int amount)
        {
            if (cooldown != 0)
                return;
            cooldown = cooldownTime;
            this.currentHp -= amount;
            if (CurrentHP <= 0)
            {
                isActive = false;
            }
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
                sprite.Draw(sb, hitbox.Location, 0, spriteSize.ToVector2());
            }
        }

        /// <summary>
        /// Updates entities every frame
        /// </summary>
        /// <param name="gt">gameTime</param>
        public abstract void Update(GameTime gt);

        public abstract void OnFloorCollide();

        public virtual void ApplyGravity()
        {
            position += velocity;
            velocity += gravity;

            velocity.X *= 0.70f;

            cooldown --;
            cooldown = Math.Max(cooldown, 0);
        }
        
        public void resetPosition(){
            position = spawn.ToVector2();
        }
    }
}
