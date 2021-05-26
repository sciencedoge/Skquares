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
    public enum UPGRADE_TYPE
    {
        XtraJump,
        Health
    }
    class Upgrade
    {
        //Fields
        private int upgradeValue;
        private bool isLearned;

        private UPGRADE_TYPE type;

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
        public bool IsLeanred
        {
            get { return isLearned; }
            set { isLearned = value; }
        }

        /// <summary>
        /// Returns the value of an upgrade
        /// </summary>
        public int Value
        {
            get { return upgradeValue; }
        }


        //Ctor
        /// <summary>
        /// Creates a new Upgrade object
        /// </summary>
        /// <param name="value">value of the upgrade</param>
        public Upgrade(int value, UPGRADE_TYPE type)
        {
            this.upgradeValue = value;
            this.isLearned = false;

            this.type = type;
        }

        //Methods
    }
}
