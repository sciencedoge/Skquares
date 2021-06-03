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
        /// <summary>
        /// Singleton stuff
        /// </summary>
        private static readonly Lazy<EntityManager>
            lazy =
            new Lazy<EntityManager>
            (() => new EntityManager());
        public static EntityManager Instance { get { return lazy.Value; } }

        //Fields

        //player and enemies
        private List<EntityObject> objects;
        private int playerMoney;
        private Level currentLevel;
        private readonly PathfindingAI pathfind;

        //methods

        /// <summary>
        /// gets total number of entities
        /// </summary>
        /// <returns></returns>
        public int Count()
        {

            int result = 0;

            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                result++;
            }

            return result;
        }

        /// <summary>
        /// gets all the enemys in the manager
        /// </summary>
        /// <returns>the enemys</returns>
        public List<Enemy> Enemies()
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

        /// <summary>
        /// gets all the coins in the manager
        /// </summary>
        /// <returns>the coins</returns>
        public List<Coin> Coins()
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

        /// <summary>
        /// gets the player in the manager
        /// </summary>
        /// <returns>the player</returns>
        public Player Player()
        {
            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                
                if (obj.Kind == EntityKind.PLAYER)
                    return (Player)obj;
            }
            return null;
        }

        /// <summary>
        /// returns player's current money
        /// </summary>
        public int PlayerMoney
        {
            get { return playerMoney; }
            set { playerMoney = value; }
        }

        /// <summary>
        /// creates the entity manager
        /// </summary>
        public EntityManager()
        {
            objects = new List<EntityObject>();
            pathfind = new PathfindingAI(Enemies(), Player());
        }

        /// <summary>
        /// Updates the entities in the game
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            currentLevel = LevelManager.Instance.ActiveLevel();

            pathfind.Update();
            pathfind.UpdateCosts();
            pathfind.MoveToPlayer();
            // IMPORTANT: Subframes are calculated here
            for (int i = 0; i < 5; i ++)
            {
                foreach (EntityObject obj in objects)
                {
                    if (obj == null) continue;
                    if (obj.IsActive == false) continue;
                    if(obj is Enemy)
                    {
                        obj.Update(gameTime);               
                    }
                    else
                    {
                        obj.Update(gameTime);
                        if (obj is LivingObject @object)
                            @object.animation.Update(gameTime);
                    }

                    Intersects(obj);

                    int gainedMoney = obj.Intersects(objects);

                    if (gainedMoney > 0)
                    {
                        SoundManager.Instance.PlaySFX("coin");
                        playerMoney += gainedMoney;
                    } else if (gainedMoney < 0)
                    {
                        playerMoney += gainedMoney;
                    }
                }
                if (Player() != null)
                    if (Player().CurrentHP <= 0)
                    {
                        SoundManager.Instance.PlayMusic("gameover");
                        EventManager.Instance.Push(new Event("SAVE", 0, new Point(0)));
                        EventManager.Instance.Push(new Event("STATE_MACHINE", 2, new Point(0)));                      
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
            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                obj.Draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// draws the lightmap from entities
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLightMap(SpriteBatch spriteBatch)
        {
            foreach (EntityObject obj in objects)
            {
                if (obj == null) continue;
                obj.DrawLightMap(spriteBatch);
            }
        }

        /// <summary>
        /// spawns an entity object
        /// </summary>
        /// <param name="obj">the object</param>
        public void Spawn(EntityObject obj) {
            if (obj != null)
                objects.Add(obj);
        }

        /// <summary>
        /// removes all entity objects
        /// </summary>
        /// <param name="player">wether the player should be removed</param>
        public void Clean(bool player)
        {
            EntityObject plyr = (EntityObject)this.Player();
            objects = new List<EntityObject>();

            if (!player)
                objects.Add(plyr);
        }

        /// <summary>
        /// cleans null and dead entities
        /// </summary>
        /// <param name="player"></param>
        public void Cleanup(GameTime gametime)
        {
            objects.RemoveAll((o) => o == null);
            objects.RemoveAll((o) => !o.IsActive && !(o is Player));
        }

        /// <summary>
        /// checks for intersections between all objects and a single object
        /// </summary>
        /// <param name="o">the object to check</param>
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
                        if (obj == (LivingObject)Player()) {
                            Player().Velocity = new Vector2(0, -4);
                            Player().JumpsLeft = 0;
                        }
                        break;
                    case 101:
                        if (intersection.Height > 0.5 * t.TileSize.Y)
                        {
                            obj.CurrentHP = 0;
                            obj.IsActive = false;
                        }                           
                        break;
                    case 102:
                        RegularCollision(intersection, temp, obj, t);
                        break;
                    case 103:
                        // goal tile
                        if (obj == (LivingObject)Player())
                            EventManager.Instance.Push(new Event("WORLD_SHOW", (uint)LevelManager.Instance.ActiveWorldNum() + 1, new Point(0)));
                        break;
                    case 104:
                        if (obj is Enemy)
                        {
                            RegularCollision(intersection, temp, obj, t);
                        }
                        else
                        {
                            if (obj.Y < t.Position.Y) 
                            {
                                //checks conditions to move the player up or down
                                if (intersection.Width > intersection.Height)
                                {
                                    //short wide rectangle
                                    //moves player up
                                    if (t.Position.Top - intersection.Top == 0)
                                    {
                                        temp.Y -= intersection.Height;
                                        if (!Player().Landed && !Player().Ducking)
                                        {
                                            Player().Landed = true;
                                            SoundManager.Instance.PlaySFX("land");
                                        }
                                        obj.OnFloorCollide();
                                    }

                                    obj.Velocity = new Vector2(obj.Velocity.X, 0);
                                }
                                obj.Y = temp.Y;
                            }
                        }                       
                        break;
                    case 105:
                        if (obj is Enemy)
                            RegularCollision(intersection, temp, obj, t);
                        break;
                }
            }
        }

        /// <summary>
        /// gets a hitbox from an object
        /// </summary>
        /// <param name="obj">the object</param>
        /// <returns>the hitbox</returns>
        public Rectangle GetTempHitbox(LivingObject obj)
        {
            return new Rectangle(
                new Point((int)obj.X,
                (int)obj.Y),
                new Point(obj.Hitbox.Width,
                obj.Hitbox.Height));
        }

        /// <summary>
        /// gets the current player hp
        /// </summary>
        /// <returns>the players hp</returns>
        public int GetPlayerHp() {
            if (Player() != null)
                return Player().CurrentHP;
            return -1;
        }

        /// <summary>
        /// respawns the player
        /// </summary>
        public void RespawnPlayer() {
            if (Player() != null)
                Player().Respawn();
        }

        /// <summary>
        /// returns the max hp of the player
        /// </summary>
        /// <returns>the max hp</returns>
        public int MaxPlayerHP()
        {
            if (Player() != null)
                return Player().MaxHP;
            return 0;
        }

        /// <summary>
        /// Regular collision handling
        /// </summary>
        /// <param name="intersection"></param>
        /// <param name="temp"></param>
        /// <param name="obj"></param>
        public void RegularCollision(Rectangle intersection, Rectangle temp, EntityObject obj, Tile t)
        {
            LivingObject obj2 = (LivingObject)obj;

            //checks conditions to move the player up or down
            if (intersection.Width > intersection.Height)
            {
                //short wide rectangle
                //moves player up
                if (t.Position.Top - intersection.Top == 0)
                {
                    temp.Y -= intersection.Height;
                    if (obj2 is Player player)
                    {
                        if (!player.Landed && !Player().Ducking)
                        {
                            player.Landed = true;

                            SoundManager.Instance.PlaySFX("land");
                        }
                       
                    }

                    obj2.OnFloorCollide();
                }

                //moves player down
                else if (t.Position.Top - intersection.Top != 0)
                {
                    temp.Y += intersection.Height;
                }
                obj2.Velocity = new Vector2(obj2.Velocity.X, 0);
            }

            //long skinny rectangle (left or right)
            else if (intersection.Width < intersection.Height)
            {
                obj2.Velocity = new Vector2(0, obj2.Velocity.Y);

                //moves the player right
                if (t.Position.Right - intersection.Right == 0)
                {
                    temp.X += intersection.Width + 1;
                    temp.Y += 2;
                    
                }
                //moves the player left
                else
                {
                    temp.X -= intersection.Width;
                }

                //marks true because the enemy is colliding
                //with a wall
                if (obj is Enemy e)
                {
                    e.Colliding = true;
                }
            }       
         
            obj2.X = temp.X;
            obj2.Y = temp.Y;
        }
    }
}
