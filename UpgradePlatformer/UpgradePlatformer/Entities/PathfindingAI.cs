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
        private readonly float speed;
        public Player player;
        public List<Enemy> _enemies;
        public List<Enemy> Enemies { get => _enemies; set { _enemies = value; relationships = new Vector2[Enemies.Count, 1];} }
        private float goombaAINum;
        private Vector2[,] relationships;


        //Constructor

        /// <summary>
        /// Creates a new PathFindingAI object
        /// </summary>
        public PathfindingAI(List<Enemy> enemies, Player player)
        {
            this.Enemies = enemies;
            this.player = player;
            speed = 3f;
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
            for (int i = 0; i < Enemies.Count; i++)
            {
                float distX = Math.Abs(Enemies[i].Position.X - player.Position.X);
                float distY = Math.Abs(Enemies[i].Position.Y - player.Position.Y);
                relationships[i, 0] = new Vector2(distX, distY);
            }
        }

        /// <summary>
        /// moves the enemy to the player
        /// </summary>
        public void MoveToPlayer()
        {
            if (player == null) return;
            for(int i = 0; i < Enemies.Count; i++)
            {
                if (relationships[i, 0].X < 150)
                {
                    Enemies[i].animation.SetFlag(2);
                    if (Enemies[i].X > player.X
                        && !Enemies[i].Colliding)
                    {
                        Enemies[i].animation.SetFlag(0);
                        Enemies[i].X -= speed;
                        Enemies[i].Flip = false;
                    }
                    else if(Enemies[i].X < player.X
                        && !Enemies[i].Colliding)
                    {
                        Enemies[i].animation.SetFlag(1);
                        Enemies[i].X += speed;
                        Enemies[i].Flip = true;
                    }
                    else
                    {
                        Enemies[i].Colliding = false;
                    }

                    if (relationships[i, 0].Y > 20
                        && player.Y < Enemies[i].Y)
                    {
                        AIJump(Enemies[i]);                       
                    }
                }

                else
                {
                    Enemies[i].animation.SetFlag(3);
                    Enemies[i].Flip = goombaAINum > 0;
                    if(Enemies[i].Colliding)
                    {
                        if (goombaAINum > 0)
                            Enemies[i].animation.SetFlag(1);
                        else
                            Enemies[i].animation.SetFlag(0);
                        goombaAINum *= -1;
                        Enemies[i].X += goombaAINum;
                        Enemies[i].Colliding = false;
                    }

                    if (!Enemies[i].Colliding)
                    {
                        Enemies[i].X += goombaAINum;
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

        /// <summary>
        /// processes enemy intersections
        /// </summary>
        public void EnemyIntersection()
        {
            foreach(Enemy e in Enemies)
            {
                foreach(Enemy e2 in Enemies)
                {
                    
                }
            }
        }

        /// <summary>
        /// updates the entitys
        /// </summary>
        public void Update() {
            Enemies = EntityManager.Instance.Enemies();
            player = EntityManager.Instance.Player();
        }
    }
}
