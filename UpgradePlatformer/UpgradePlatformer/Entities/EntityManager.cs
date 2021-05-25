using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Entities
{
    //HEADER===========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Manages all of the entities in the game
    //=================================================
    class EntityManager
    {
        //Fields

        //player and enemies
        private Player player;
        private List<Enemy> enemies;
        private GraphicsDeviceManager device;
        private LevelManager levelManager;

        private Level currentLevel;
        private PathfindingAI pathfind;

        //list of tiles



        public EntityManager(Texture2D texture, GraphicsDeviceManager device,
            LevelManager levelMan)
        {
            enemies = new List<Enemy>();

            player = new Player(10, 2, 
                new Rectangle(new Point(50, 600), new Point(25, 25)), texture, device, 2);

            // enemies.Add(new Enemy(
            //     10, 1, new Rectangle(new Point(100, 600), new Point(25, 25)), texture, device, 1));

            // enemies.Add(new Enemy(
            //     10, 1, new Rectangle(new Point(500, 600), new Point(25, 25)), texture, device, 1));

            this.levelManager = levelMan;

            pathfind = new PathfindingAI(enemies, player);
        }

        //methods

        /// <summary>
        /// Updates the entities in the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="eventManager"></param>
        public void Update(GameTime gameTime, EventManager eventManager, InputManager inputManager)
        {
            currentLevel = levelManager.ActiveLevel();
            // IMPORTANT: Subframes are calculated here
            for (int i = 0; i < 5; i ++) {
                player.Update(gameTime, eventManager, inputManager);
                Intersects(player, eventManager);
                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime);
                    Intersects(e, eventManager);
                }
               
                player.Intersects(enemies);
                pathfind.UpdateCosts();
                pathfind.MoveToPlayer();
                pathfind.EnemyIntersection();
            }          
            if (player.CurrentHP == 0)
            {
                eventManager.Push(new Event("STATE_MACHINE", 2, new Point(0, 0)));
            }                         
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch, gameTime);
            foreach(Enemy e in enemies)
            {
                e.Draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// Checks if an entity intersects with anything
        /// </summary>
        public void Intersects(LivingObject obj, EventManager em)
        {
            if (!obj.IsActive)
                return;
            Rectangle temp = GetTempHitbox(obj);

            foreach (Tile t in currentLevel.GetCollisions(temp, 4))
            {
                //Gets a rectangle that represents the intersection
                Rectangle intersection = Rectangle.Intersect(t.Position, temp);
                
                switch (t.CollisionKind) {
                    case 100:
                        obj.TakeDamage(1);
                        break;
                    case 101:
                        if (intersection.Height > 0.5 * t.TileSize.Y)
                            obj.TakeDamage(1);
                        break;
                    case 102:
                        //checks conditions to move the player up or down
                        if (intersection.Width > intersection.Height)
                        {
                            //short wide rectangle
                            //moves player up
                            if (t.Position.Top - intersection.Top == 0)
                            {
                                temp.Y -= intersection.Height;
                                obj.OnFloorCollide();
                            }

                            //moves player down
                            else if (t.Position.Top - intersection.Top != 0)
                            {
                                temp.Y += intersection.Height;
                            }
                            obj.Velocity = new Vector2(obj.Velocity.X, 0);
                        }

                        //long skinny rectangle (left or right)
                        else if (intersection.Width < intersection.Height)
                        {
                            obj.Velocity = new Vector2(0, obj.Velocity.Y);

                            //moves the player right
                            if (t.Position.Right - intersection.Right == 0)
                            {
                                temp.X += intersection.Width;
                            }
                            //moves the player left
                            else
                            {
                                temp.X -= intersection.Width;
                            }       
                            
                            //marks true because the enemy is colliding
                            //with a wall
                            if(obj is Enemy)
                            {
                                Enemy e = (Enemy)obj;

                                e.Colliding = true;
                            }
                        }

                        obj.X = temp.X;
                        obj.Y = temp.Y;
                        break;
                    case 103:
                        em.Push(new Event("LEVEL_NEXT", 1, new Point(0)));
                        break;
                }
            }
        }

        /// <summary>
        /// returns a temp hitbox
        /// </summary>
        /// <returns></returns>
        public Rectangle GetTempHitbox(LivingObject obj)
        {
            return new Rectangle(
                new Point(obj.Hitbox.X,
                obj.Hitbox.Y),
                new Point(obj.Hitbox.Width,
                obj.Hitbox.Height));
        }

        public int GetPlayerHp() {
            return player.CurrentHP;
        }

        public void RespawnPlayer() {
            player.Respawn();
        }
    }
}
