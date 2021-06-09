using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Input;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Weapon
{
    //HEADER===================================================
    //Names: Sami Chamberlain, Preston Precourt
    //Date: 5/30/2021
    //Purpose: Gives stats for the player's weapon
    //=========================================================
    public class WeaponStats
    {
        public float knockBack;
    
        public WeaponStats(float knockBack){
            this.knockBack = knockBack;
        }
    }
}
