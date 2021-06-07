using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.UI;

namespace UpgradePlatformer.Upgrade_Stuff
{
    //HEADER=========================================
    //Name: Sami Chamberlain 
    //Date: 5/26/2021
    //Purpose: establishes the upgrade tree
    //===============================================
    public static class UpgradeStructure
    {
        public static UIPanel panel;
        public static void InitStructure()
        {
            ShopManager.Instance.Root = null;
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 10);
            ShopManager.Instance.Add(1, UpgradeType.EXTRA_JUMP, 10);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 10);
            ShopManager.Instance.Add(1, UpgradeType.WEAPON, 0);
            ShopManager.Instance.Add(1, UpgradeType.EXTRA_JUMP, 0);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 30);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 30);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 40);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 40);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 50);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 60);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 70);
            ShopManager.Instance.Add(1, UpgradeType.HEALTH, 80);
        }
        public static void ShowMessage(Upgrade ug)
        {
            panel.Text.Text = ug.UnlockText;
            panel.show(360);
        }
    }
}
