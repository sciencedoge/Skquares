using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Entities
{
  public abstract class EntityObject
  {

    public bool IsActive;
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    public abstract int Intersects(List<EntityObject> objects);
    public EntityKind Kind;

    /// <summary>
    /// creates an entity object
    /// </summary>
    /// <param name="kind"></param>
    public EntityObject(EntityKind kind)
    {
      Kind = kind;
      IsActive = true;
    }

    public Rectangle hitbox;

    /// <summary>
    /// draws the light map for the entity
    /// </summary>
    private static Rectangle TILE_EMPTY = new Rectangle(49, 30, 60, 60);
    public virtual void DrawLightMap(SpriteBatch spriteBatch)
    {
      if (!IsActive) return;
      Sprite s = new Sprite(TILE_EMPTY, new Vector2(30), Color.White);
      s.Draw(spriteBatch, hitbox.Location, 0, hitbox.Size.ToVector2() * 5);
    }

  }
  public enum EntityKind
  {
    PLAYER,
    ENEMY,
    BOSS,
    COIN,
    UPGRADE,
    TORCH,
  }

}