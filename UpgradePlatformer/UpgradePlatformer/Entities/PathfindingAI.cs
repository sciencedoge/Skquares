using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Levels;

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

        private Random random;

        private Rectangle raycastHitbox;
        private Vector2 rayCastSpeed;


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

            random = new Random();
            
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

                rayCastSpeed = new Vector2(distX / 5, distY / 5);
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

                    if ((int)Enemies[i].X + 1 == (int)player.X
                        || (int)Enemies[i].X - 1 == (int)player.X
                        || (int)Enemies[i].X == (int)player.X
                        && !Enemies[i].Colliding)
                    {
                        Enemies[i].animation.SetFlag(1);
                    }
                    else if (Enemies[i].X > player.X
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

                        if(random.Next(1, 11) == 10)
                        {

                            if (Raycast(Enemies[i]))
                            {
                                while (Enemies[i].Velocity.Y > -4f
                                && Enemies[i].JumpsLeft > 0)
                                {

                                    AIJump(Enemies[i]);
                                }
                            }
                                
                            Enemies[i].JumpsLeft -= 1;
                        }                                           
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

                    if (random.Next(1, 21) == 20)
                    {
                        
                        while (Enemies[i].Velocity.Y > -4f
                            && Enemies[i].JumpsLeft > 0)
                        {
                            AIJump(Enemies[i]);
                        }

                        Enemies[i].JumpsLeft -= 1;
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
        }

        /// <summary>
        /// updates the entitys
        /// </summary>
        public void Update() {
            Enemies = EntityManager.Instance.Enemies();
            player = EntityManager.Instance.Player();
        }

        /// <summary>
        /// Performs a raycast calc on the enemy and player
        /// </summary>
        /// <param name="enemy">The enemy being examined</param>
        /// <returns></returns>
        public bool Raycast(Enemy enemy)
        {
            raycastHitbox = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 25, 25);
            rayCastSpeed = (player.Position - enemy.Position) / 20;

            while (!raycastHitbox.Intersects(player.Hitbox))
            {
                raycastHitbox.X += (int)rayCastSpeed.X;
                raycastHitbox.Y += (int)rayCastSpeed.Y;

                foreach(Tile t in LevelManager.Instance.ActiveLevel().Tiles)
                {
                    if(t.CollisionKind != 9)
                    {
                        if (raycastHitbox.Intersects(t.Position))
                        {
                            return false;
                        }
                        else if (raycastHitbox.X < 0 || raycastHitbox.X > Sprite.graphics.PreferredBackBufferWidth
                            || raycastHitbox.Y < 0 || raycastHitbox.Y > Sprite.graphics.PreferredBackBufferHeight)
                        {
                            return false;
                        }
                    }                   
                }              
            }

            return true;
        }
    }
}
