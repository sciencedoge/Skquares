using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Upgrade_Stuff
{
    //HEADER=========================================
    //Name: Sami Chamberlain 
    //Date: 5/26/2021
    //Purpose: establishes the upgrade tree
    //===============================================
    static class UpgradeStructure
    {
        public static void InitStructure() {
            UpgradeManager.Instance.Add(1, UpgradeType.XtraJump, 10);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 10);
            UpgradeManager.Instance.Add(1, UpgradeType.Weapon, 20);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 30);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 30);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 40);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 40);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 50);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 60);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 70);
            UpgradeManager.Instance.Add(1, UpgradeType.Health, 80);

        }
    }
}
