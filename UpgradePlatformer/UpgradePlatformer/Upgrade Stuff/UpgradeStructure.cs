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
    class UpgradeStructure
    {
        public UpgradeStructure(UpgradeManager manager)
        {
            manager.Add(1, UPGRADE_TYPE.XtraJump, 10);
            manager.Add(1, UPGRADE_TYPE.Health, 10);
            manager.Add(1, UPGRADE_TYPE.Weapon, 20);
            manager.Add(1, UPGRADE_TYPE.Health, 30);
            manager.Add(1, UPGRADE_TYPE.Health, 30);
            manager.Add(1, UPGRADE_TYPE.Health, 40);
            manager.Add(1, UPGRADE_TYPE.Health, 40);
            manager.Add(1, UPGRADE_TYPE.Health, 50);
            manager.Add(1, UPGRADE_TYPE.Health, 60);
            manager.Add(1, UPGRADE_TYPE.Health, 70);
            manager.Add(1, UPGRADE_TYPE.Health, 80);

        }
    }
}
