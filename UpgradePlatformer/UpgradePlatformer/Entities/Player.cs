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
            jumpVelocity = new Vector2(0, -1.8f) ;
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
                if (jumpsLeft > 0 && velocity.Y >= -4f)
                {
                    velocity.Y += jumpVelocity.Y;
                } else if (!(velocity.Y >= -4f)) {
                    keyUp = false;
                    jumpsLeft -= 1;
                }
            }

            ApplyGravity();
            hitbox.Location = position.ToPoint();
        }
        
        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="gt"></param>
        public override void Update(GameTime gt)
        {
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public void ApplyGravity()
        {
            position += velocity;
            velocity += gravity;
                      
            velocity.X *= 0.70f;

            if (position.Y > _graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = _graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;
            }

            if (position.X > _graphics.PreferredBackBufferWidth + hitbox.Width)
                position.X = 0 - hitbox.Width;

            if (position.X < 0 - hitbox.Width)
                position.X = _graphics.PreferredBackBufferWidth + hitbox.Width;
             

            if (position.Y >= _graphics.PreferredBackBufferHeight - hitbox.Height) jumpsLeft = 1;
        }
        
        public override void OnFloorCollide()
        {
            jumpsLeft = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckForInput(InputManager inputManager)
        {
            inputManager.Update();
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
                if (keyUp && up == Keys.W) { keyUp = false; jumpsLeft -= 1;}
                else if (up == Keys.A) keyLeft = false;
                else if (up == Keys.S) keyDown = false;
                else if (up == Keys.D) keyRight = false;
            }
        }
    }
}
