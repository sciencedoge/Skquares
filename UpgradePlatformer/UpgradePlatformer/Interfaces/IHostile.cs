using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Interfaces
{
    //HEADER========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Provides functionality for hostility
    //==============================================
    interface IHostile : IGameEntity
    {
        //Properties

        /// <summary>
        /// Gets the amount of damage an
        /// entity can deal
        /// </summary>
        public int Damage { get; }

    }
}
