using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Upgrade_Stuff
{
    //HEADER=======================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/26/2021
    //Purpose: Manages upgrades
    //============================================
    class UpgradeManager
    {
        private Upgrade root;

        private List<Upgrade> possibleUpgrades;

        /// <summary>
        /// returns the root upgrade
        /// </summary>
        public Upgrade Root
        {
            get { return root; }
        }


        /// <summary>
        /// Creates a new UpgradeManager class
        /// </summary>
        public UpgradeManager() 
        {
            possibleUpgrades = new List<Upgrade>();
        }

        /// <summary>
        /// Adds a node to the tree
        /// </summary>
        /// <param name="value">value of the upgrade</param>
        /// <param name="type">type of the upgrade</param>
        public void Add(int value, UPGRADE_TYPE type, int cost)
        {
            if(root == null)
            {
                this.root = new Upgrade(value, type, cost);
            }
            else
            {
                Add(root, value, type, cost);
            }

        }

        /// <summary>
        /// Provides functionality for adding upgrades to the tree
        /// </summary>
        /// <param name="node">parent node</param>
        /// <param name="value">value of the new node</param>
        /// <param name="type">new node type</param>
        private void Add(Upgrade upgrade, int value, UPGRADE_TYPE type, int cost)
        {
            //Health node, all stored to the LEFT of the tree
            if(type == UPGRADE_TYPE.Health)
            {
                if (upgrade.Left != null)
                {
                    upgrade.Left = new Upgrade(value, type, cost);
                }
                else
                {
                    Add(upgrade.Left, value, type, cost);
                }
            }

            //Speed node, stored to the RIGHT of the tree
            else if(type == UPGRADE_TYPE.XtraJump)
            {
                if(upgrade.Right != null)
                {
                    upgrade.Right = new Upgrade(value, type, cost);
                }
                else
                {
                    Add(upgrade.Right, value, type, cost);
                }
            }
        }

        /// <summary>
        /// resets the possible upgrades list
        /// </summary>
        public void EstablishPossibleList()
        {
            possibleUpgrades = new List<Upgrade>();
        }

        /// <summary>
        /// Lists all of the upgrades
        /// purchased by the player
        /// </summary>
        public List<Upgrade> CanBeLearned(Upgrade upgrade)
        {
            //LCR Data pattern

            if (upgrade.IsLearned)
            {
                if(upgrade.Left != null)
                {
                    CanBeLearned(upgrade.Left);
                }
                
                if(upgrade.Right != null)
                {
                    CanBeLearned(upgrade.Right);
                }

                possibleUpgrades.Add(upgrade);

            }

            return new List<Upgrade>();
        }


    }
}
