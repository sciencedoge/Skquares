using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Input;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
    //HEADER================================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Provides the basis for all living entities in
    //the game
    //======================================================
    public abstract class LivingObject : EntityObject, Interfaces.IDamageable, Interfaces.IHostile
    {
        private const int cooldownTime = 2500;
        private Rectangle SpriteBounds = new Rectangle(17, 14, 14, 14);

        //fields
        protected int currentHp;
        protected int maxHp;
        protected int damage;

        protected int jumpsLeft;
        public double cooldown;

        //Gravity and movement
        protected Vector2 gravity;
        protected Vector2 velocity;
        protected float speedX;
        protected Vector2 position;
        protected Vector2 jumpVelocity;
        protected Point spriteSize;
        protected Point spawn;

        //sprite info
        public AnimationFSM animation;

        //properties

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
            get { return IsActive ? damage : 0; }
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
            set { position = value; }
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
        
        /// <summary>
        /// Returns the x speed of the entity
        /// </summary>
        public float SpeedX
        {
            get { return speedX; }
        }

        //const

        /// <summary>
        /// creates a living object
        /// </summary>
        /// <param name="maxHp">the max hp of the object</param>
        /// <param name="damage">the damage the object deals</param>
        /// <param name="hitbox">the starting hitbox of the object</param>
        /// <param name="jumpsLeft">the ammount of jumps to start with</param>
        /// <param name="kind">the kind of the object</param>
        public LivingObject(int maxHp, int damage, Rectangle hitbox,
            int jumpsLeft, EntityKind kind) : base(kind)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
            this.damage = damage;
            IsActive = true;

            this.hitbox = hitbox;
            this.jumpsLeft = jumpsLeft;

            spriteSize = SpriteBounds.Size;
            this.animation = AnimationManager.Instance.animations[0];
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
            if (cooldown > 0)
                return;
            this.currentHp -= amount;
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("damage");
            if (CurrentHP <= 0)
            {
                IsActive = false;
            }
            cooldown = cooldownTime;          
        }

        /// <summary>
        /// Draws sprites to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            if (IsActive && cooldown % 6 != 1)
            {          
                animation.Draw(sb, hitbox.Location, 0, spriteSize.ToVector2());
            }
        }

        /// <summary>
        /// processes floor collisions
        /// </summary>
        public abstract void OnFloorCollide();

        /// <summary>
        /// applys gravity to the entity
        /// </summary>
        public virtual void ApplyGravity(GameTime gt)
        {
            // fixes different speeds on different window sizes
            position += velocity * ((float)gt.ElapsedGameTime.TotalSeconds * 60f);
            velocity += gravity * ((float)gt.ElapsedGameTime.TotalSeconds * 60f);
            velocity.X *= 0.70f;
            velocity.X = Math.Min(velocity.X, 6);
            velocity.Y = Math.Min(velocity.Y, 6);   
        }
        
        /// <summary>
        /// resets the entitys position
        /// </summary>
        public void ResetPosition(){
            position = spawn.ToVector2();
            EventManager.Instance.Push(new Event("LEVEL_SHOW", 0, new Point()));
        }
    }
}
