using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;

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

        //list of tiles



        public EntityManager(Texture2D texture, GraphicsDeviceManager device)
        {
            player = new Player(10, 2, 
                new Rectangle(new Point(50, 600), new Point(40, 40)), texture, device);
        }

        //methods

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            player.Update(gameTime, inputManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch, gameTime);
        }
    }
}
