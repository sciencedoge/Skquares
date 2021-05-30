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
            UpgradeManager.Instance.Add(1, UpgradeType.EXTRA_JUMP, 10);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 10);
            UpgradeManager.Instance.Add(1, UpgradeType.WEAPON, 20);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 30);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 30);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 40);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 40);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 50);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 60);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 70);
            UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 80);

        }
    }
}
