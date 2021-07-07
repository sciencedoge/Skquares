using System;
using System.Collections.Generic;

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

    public void Add(int value, UpgradeType type, int price)
    {
      Upgrade ug = new Upgrade(value, type, price);
      ug.IsLearned = false;
      Upgrades.Add(ug);
    }

    public int GetAmmnt(UpgradeType filter)
    {
      float result = 0;
      foreach (Upgrade ug in Upgrades)
        if (ug.IsLearned && ug.Type == filter)
          if (ug.bonus.percent)
            result *= ug.bonus.value;
          else
            result += ug.bonus.value;
      return (int)result;
    }
  }
}