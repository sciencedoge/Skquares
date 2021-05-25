using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UpgradePlatformer.Entities
{
    //HEADER=======================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for coins - 
    //a basic collectible
    //=============================================
    class Coin : CollectibleObject
    {
        //Constructor
        public Coin(int value, Texture2D texture, Rectangle hitbox)
            : base(value, texture, hitbox) { }

        public void Update()
        {
            spriteSize = hitbox.Size;
        }
    }
}
