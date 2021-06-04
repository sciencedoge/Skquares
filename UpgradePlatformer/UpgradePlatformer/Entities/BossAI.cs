using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Weapon;

namespace UpgradePlatformer.Entities
{
    //HEADER===========================================
    //Names: Sami Chamberlain, Preston Precourt
    //Date: 6/3/2021
    //PUrpose: Creates the AI for the boss Entity
    //=================================================
    class BossAI
    {
        //Fields
        private Boss boss;
        private Player player;

        /// <summary>
        /// Creates the BossAI object
        /// </summary>
        public BossAI(Player player, Boss boss)
        {
            this.boss = boss;
            this.player = player;
        }

        //Methods
        public void ShootFireball()
        {

        }

    }
}
