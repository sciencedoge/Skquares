using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Upgrade_Stuff;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
    //HEADER===========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Manages all of the entities in the game
    //=================================================
    class EntityManager
    {
        private static readonly Lazy<EntityManager>
            lazy =
            new Lazy<EntityManager>
                (() => new EntityManager());
        public static EntityManager Instance { get { return lazy.Value; } }

        //Fields

        //player and enemies
        private List<EntityObject> objects;

        public List<Enemy> enemies()
        {
            List<Enemy> result = new List<Enemy>();

            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                if (obj.Kind == EntityKind.ENEMY)
                    result.Add((Enemy)obj);
            }

            return result;
        }
        public List<Coin> coins()
        {
            List<Coin> result = new List<Coin>();

            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                if (obj.Kind == EntityKind.COIN)
                    result.Add((Coin)obj);
            }

            return result;
        }
        public Player player()
        {
            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                if (obj.Kind == EntityKind.PLAYER)
                    return (Player)obj;
            }
            return null;
        }
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


        public EntityManager()
        {
            this.objects = new List<EntityObject>();

            objects.Add((EntityObject)new UpgradeEntity(10, new Rectangle(100, 300, 25, 25), UpgradeManager.Instance.Root));

            this.playerMoney = 0;

            pathfind = new PathfindingAI(enemies(), player());
        }

        //methods

        /// <summary>
        /// Updates the entities in the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="eventManager"></param>
        public void Update(GameTime gameTime)
        {
            currentLevel = LevelManager.Instance.ActiveLevel();
            // IMPORTANT: Subframes are calculated here
            for (int i = 0; i < 5; i ++) {
                foreach (EntityObject obj in objects)
                {
                    if (obj == null) continue;
                    obj.Update(gameTime);
                    Intersects(obj);
                    int gainedMoney = obj.Intersects(objects);
                    
                    if(gainedMoney > 0)
                    {
                        SoundManager.Instance.PlaySFX("coin");
                        playerMoney += gainedMoney;
                    }

                    
                }
                if (player() != null)
                    if (player().CurrentHP <= 0)
                    {
                        SoundManager.Instance.PlayMusic("gameover");
                        EventManager.Instance.Push(new Event("STATE_MACHINE", 2, new Point(0)));                      
                    }

                        
                pathfind.Update(this);
                pathfind.UpdateCosts();
                pathfind.MoveToPlayer();
                pathfind.EnemyIntersection();
            }                   
        }

        /// <summary>
        /// Draws all entities to the screen
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        /// <param name="spriteBatch">spriteBatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                obj.Draw(spriteBatch, gameTime);
            }
        }
        
        public void Spawn(EntityObject obj) {
            if (obj != null)
                objects.Add(obj);
        }

        public void Clean(bool player) {
            EntityObject plyr = (EntityObject)this.player();
            objects = new List<EntityObject>();
            if (!player)
                objects.Add(plyr);
        }

        /// <summary>
        /// Checks if an entity intersects with anything
        /// </summary>
        public void Intersects(EntityObject o)
        {
            if (!new List<EntityKind> { EntityKind.PLAYER, EntityKind.ENEMY}.Contains(o.Kind))
                return;
            LivingObject obj = (LivingObject)o;
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
                        if (obj == (LivingObject)player()) {
                            player().Velocity = new Vector2(0, -4);
                            player().JumpsLeft = 0;
                        }
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
                                if(obj.JumpsLeft == 0)
                                {
                                    SoundManager.Instance.PlaySFX("land");
                                }
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
                        if (obj == (LivingObject)player())
                            EventManager.Instance.Push(new Event("WORLD_SHOW", (uint)LevelManager.Instance.ActiveWorldNum() + 1, new Point(0)));
                        break;
                    case 104:

                        if(player().Y < t.Position.Y)
                        {
                            //checks conditions to move the player up or down
                            if (intersection.Width > intersection.Height - 20)
                            {
                                //short wide rectangle
                                //moves player up
                                if (t.Position.Top - intersection.Top == 0)
                                {
                                    temp.Y -= intersection.Height;
                                    if (obj.JumpsLeft == 0)
                                    {
                                        SoundManager.Instance.PlaySFX("land");
                                    }
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
            if (player() != null)
                return player().CurrentHP;
            return -1;
        }

        public void RespawnPlayer() {
            if (player() != null)
                player().Respawn();
        }

        /// <summary>
        /// returns the max hp of the player
        /// </summary>
        /// <returns></returns>
        public int MaxPlayerHP()
        {
            if (player() != null)
                return player().MaxHP;
            return 0;
        }
    }
}
