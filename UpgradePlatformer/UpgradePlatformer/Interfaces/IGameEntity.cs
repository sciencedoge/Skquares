using Microsoft.Xna.Framework;

namespace UpgradePlatformer.Interfaces
{
  //HEADER========================================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/22/2021
  //Purpose: Provides the groundwork for all game related entities
  //==============================================================
  interface IGameEntity
  {
    //Properties

    /// <summary>
    /// gets or sets the current HP of the
    /// entity
    /// </summary>
    public int CurrentHP { get; set; }

    /// <summary>
    /// gets or sets the max HP of the
    /// entity
    /// </summary>
    public int MaxHP { get; set; }

    /// <summary>
    /// Returns the hitbox of an entity
    /// </summary>
    public Rectangle Hitbox { get; }
  }
}
