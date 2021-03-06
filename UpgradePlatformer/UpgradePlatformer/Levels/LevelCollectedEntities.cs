using System;

namespace UpgradePlatformer.Levels
{
  /// <summary>
  /// stores collected entitys within levels
  /// </summary>
  [Serializable]
  public class LevelCollectedEntity
  {
    public Tile tile;
    public int World;
    public int Level;
    public LevelCollectedEntity(Tile t)
    {
      tile = t;
      World = LevelManager.Instance.ActiveWorldNum();
      Level = LevelManager.Instance.ActiveLevelNum();
    }
    public LevelCollectedEntity() { }
  }
}