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

        private Random random;
        private int phase;

        private int fireballChance;

        private List<Fireball> fireballs;
            
        /// <summary>
        /// Creates the BossAI object
        /// </summary>
        public BossAI(Player player, Boss boss)
        {
            this.boss = boss;
            this.player = player;

            random = new Random();
            fireballs = new List<Fireball>();

            fireballChance = 1000;
            phase = 0;
        }

        /// <summary>
        /// Determines the phases and random chance of summoning fireballs
        /// </summary>
        public void Update()
        {
            if(boss.CurrentHP <= 50
                && phase == 0)
            {
                phase = 1;
                fireballChance = 100;
            }
            else if(boss.CurrentHP <= 10
                && phase == 1)
            {
                phase = 2;
                fireballChance = 10;
            }

            foreach(Fireball b in fireballs)
            {
                b.Update();
            }
        }

        //Methods
        /// <summary>
        /// Allows the boss to shoot a fireball
        /// </summary>
        public void ShootFireball()
        {
            if(random.Next(0, fireballChance) == fireballChance - 1)
            {
                fireballs.Add(new Fireball(player.Position, boss.Position, 0));
            }
        }

    }
}
