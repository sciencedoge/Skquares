using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Entities
{
    //HEADER================================================
    //Name: Sami Chamberlain
    //Date: 5/24/2021
    //Purpose: Creates enemy AI
    //======================================================
    class PathfindingAI
    {
        //fields
        private List<Enemy> enemies;
        private Player player;

        private Vector2[,] relationships;


        //Constructor

        /// <summary>
        /// Creates a new PathFindingAI object
        /// </summary>
        public PathfindingAI(List<Enemy> enemies, Player player)
        {
            this.enemies = enemies;
            this.player = player;

            relationships = new Vector2[enemies.Count, 1];
        }

        //Methods

        /// <summary>
        /// Updates the distances between the enemies and the player
        /// </summary>
        public void UpdateCosts()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                float distX = Math.Abs(enemies[i].Position.X - player.Position.X);
                float distY = Math.Abs(enemies[i].Position.Y - player.Position.Y);
                relationships[i, 0] = new Vector2(distX, distY);
            }
        }

        /// <summary>
        /// moves the enemy to the player
        /// </summary>
        public void MoveToPlayer()
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(relationships[i, 0].X < 150)
                {
                    if(enemies[i].X > player.X)
                    {
                        enemies[i].X -= 0.5f;
                    }
                    else
                    {
                        enemies[i].X += 0.5f;
                    }
                }
            }
        }

        /// <summary>
        /// Performs a jump on the enemy
        /// (will add after confirming basic pathfinding)
        /// </summary>
        public void AIJump()
        {

        }


    }
}
