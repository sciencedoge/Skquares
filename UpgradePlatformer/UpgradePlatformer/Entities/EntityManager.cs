using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;

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

        //input
        private InputManager inputManager;

        //player and enemies
        private Player player;
        private List<Enemy> enemies;

        public EntityManager()
        {
            inputManager = new InputManager();
        }

        /// <summary>
        /// Performs updates on the entities in the game
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            
            InputEvent e = inputManager.Pop(InputEventKind.KEY_DOWN);
            player.Update(gameTime, (Keys)e.Data);
        }

        /// <summary>
        /// Draws entities to the screen
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public void Draw(SpriteBatch sb, GameTime gt)
        {
            player.Draw(sb, gt);
        }
    }
}
