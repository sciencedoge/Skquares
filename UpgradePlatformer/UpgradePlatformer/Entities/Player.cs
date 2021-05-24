using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Input;

namespace UpgradePlatformer.Entities
{
    //Header=====================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for players
    //===========================================
    class Player : LivingObject
    {

        //Fields
        private bool keyUp, keyDown, keyLeft, keyRight;
        private int jumpsLeft;

        private bool invincible;

        //screen bounds stuff
        private GraphicsDeviceManager _graphics;


        /// <summary>
        /// Creates a player object
        /// </summary>
        public Player(int maxHp, int damage, Rectangle hitbox,
            Texture2D texture, GraphicsDeviceManager device)
            : base(maxHp, damage, hitbox, texture) 
        {
            jumpsLeft = 2;

            this._graphics = device;

            invincible = false;
        }

        //Methods

        /// <summary>
        /// Checks if the player intersects with another living object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Intersects(List<Enemy> obj)
        {
            if (invincible)
            {
                return;
            }

            if (isActive)
            {
                foreach(Enemy enemy in obj)
                {
                    if (this.hitbox.Intersects(enemy.Hitbox))
                    {
                        invincible = true;
                        this.TakeDamage(enemy.Damage);

                        if (CurrentHP <= 0)
                        {
                            isActive = false;
                        }
                    }
                }             
            }
        }

        /// <summary>
        /// Updates the player's position based on the
        /// key pressed
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="keys"></param>
        public void Update(GameTime gt, EventManager eventManager, InputManager inputManager)
        {
            
            CheckForInput(inputManager, eventManager);

            //put for testing purposes so the player doesnt immediately die
            if(gt.TotalGameTime.TotalSeconds % 1 <= 0.01)
            {
                invincible = false;
            }

            if (keyRight)
            {
                velocity.X += speedX;
            }

            if (keyLeft)
            {
                velocity.X -= speedX;
            }

            if (keyUp)
            {
                //check for ground collision
                if (jumpsLeft > 0 && velocity.Y >= -4f)
                {
                    velocity.Y += jumpVelocity.Y;
                } else if (!(velocity.Y >= -4f)) {
                    keyUp = false;
                    jumpsLeft -= 1;
                }
            }
            Update(gt);
        }
        
        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="gt"></param>
        public override void Update(GameTime gt)
        {
            ApplyGravity();
            hitbox.Location = position.ToPoint();
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public override void ApplyGravity()
        {
            base.ApplyGravity();

            if (position.Y > _graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = _graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;
            }

            if (position.X > _graphics.PreferredBackBufferWidth + hitbox.Width)
                position.X = 0 - hitbox.Width;

            if (position.X < 0 - hitbox.Width)
                position.X = _graphics.PreferredBackBufferWidth + hitbox.Width;
             

            if (position.Y >= _graphics.PreferredBackBufferHeight - hitbox.Height) jumpsLeft = 2;
        }
        
        public override void OnFloorCollide()
        {
            jumpsLeft = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckForInput(InputManager inputManager, EventManager eventManager)
        {
            inputManager.Update(eventManager);
            Event dev = eventManager.Pop("KEY_DOWN");
            Event uev = eventManager.Pop("KEY_UP");
            if (dev != null)
            {
                Keys down = (Keys)dev.Data;
                if (down == Keys.W) keyUp = true;
                else if (down == Keys.A) { keyLeft = true; sprite.effects = SpriteEffects.None; }
                else if (down == Keys.S) keyDown = true;
                else if (down == Keys.D) { keyRight = true; sprite.effects = SpriteEffects.FlipHorizontally; }
            }
            if (uev != null)
            {
                Keys up = (Keys)uev.Data;
                if (keyUp && up == Keys.W) { keyUp = false; jumpsLeft -= 1; }
                else if (up == Keys.A) keyLeft = false; 
                else if (up == Keys.S) keyDown = false;
                else if (up == Keys.D) keyRight = false;
            }
        }
        
        /// <summary>
        /// Respawns the player
        /// </summary>
        public void Respawn()
        {
            this.currentHp = maxHp;
            this.isActive = true;
        }
    }
}
