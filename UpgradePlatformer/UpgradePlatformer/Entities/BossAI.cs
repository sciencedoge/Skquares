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
    class BossAI : PathfindingAI
    {
        //Fields
        private Boss boss;

        /// <summary>
        /// Creates the BossAI object
        /// </summary>
        public BossAI(List<Enemy> enemies, Player player, Boss boss)
            : base(enemies, player) { }

        //Methods
        public void ShootFireball()
        {

        }

    }
}
