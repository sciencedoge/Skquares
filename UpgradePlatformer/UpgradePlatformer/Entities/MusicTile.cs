using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
  //HEADER=======================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/22/2021
  //Purpose: Creates functionality for coins - 
  //a basic collectible
  //=============================================
  class MusicTile : CollectibleObject
  {

    //Constructor
    public MusicTile(int value, Rectangle hitbox, Tile t)
        : base(value, hitbox, EntityKind.TORCH, t)
    {
      SpriteBounds = new Rectangle(0, 13, 16, 16); // empty
      UpdateSprite();
      Bob = 0;
      this.hitbox = t.Position;
    }

    /// <summary>
    /// updates the Coin
    /// </summary>
    public override void Update(GameTime gt)
    {
      spriteSize = hitbox.Size;
    }

    /// <summary>
    /// draws the particles
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="gt"></param>
    public override void Draw(SpriteBatch sb, GameTime gt)
    {
      base.Draw(sb, gt);
    }

    public override int Intersects(EntityObject o)
    {
      if (o.Kind != EntityKind.PLAYER || !hitbox.Intersects(o.hitbox)) return 0;
      SoundManager.Instance.PlayMusic(Value - 1);
      return 0;
    }
  }
}
