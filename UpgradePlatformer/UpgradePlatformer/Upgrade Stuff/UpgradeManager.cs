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

        private UpgradeManager() {
            Upgrades = new List<Upgrade>();
        }

        public void Add(int value, UpgradeType type, int price) {
            Upgrade ug = new Upgrade(value, type, price);
            Upgrades.Add(ug);    
        }

        public int GetAmmnt(UpgradeType filter) {
            int result = 0;
            foreach (Upgrade ug in Upgrades)
                result += ug.IsLearned && ug.Type == filter ? ug.Value : 0;
            return result;
        }
    }
}