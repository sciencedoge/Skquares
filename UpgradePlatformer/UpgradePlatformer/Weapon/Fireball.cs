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
            spriteBounds = new Rectangle(20, 7, 5, 5);
        }

        //Methods

        /// <summary>
        /// Checks for intersections with fireballs
        /// </summary>
        public override void Intersects()
        {
            foreach (Enemy e in EntityManager.Instance.Enemies())
            {
                if (hitbox.Intersects(e.Hitbox))
                {
                    if (e.IsActive)
                    {
                        e.CurrentHP -= EntityManager.Instance.Player().Damage;

                        isActive = false;
                        if (e.CurrentHP <= 0)
                        {
                            e.IsActive = false;
                        }
                        return;
                    }
                }
            }
        }
    }
}
