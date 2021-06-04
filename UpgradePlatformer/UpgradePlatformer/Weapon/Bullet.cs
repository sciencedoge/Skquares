using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Weapon
{
    //HEADER==========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/30/2021
    //Purpose: Creates bullets for the weapon
    //================================================
    class Bullet : Projectile
    {
        /// <summary>
        /// Creates a bullet object
        /// </summary>
        public Bullet(Vector2 path, Vector2 location, float rotation)
            : base(path, location, rotation) { }
    }
}
