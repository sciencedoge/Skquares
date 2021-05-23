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
        private Vector2 jumpVelocity;
        private bool keyUp, keyDown, keyLeft, keyRight;

        private int jumpsLeft;

        //screen bounds stuff
        private GraphicsDeviceManager _graphics;


        /// <summary>
        /// Creates a player object
        /// </summary>
        public Player(int maxHp, int damage, Rectangle hitbox,
            Texture2D texture, GraphicsDeviceManager device)
            : base(maxHp, damage, hitbox, texture) 
        {
            jumpVelocity = new Vector2(0, -0.33f);
            jumpsLeft = 1;

            this._graphics = device;
        }

        //Methods

        /// <summary>
        /// Checks if the player intersects with another living object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Intersects(List<LivingObject> obj)
        {
            if (isActive)
            {
                foreach(LivingObject live in obj)
                {
                    if (this.hitbox.Intersects(live.Hitbox))
                    {
                        this.TakeDamage(live.Damage);

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
        public void Update(GameTime gt, InputManager inputManager)
        {

            CheckForInput(inputManager);

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
                while (keyUp && jumpsLeft > 0
                    && velocity.Y >= -30f)
                {
                    velocity.Y += jumpVelocity.Y;
                    CheckForInput(inputManager);                    
                }

                keyUp = false;
                jumpsLeft -= 1;
            }
            Update(gt);
            hitbox.Location = position.ToPoint();
        }
        
        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="gt"></param>
        public override void Update(GameTime gt)
        {
            ApplyGravity();
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public void ApplyGravity()
        {
            velocity += gravity;
            position += velocity;
            //gradual slow down
            velocity.X *= 0.80f;

            if (position.Y > _graphics.PreferredBackBufferHeight - hitbox.Height / 2)
                position.Y = _graphics.PreferredBackBufferHeight - hitbox.Height / 2;

            if (position.X > _graphics.PreferredBackBufferWidth + hitbox.Width)
                position.X = 0 - hitbox.Width;

            if (position.X < 0 - hitbox.Width)
                position.X = _graphics.PreferredBackBufferWidth + hitbox.Width;
             

            if (position.Y >= _graphics.PreferredBackBufferHeight - hitbox.Height) jumpsLeft = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckForInput(InputManager inputManager)
        {
            InputEvent dev = inputManager.Pop(InputEventKind.KEY_DOWN);
            InputEvent uev = inputManager.Pop(InputEventKind.KEY_UP);
            if (dev != null)
            {
                Keys down = (Keys)dev.Data;
                if (down == Keys.W) keyUp = true;
                else if (down == Keys.A) keyLeft = true;
                else if (down == Keys.S) keyDown = true;
                else if (down == Keys.D) keyRight = true;
            }
            if (uev != null)
            {
                Keys up = (Keys)uev.Data;
                if (up == Keys.W) keyUp = false;
                if (up == Keys.A) keyLeft = false;
                if (up == Keys.S) keyDown = false;
                if (up == Keys.D) keyRight = false;
            }
        }
    }
}
