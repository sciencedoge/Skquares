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
        /// <summary>
        /// Creates an enemy object
        /// </summary>
        /// <param name="maxHp">the max hp of the enemy</param>
        /// <param name="damage">the damage that the enemy deals</param>
        /// <param name="hitbox">the hitbox of the enemy</param>
        /// <param name="texture">the texture of the enemy</param>
        public Enemy(int maxHp, int damage, Rectangle hitbox, Texture2D texture)
            :base(maxHp, damage, hitbox, texture)
        {

        }

        /// <summary>
        /// Updates the enemy every frame
        /// </summary>
        /// <param name="gt">gameTime</param>
        public override void Update(GameTime gt)
        {
            hitbox.Location = position.ToPoint();
        }
        public override void OnFloorCollide()
        {
        }
    }
}
