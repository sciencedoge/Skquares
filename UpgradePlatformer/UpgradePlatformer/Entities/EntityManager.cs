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

        //input
        private InputManager inputManager;

        //player and enemies
        private Player player;
        private List<Enemy> enemies;

        //list of tiles



        public EntityManager()
        {
            inputManager = new InputManager();
        }
    }
}
