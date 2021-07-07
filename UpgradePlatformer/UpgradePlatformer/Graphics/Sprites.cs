using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UpgradePlatformer.Graphics
{
  public class Sprite
  {
    public static List<Effect> Shaders;
    public static bool Dim, Light;
    public static Texture2D texture;
    public static GraphicsDeviceManager graphics;
    public SpriteEffects effects = SpriteEffects.None;
    public Rectangle Position;
    public Vector2 Origin;
    public Color TintColor;

    /// <summary>
    /// creates a sprite object
    /// </summary>
    /// <param name="position">the position of the texture on the sheet</param>
    /// <param name="origin">the center of the Sprite</param>
    /// <param name="tintColor">the color to tint the sprite</param>
    public Sprite(Rectangle position, Vector2 origin, Color tintColor)
    {
      Position = position;
      Origin = origin;
      TintColor = tintColor;
    }

    /// <summary>
    /// renders a sprite object
    /// </summary>
    /// <param name="spriteBatch">the SpriteBatch object</param>
    /// <param name="renderPosition">where to render the sprite</param>
    /// <param name="rotation">the rotation of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation)
    {
      Draw(spriteBatch, renderPosition, rotation, Position.Size.ToVector2());
    }

    /// <summary>
    /// renders a sprite object
    /// </summary>
    /// <param name="spriteBatch">the SpriteBatch object</param>
    /// <param name="renderPosition">where to render the sprite</param>
    /// <param name="rotation">the rotation of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation, SpriteEffects effect)
    {
      this.effects = effect;
      Draw(spriteBatch, renderPosition, rotation, Position.Size.ToVector2());
    }

    /// <summary>
    /// renders a sprite object
    /// </summary>
    /// <param name="spriteBatch">the SpriteBatch object</param>
    /// <param name="renderPosition">where to render the sprite</param>
    /// <param name="rotation">the rotation of the sprite</param>
    /// <param name="Size">the size to render the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation, Vector2 Size)
    {
      renderPosition.X = (int)(GetScale() * renderPosition.X);
      renderPosition.Y = (int)(GetScale() * renderPosition.Y);
      Size.X = (int)(GetScale() * (Size.X)) + 1;
      Size.Y = (int)(GetScale() * (Size.Y)) + 1;
      Color c = TintColor;
      if (Dim && Light)
      {
        c.R /= 2;
        c.G /= 2;
        c.B /= 2;
      }
      Rectangle renderRect = Position;
      renderRect.Location = renderPosition + new Point(0, (int)(40f * GetScale())) + GetOrigin();
      renderRect.Size = Size.ToPoint();
      spriteBatch.Draw(texture, renderRect, Position, c, rotation, Origin, effects, 0f);
    }

    /// <summary>
    /// gets the origin of the window render area
    /// </summary>
    /// <returns></returns>
    public static Point GetOrigin()
    {
      Point Size = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - (int)(40f * GetScale()));
      if (Size.X > Size.Y)
      {
        return new Point((Size.X - Size.Y) / 2, 0);
      }
      else
      {
        return new Point(0, (Size.Y - Size.X) / 2);
      }
    }

    /// <summary>
    /// returns a rect of the render positions
    /// </summary>
    /// <returns></returns>
    public static Rectangle GetRect()
    {
      Rectangle result = new Rectangle();
      result.Location = GetOrigin();
      result.Size = new Point((int)MathF.Min(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 40f * GetScale()));
      return result;
    }

    /// <summary>
    /// gets the scale of the window
    /// </summary>
    /// <returns>the size of one unit in the window</returns>
    public static float GetScale()
    {
      return MathF.Min(graphics.PreferredBackBufferWidth / 630f, graphics.PreferredBackBufferHeight / 670f);
    }

    /// <summary>
    /// copys a sprite, problaby not needed
    /// </summary>
    /// <returns>a new sprite</returns>
    public Sprite Copy()
    {
      Sprite spr = new Sprite(Position, Origin, TintColor)
      {
        effects = effects
      };
      return spr;
    }
  }
}
