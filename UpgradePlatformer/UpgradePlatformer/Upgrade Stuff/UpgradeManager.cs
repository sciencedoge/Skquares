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
    public class UpgradeManager
    {
        private static readonly Lazy<UpgradeManager>
            lazy =
            new Lazy<UpgradeManager>
                (() => new UpgradeManager());
        public static UpgradeManager Instance { get { return lazy.Value; } }

        public List<Upgrade> Upgrades;

        private UpgradeManager() { }
    }
}