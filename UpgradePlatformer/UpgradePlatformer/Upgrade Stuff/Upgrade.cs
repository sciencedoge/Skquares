using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Upgrade_Stuff
{
    //HEADER================================================
    //Names: Sami Chamberlain, Preston Precourt
    //Date: 5/26/2021
    //Purpose: Data capsule for upgrades in the game
    //=======================================================

    /// <summary>
    /// Dictates which type this upgrade is.
    /// </summary>
    public enum UpgradeType
    {
        XtraJump,
        Health,
        Weapon
    }
    class Upgrade
    {
        //Fields
        private int upgradeValue;
        private bool isLearned;
        private bool canBeLearned;
        private int cost;

        private UpgradeType type;

        private Upgrade left;
        private Upgrade right;

        //prop
        
        /// <summary>
        /// Returns or sets the left neighbor
        /// </summary>
        public Upgrade Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// returns the right neighbor
        /// </summary>
        public Upgrade Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// Returns or sets whether or not
        /// this upgrade is learned
        /// </summary>
        public bool IsLearned
        {
            get { return isLearned; }
            set { isLearned = value; }
        }

        /// <summary>
        /// returns or sets whether or not this upgrade can be learned
        /// </summary>
        public bool CanLearn
        {
            get { return canBeLearned; }
            set { canBeLearned = value; }
        }

        /// <summary>
        /// Returns the value of an upgrade
        /// </summary>
        public int Value
        {
            get { return upgradeValue; }
        }

        /// <summary>
        /// Returns the cost of the upgrade
        /// </summary>
        public int Cost
        {
            get { return cost; }
        }

        /// <summary>
        /// returns the type of the upgrade
        /// </summary>
        public UpgradeType Type
        {
            get { return type; }
        }


        //Ctor
        /// <summary>
        /// Creates a new Upgrade object
        /// </summary>
        /// <param name="value">value of the upgrade</param>
        public Upgrade(int value, UpgradeType type, int cost)
        {
            this.upgradeValue = value;
            this.isLearned = false;

            this.type = type;
            this.cost = cost;
        }
    }
}
