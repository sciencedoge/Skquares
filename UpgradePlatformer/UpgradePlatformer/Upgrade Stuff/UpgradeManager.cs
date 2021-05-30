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
        private static readonly Lazy<UpgradeManager>
            lazy =
            new Lazy<UpgradeManager>
                (() => new UpgradeManager());
        public static UpgradeManager Instance { get { return lazy.Value; } }
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
            root = null;
            possibleUpgrades = new List<Upgrade>();
        }

        /// <summary>
        /// Adds a node to the tree
        /// </summary>
        /// <param name="value">value of the upgrade</param>
        /// <param name="type">type of the upgrade</param>
        public void Add(int value, UpgradeType type, int cost)
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
        private void Add(Upgrade upgrade, int value, UpgradeType type, int cost)
        {
            //Health node, all stored to the LEFT of the tree
            if(type == UpgradeType.HEALTH)
            {
                if (upgrade.Left == null)
                {
                    upgrade.Left = new Upgrade(value, type, cost);
                }
                else
                {
                    Add(upgrade.Left, value, type, cost);
                }
            }

            //Jump node, stored to the RIGHT of the tree
            else if(type == UpgradeType.EXTRA_JUMP)
            {
                if(upgrade.Right == null)
                {
                    upgrade.Right = new Upgrade(value, type, cost);
                }
                else
                {
                    Add(upgrade.Right, value, type, cost);
                }
            }

            //Speed node, stored to the RIGHT of the tree
            else if (type == UpgradeType.WEAPON)
            {
                if (upgrade.Right == null)
                {
                    upgrade.Right = new Upgrade(value, type, cost);
                }
                else
                {
                    Add(upgrade.Right, value, type, cost);
                }
            }
        }

        public int GetAmmnt(UpgradeType Type, Upgrade upgrade = null) {
            if (upgrade == null)
                upgrade = Root;
            //LRC Data pattern
            int result = 0;

            if (upgrade.IsLearned)
            {
                if (upgrade.Type == Type)
                    result += 1;

                if (upgrade.Left != null)
                {
                    result += GetAmmnt(Type, upgrade.Left);
                }

                if (upgrade.Right != null)
                {
                    result += GetAmmnt(Type, upgrade.Right);
                }
            }

            return result;
        }

        /// <summary>
        /// Lists all of the upgrades
        /// purchased by the player
        /// </summary>
        public List<Upgrade> CanBeLearned(Upgrade upgrade = null)
        { 
            if (upgrade == null) 
                upgrade = Root;
            //LRC Data pattern
            List<Upgrade> result = new List<Upgrade>();

            if (upgrade.IsLearned)
            {
                if(upgrade.Left != null)
                {
                    result.AddRange(CanBeLearned(upgrade.Left));
                }
                
                if(upgrade.Right != null)
                {
                    result.AddRange(CanBeLearned(upgrade.Right));
                }
            } else {
                return new List<Upgrade>{upgrade};
            }

            return result;
        }
    }
}
