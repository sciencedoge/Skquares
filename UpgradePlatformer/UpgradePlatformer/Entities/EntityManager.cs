using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Upgrade_Stuff;

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
        private List<Coin> coins;
        private LevelManager levelManager;
        private UpgradeManager upgradeManager;
        private int playerMoney;

        private Level currentLevel;
        private PathfindingAI pathfind;

        /// <summary>
        /// returns player's current money
        /// </summary>
        public int PlayerMoney
        {
            get { return playerMoney; }
        }


        public EntityManager(Texture2D texture, GraphicsDeviceManager device,
            LevelManager levelMan, UpgradeManager upgradeManager)
        {
            enemies = new List<Enemy>();
            coins = new List<Coin>();

            this.levelManager = levelMan;
            this.upgradeManager = upgradeManager;

            this.playerMoney = 0;

            pathfind = new PathfindingAI(enemies, player);
        }

        //methods

        /// <summary>
        /// Updates the entities in the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="eventManager"></param>
        public void Update(GameTime gameTime, EventManager eventManager, InputManager inputManager, LevelManager levelManager)
        {
            currentLevel = levelManager.ActiveLevel();
            // IMPORTANT: Subframes are calculated here
            for (int i = 0; i < 5; i ++) {
                if (player != null) {
                    player.Update(gameTime, eventManager, inputManager, levelManager);
                    Intersects(player, eventManager);
                }

                foreach (Enemy e in enemies)
                {
                    e.Update(gameTime, levelManager, eventManager);
                    Intersects(e, eventManager);
                }

                foreach (Coin c in coins)
                {
                    c.Update();
                    playerMoney += c.Intersects(player);
                }
                if (player != null)
                    player.Intersects(enemies);
                pathfind.UpdateCosts();
                pathfind.MoveToPlayer();
                pathfind.EnemyIntersection();
            }
                

            
            if (player != null) {
                if (player.CurrentHP <= 0)
                {
                    eventManager.Push(new Event("STATE_MACHINE", 2, new Point(0, 0)));
                }
            }                         
        }

        /// <summary>
        /// Draws all entities to the screen
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        /// <param name="spriteBatch">spriteBatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (player != null)
                player.Draw(spriteBatch, gameTime);
            foreach(Enemy e in enemies)
            {
                e.Draw(spriteBatch, gameTime);
            }
            foreach(Coin c in coins)
            {
                c.Draw(spriteBatch, gameTime);
            }
        }
        
        public void Spawn(EntityObject obj, int kind) {
            if (kind == 2)
            {
                player = (Player)obj;
                pathfind.player = player;
            }
            else if (kind == 1)
            {
                enemies.Add((Enemy)obj);
                pathfind.enemies = enemies;
            }
            else if (kind == 0) coins.Add((Coin)obj);
        }

        public void Clean(bool player) {
            if (player)
                this.player = null;
            enemies = new List<Enemy>();
            coins = new List<Coin>();
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
                        if (obj == (LivingObject)player)
                            player.Velocity = new Vector2(0, -4);
                        break;
                    case 101:
                        if (intersection.Height > 0.5 * t.TileSize.Y)
                            obj.TakeDamage(obj.MaxHP);
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
                            if (obj is Enemy)
                            {
                                Enemy e = (Enemy)obj;
                                e.Colliding = true;
                            }
                        }                   

                        obj.X = temp.X;
                        obj.Y = temp.Y;
                        break;
                    case 103:
                        if (obj == (LivingObject)player)
                            em.Push(new Event("WORLD_SHOW", (uint)levelManager.ActiveWorldNum() + 1, new Point(0)));
                        break;
                    case 104:

                        if(player.Y < t.Position.Y)
                        {
                            //checks conditions to move the player up or down
                            if (intersection.Width > intersection.Height - 20)
                            {
                                //short wide rectangle
                                //moves player up
                                if (t.Position.Top - intersection.Top == 0)
                                {
                                    temp.Y -= intersection.Height;
                                    obj.OnFloorCollide();
                                }

                                obj.Velocity = new Vector2(obj.Velocity.X, 0);
                            }
                            obj.Y = temp.Y;
                        }                       
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
            if (player != null)
                return player.CurrentHP;
            return -1;
        }

        public void RespawnPlayer() {
            if (player != null)
                player.Respawn();
        }
        public void FlipPlayerSide(GraphicsDeviceManager graphicsDevice) {
            // if (player != null)
            //     player.Position = new Vector2(graphicsDevice.PreferredBackBufferWidth - (player.Position.X + player.Hitbox.Width), player.Position.Y);
            // player.Velocity = new Vector2(0);
        }

        /// <summary>
        /// returns the max hp of the player
        /// </summary>
        /// <returns></returns>
        public int MaxPlayerHP()
        {
            if (player != null)
                return player.MaxHP;
            return 0;
        }
    }
    class EntityObject { }
}
