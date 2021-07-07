using Microsoft.Xna.Framework;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Entities
{
  //HEADER=======================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/22/2021
  //Purpose: Creates functionality for coins - 
  //a basic collectible
  //=============================================
  class Coin : CollectibleObject
  {
    //Constructor
    public Coin(int value, Rectangle hitbox, Tile t)
        : base(value, hitbox, EntityKind.COIN, t)
    {
      SpriteBounds = new Rectangle(0, 7, 5, 5);
      UpdateSprite();
    }

    /// <summary>
    /// updates the Coin
    /// </summary>
    public void Update()
    {
      spriteSize = hitbox.Size;
    }
  }
}
