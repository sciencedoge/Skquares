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

        //list of tiles



        public EntityManager(Texture2D texture, GraphicsDeviceManager device,
            LevelManager levelMan)
        {
            player = new Player(10, 2, 
                new Rectangle(new Point(50, 600), new Point(25, 25)), texture, device);

            this.levelManager = levelMan;
        }

        //methods

        /// <summary>
        /// Updates the entities in the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="inputManager"></param>
        public void Update(GameTime gameTime, InputManager inputManager)
        {
            currentLevel = levelManager.ActiveLevel();
            // IMPORTANT: Subframes are calculated here
            for (int i = 0; i < 5; i ++) {
                player.Update(gameTime, inputManager);
                Intersects();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Checks if an entity intersects with anything
        /// </summary>
        public void Intersects()
        {
            Rectangle temp = GetTempHitbox();

            foreach (Tile t in currentLevel.Tiles)
            {              
                if (t.Position.Intersects(temp)
                    && t.Kind != 9)
                {
                    //Gets a rectangle that represents the intersection
                    Rectangle intersection = Rectangle.Intersect(t.Position, temp);

                    //checks conditions to move the player up or down
                    if (intersection.Width > intersection.Height)
                    {
                        //short wide rectangle
                        //moves player up
                        if (t.Position.Top - intersection.Top == 0)
                        {
                            temp.Y -= intersection.Height;
                        }

                        //moves player down
                        else if (t.Position.Top - intersection.Top != 0)
                        {
                            temp.Y += intersection.Height;
                        }
                        player.OnFloorCollide();
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                    }

                    //long skinny rectangle (left or right)
                    else if (intersection.Width < intersection.Height)
                    {
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

                        player.Velocity = new Vector2(0, player.Velocity.Y);
                    }

                    player.X = temp.X;
                    player.Y = temp.Y;
                }               
            }
        }

        /// <summary>
        /// returns a temp hitbox
        /// </summary>
        /// <returns></returns>
        public Rectangle GetTempHitbox()
        {
            return new Rectangle(
                new Point(player.Hitbox.X,
                player.Hitbox.Y),
                new Point(player.Hitbox.Width,
                player.Hitbox.Height));
        }
    }
}
