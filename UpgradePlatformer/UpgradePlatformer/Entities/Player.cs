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
        public bool keyUp, keyDown, keyLeft, keyRight;


        /// <summary>
        /// Creates a player object
        /// </summary>
        public Player(int maxHp, int damage, Rectangle hitbox, Texture2D texture)
            : base(maxHp, damage, hitbox, texture) 
        {
            jumpVelocity = new Vector2(0, -1);
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
            InputEvent dev = inputManager.Pop(InputEventKind.KEY_DOWN);
            InputEvent uev = inputManager.Pop(InputEventKind.KEY_UP);
            Keys down = (Keys)dev.Data;
            Keys up = (Keys)dev.Data;
            if (down == Keys.W) keyUp = true;
            if (down == Keys.A) keyLeft = true;
            if (down == Keys.S) keyDown = true;
            if (down == Keys.D) keyRight = true;
            if (up == Keys.W) keyUp = false;
            if (up == Keys.A) keyLeft = false;
            if (up == Keys.S) keyDown = false;
            if (up == Keys.D) keyRight = false;

            if (keyRight)
            {
                this.speedX += speedX;
            }

            if (keyLeft)
            {
                this.speedX -= speedX;
            }

            if (keyUp)
            {
                velocity.Y = -10f;
            }

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
            position += gravity;
        }
    }
}
