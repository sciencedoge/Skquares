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
        public UpgradeManager() { }


        public void Add(int value, UPGRADE_TYPE type)
        {
            if(root == null)
            {
                this.root = new Upgrade(value, type);
            }
            else
            {
                Add(root, value, type);
            }

        }

        /// <summary>
        /// Provides functionality for adding upgrades to the tree
        /// </summary>
        /// <param name="node">parent node</param>
        /// <param name="value">value of the new node</param>
        /// <param name="type">new node type</param>
        private void Add(Upgrade upgrade, int value, UPGRADE_TYPE type)
        {
            //Health node, all stored to the LEFT of the tree
            if(type == UPGRADE_TYPE.Health)
            {
                if (upgrade.Left != null)
                {
                    upgrade.Left = new Upgrade(value, type);
                }
                else
                {
                    Add(upgrade.Left, value, type);
                }
            }

            //Speed node, stored to the RIGHT of the tree
            else if(type == UPGRADE_TYPE.XtraJump)
            {
                if(upgrade.Right != null)
                {
                    upgrade.Right = new Upgrade(value, type);
                }
                else
                {
                    Add(upgrade.Right, value, type);
                }
            }
        }


    }
}
