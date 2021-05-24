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
    //Purpose: Creates functionality for enemies
    //===========================================
    class Enemy : LivingObject
    {
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// Creates an enemy object
        /// </summary>
        /// <param name="maxHp">the max hp of the enemy</param>
        /// <param name="damage">the damage that the enemy deals</param>
        /// <param name="hitbox">the hitbox of the enemy</param>
        /// <param name="texture">the texture of the enemy</param>
        public Enemy(int maxHp, int damage, Rectangle hitbox, Texture2D texture, GraphicsDeviceManager _graphics, int jumpsLeft)
            :base(maxHp, damage, hitbox, texture, jumpsLeft)
        {
            this.jumpsLeft = jumpsLeft;
            this._graphics = _graphics;
        }

        /// <summary>
        /// Updates the enemy every frame
        /// </summary>
        /// <param name="gt">gameTime</param>
        public override void Update(GameTime gt)
        {
            hitbox.Location = position.ToPoint();
            ApplyGravity();
        }
        public override void OnFloorCollide()
        {
        }

        public override void ApplyGravity()
        {
            base.ApplyGravity();
            if (position.Y > _graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = _graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;

                jumpsLeft = 1;
            }

            if (position.X > _graphics.PreferredBackBufferWidth + hitbox.Width)
                position.X = 0 - hitbox.Width;

            if (position.X < 0 - hitbox.Width)
                position.X = _graphics.PreferredBackBufferWidth + hitbox.Width;
        }
    }
}
