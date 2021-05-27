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
        private float speed;
        public Player player;
        public List<Enemy> _enemies;
        public List<Enemy> enemies { get => _enemies; set { _enemies = value; relationships = new Vector2[enemies.Count, 1];} }
        private float goombaAINum;
        private Vector2[,] relationships;


        //Constructor

        /// <summary>
        /// Creates a new PathFindingAI object
        /// </summary>
        public PathfindingAI(List<Enemy> enemies, Player player)
        {
            this.enemies = enemies;
            this.player = player;
            speed = 0.5f;
            goombaAINum = speed;
            relationships = new Vector2[enemies.Count, 1];
        }

        //Methods

        /// <summary>
        /// Updates the distances between the enemies and the player
        /// </summary>
        public void UpdateCosts()
        {
            if (player == null)
                return;
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
            if (player == null) return;
            for(int i = 0; i < enemies.Count; i++)
            {
                if(relationships[i, 0].X < 150)
                {
                    if(enemies[i].X > player.X
                        && !enemies[i].Colliding)
                    {
                        enemies[i].X -= speed;
                        enemies[i].Flip = false;
                    }
                    else if(enemies[i].X < player.X
                        && !enemies[i].Colliding)
                    {
                        enemies[i].X += speed;
                        enemies[i].Flip = true;
                    }
                    else
                    {
                        enemies[i].Colliding = false;
                    }

                    if (relationships[i, 0].Y > 20
                        && player.Y < enemies[i].Y)
                    {
                        AIJump(enemies[i]);                       
                    }
                }

                else
                {
                    enemies[i].Flip = goombaAINum > 0;
                    if(enemies[i].Colliding)
                    {
                        goombaAINum *= -1;
                        enemies[i].X += goombaAINum;
                        enemies[i].Colliding = false;
                    }

                    if (!enemies[i].Colliding)
                    {
                        enemies[i].X += goombaAINum;
                    }                
                }
            }
        }

        /// <summary>
        /// Performs a jump on the enemy
        /// (will add after confirming basic pathfinding)
        /// </summary>
        public void AIJump(Enemy e)
        {
            //check for ground collision
            if (e.JumpsLeft > 0 && e.Velocity.Y >= -4f)
            {
                e.Velocity = new Vector2(e.Velocity.X, e.Velocity.Y + e.JumpVelocity.Y);
            }
            else if (!(e.Velocity.Y >= -4f))
            {
                e.JumpsLeft -= 1;
            }
        }

        public void EnemyIntersection()
        {
            foreach(Enemy e in enemies)
            {
                foreach(Enemy e2 in enemies)
                {
                    
                }
            }
        }

        public void Update(EntityManager em) {
            enemies = em.enemies();
            player = em.player();
        }
    }
}
