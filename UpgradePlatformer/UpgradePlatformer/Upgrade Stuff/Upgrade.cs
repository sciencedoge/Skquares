using System;
using UpgradePlatformer.Entities;

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
  [Serializable]
  public enum UpgradeType
  {
    NONE,
    ANY,
    EXTRA_JUMP,
    HEALTH,
    WEAPON
  }

  [Serializable]
  public struct UpgradeBonus
  {
    public UpgradeType type;
    public float value;
    public bool percent;
  }

  [Serializable]
  public class Upgrade
  {
    public Upgrade() { }
    //Fields
    public bool isLearned;
    public int cost;

    public UpgradeBonus bonus;

    private Upgrade left;
    private Upgrade right;
    //prop

    public String UnlockText => $"You unlocked the {GetName(bonus.type)}\nUpgrade, now you {GetEffect(bonus.type)}!";

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
      get => cost <= EntityManager.Instance.PlayerMoney;
    }

    /// <summary>
    /// Returns the value of an upgrade
    /// </summary>
    public float Value
    {
      get { return bonus.value; }
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
      get { return bonus.type; }
    }


    //Ctor
    /// <summary>
    /// Creates a new Upgrade object
    /// </summary>
    /// <param name="value">value of the upgrade</param>
    public Upgrade(int value, UpgradeType type, int cost)
    {
      this.bonus.value = value;
      this.isLearned = false;

      this.bonus.type = type;
      this.cost = cost;
    }

    private static String GetName(UpgradeType type)
    {
      int level = UpgradeManager.Instance.GetAmmnt(type) + 1;
      switch (type)
      {
        case UpgradeType.EXTRA_JUMP:
          return $"Another Jump {level}";
        case UpgradeType.HEALTH:
          return $"Health {level}";
        case UpgradeType.WEAPON:
          return $"Weapon {level}";
        default:
          return "Broken";
      }
    }
    private static String GetEffect(UpgradeType type)
    {
      int level = UpgradeManager.Instance.GetAmmnt(type) + 1;
      switch (type)
      {
        case UpgradeType.EXTRA_JUMP:
          return $"can jump {level + 1} times";
        case UpgradeType.HEALTH:
          return $"have {level * 3 + 3} health";
        case UpgradeType.WEAPON:
          return $"can deal {level} damage";
        default:
          return "Broken";
      }
    }
  }
}
