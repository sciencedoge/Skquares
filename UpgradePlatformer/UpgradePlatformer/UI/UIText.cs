using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.UI
{
  public delegate string UITextUpdate();
  public class UIText : UIElement
  {
    public String Text;
    public SpriteFont Font;
    public int ClickTimeout = 5;
    public UITextUpdate update = new UITextUpdate(() => "");
    public float Scale;
    public bool Centered = false;
    public Color color;

    /// <summary>
    /// Updates the Text
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Update(GameTime gameTime)
    {
      string s = update();
      if (s != "")
        Text = s;
    }

    /// <summary>
    /// creates a UIText object
    /// </summary>
    /// <param name="bounds">What the coords of the text are</param>
    public UIText(SpriteFont font, Rectangle bounds, float scale, Color c)
    {
      Bounds = bounds;
      Font = font;
      Text = "";
      color = c;
      Scale = scale;
      IsActive = true;
    }

    /// <summary>
    /// draws the Text
    /// </summary>
    /// <param name="gameTime">the Gametime object</param>
    /// <param name="spriteBatch">the spritebatch object</param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (!IsActive)
        return;
      float sizeX = Font.MeasureString(Text).X * Scale * Sprite.GetScale() / 2;
      float sizeY = Font.MeasureString(Text).Y * Scale * Sprite.GetScale() / 2;
      Rectangle bounds = Bounds;
      bounds.X = (int)(Sprite.GetScale() * bounds.X);
      bounds.Y = (int)(Sprite.GetScale() * bounds.Y);
      bounds.Location += Sprite.GetOrigin();
      bounds.Width = (int)(Sprite.GetScale() * bounds.Width);
      bounds.Height = (int)(Sprite.GetScale() * bounds.Height);
      if (Centered)
        spriteBatch.DrawString(Font, Text, new Vector2((bounds.Left + bounds.Right) / 2f - sizeX, (bounds.Top + bounds.Bottom) / 2f - sizeY), color, 0f, new Vector2(0, 0), Scale * Sprite.GetScale(), SpriteEffects.None, 0f);
      else
        spriteBatch.DrawString(Font, Text, bounds.Location.ToVector2(), color, 0f, new Vector2(0, 0), Scale * Sprite.GetScale(), SpriteEffects.None, 0f);
    }

    public override bool WhenClicked(Point at) { return false; }
    public override UIElement GetActive() { return null; }
    public override UISprite CurrentSprite() { return null; }
  }
}
