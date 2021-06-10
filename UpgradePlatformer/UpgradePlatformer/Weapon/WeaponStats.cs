﻿using System;
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
        public int knockBackTime;
        public int cap;
        public float projSpeed;
        public int pierce;
        public WeaponStats(float knockBack, int cap, int knockBackTime, float projSpeed, int pierce){
            this.projSpeed = projSpeed;
            this.knockBack = knockBack;
            this.knockBackTime = knockBackTime;
            this.cap = cap;
            this.pierce = pierce;
        }
    }
}
