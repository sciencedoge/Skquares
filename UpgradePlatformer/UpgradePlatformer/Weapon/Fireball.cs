using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Weapon
{
    //HEADER===========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 6/4/2021
    //Purpose: Creates the fireball boss projectile
    //=================================================
    class Fireball : Projectile
    {
        /// <summary>
        /// Creates a fireball object
        /// </summary>
        public Fireball(Vector2 path, Vector2 location, float rotation)
            :base(path, location, rotation)
        {
            spriteBounds = new Rectangle(28, 0, 7, 7);
            UpdateSprite();
        }

        /// <summary>
        /// Checks for intersections
        /// </summary>
        public override void Intersects()
        {
            Player p = EntityManager.Instance.Player();

            if (hitbox.Intersects(p.Hitbox))
            {
                if (p.IsActive)
                {
                    p.CurrentHP -= 2;

                    isActive = false;
                    if (p.CurrentHP <= 0)
                    {
                        p.IsActive = false;
                    }
                    return;
                }
            }
        }
    }
}