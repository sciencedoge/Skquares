using System.Collections.Generic;
using UpgradePlatformer.Menus.HatShop;
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
      UpgradeManager.Instance.Upgrades = new List<Upgrade>();
      UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 10);
      UpgradeManager.Instance.Add(1, UpgradeType.EXTRA_JUMP, 10);
      UpgradeManager.Instance.Add(1, UpgradeType.WEAPON, 0);
      UpgradeManager.Instance.Add(1, UpgradeType.EXTRA_JUMP, 0);
      UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 10);
      UpgradeManager.Instance.Add(1, UpgradeType.HEALTH, 10);

      ShopManager.Instance.Add(new Hat(0, UpgradeType.HEALTH, 100, "OH LAWD HE COMIN"), -1);
      ShopManager.Instance.Add(new Hat(1, UpgradeType.HEALTH, 100, "OH LAWD HE COMIN"), -1);
      ShopManager.Instance.Add(new Hat(2, UpgradeType.HEALTH, 100, "OH LAWD HE COMIN"), -1);
      ShopManager.Instance.Add(new Hat(3, UpgradeType.HEALTH, 100, "OH LAWD HE COMIN"), -1);
    }

    public static void ShowMessage(Upgrade ug)
    {
      panel.Text.Text = ug.UnlockText;
      panel.show(360);
    }
  }
}
