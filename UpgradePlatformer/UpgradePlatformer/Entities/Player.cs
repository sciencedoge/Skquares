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
        public void Intersects(LivingObject obj)
        {
            if (isActive)
            {
                if (this.hitbox.Intersects(obj.Hitbox))
                {
                    this.CurrentHP -= obj.Damage;

                    if(CurrentHP <= 0)
                    {
                        isActive = false;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the player
        /// every frame
        /// </summary>
        /// <param name="gt">game time</param>
        public override void Update(GameTime gt)
        {

        }

        /// <summary>
        /// tracks player movement
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="key"></param>
        public void Update(Keys key)
        {

        }
    }
}
