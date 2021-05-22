using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UpgradePlatformer.Interfaces
{
    //HEADER========================================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Provides the groundwork for all game related entities
    //==============================================================
    interface IGameEntity
    {
        //Properties

        /// <summary>
        /// returns whether or not an entity
        /// is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// gets or sets the current HP of the
        /// entity
        /// </summary>
        public int CurrentHP { get; set; }

        /// <summary>
        /// gets or sets the max HP of the
        /// entity
        /// </summary>
        public int MaxHP { get; set; }

        /// <summary>
        /// Returns the hitbox of an entity
        /// </summary>
        public Rectangle Hitbox { get; }

        //Methods

        /// <summary>
        /// Draws an entity to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public void Draw(SpriteBatch sb, GameTime gt);

        /// <summary>
        /// Updates elements of 
        /// </summary>
        /// <param name="gt"></param>
        public void Update(GameTime gt);

    }
}
