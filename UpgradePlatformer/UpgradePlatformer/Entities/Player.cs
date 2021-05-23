using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;

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
        public override void Update(GameTime gt, Keys keys)
        {
            if (keys == Keys.D)
            {
                this.speedX += speedX;
            }

            if (keys == Keys.A)
            {
                this.speedX -= speedX;
            }

            if (keys == Keys.W)
            {
                velocity.Y = -10f;
            }
        }

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

        /// <summary>
        /// Checks tiles and resolves collisions accordingly
        /// </summary>
        /// <param name="tiles">list of tiles</param>
        public void ResolveCollisions()
        {
        }
    }
}
