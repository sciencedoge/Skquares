using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Entities
{
    //HEADER=======================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for coins - 
    //a basic collectible
    //=============================================
    class Torch : CollectibleObject
    {
        //Constructor
        public Torch(int value, Rectangle hitbox, Tile t)
            : base(value, hitbox, EntityKind.TORCH, t) 
        {
            SpriteBounds = new Rectangle(80, 13, 16, 16);
            UpdateSprite();
            Bob = 0;
            this.hitbox = t.Position;
        }

        /// <summary>
        /// updates the Coin
        /// </summary>
        public void Update()
        {
            spriteSize = hitbox.Size;
        }

        public override int Intersects(EntityObject o)
        {
            return 0;
        }
    }
}
