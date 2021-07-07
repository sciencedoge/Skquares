using System;
using System.Collections.Generic;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Saves
{
  /// <summary>
  /// Container for your save game data.
  /// Put the variables you need here, as long as it's serializable.
  /// </summary>
  [Serializable]
  public class SaveData
  {
    public bool muted;
    public bool valid;
    public bool fullscreen;
    public uint lastWorld;
    public int money;
    public List<LevelCollectedEntity> collectedEntities;
    public List<Upgrade> upgrades;
  }
}