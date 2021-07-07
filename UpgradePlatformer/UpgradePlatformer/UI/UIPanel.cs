using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace UpgradePlatformer.UI
{
  public delegate bool ShowCond();
  public class UIPanel : UIElement
  {
    // Setup constants for sprites
    private static Rectangle PANEL_NORMAL_SPRITE = new Rectangle(0, 0, 7, 7);
    private static Rectangle PANEL_NORMAL_CENTER = new Rectangle(3, 3, 1, 1);

    // Vars
    UISprite NormalSprite;
    public UIText Text;
    public UIAction onClick = new UIAction((i) => { });
    public bool Disabled = false;
    private Color NormalTextColor, InvertedTextColor;
    public int ClickTimeout = 5;
    private int ClickTime;
    private int showtime;
    public ShowCond cond;

    /// <summary>
    /// Updates the Button
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Update(GameTime gameTime)
    {
      if (showtime > 0 && cond()) { showtime--; IsActive = true; };
      if (showtime == 0 && IsActive) IsActive = false;
      ClickTime = Math.Max(0, ClickTime - 1);
      Text.Update(gameTime);
      Text.color = InvertedTextColor;
      if (Disabled || ClickTime != 0) return;
      Text.color = NormalTextColor;
    }

    public void show(int ShowTime)
    {
      showtime = ShowTime;
      IsActive = true;
    }

    /// <summary>
    /// creates a UIButton
    /// </summary>
    /// <param name="bounds">What the coords of the button are</param>
    public UIPanel(SpriteFont font, Rectangle bounds)
    {
      cond = new ShowCond(() => true);
      Bounds = bounds;
      Rectangle TextBounds = new Rectangle(Bounds.Location + new Point(5, 5), Bounds.Size - new Point(10, 10));
      Text = new UIText(font, TextBounds, 1.5f, Color.Black);
      NormalTextColor = Color.White;
      InvertedTextColor = Color.Black;
      NormalSprite = new UISprite(PANEL_NORMAL_SPRITE, PANEL_NORMAL_CENTER, new Vector2(0, 0), Color.White);
    }

    /// <summary>
    /// Calls the onclick function if Disabled is false
    /// </summary>
    /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
    public override bool WhenClicked(Point at)
    {
      if (!IsActive || Disabled || ClickTime != 0) return false;

      ClickTime = ClickTimeout;
      onClick(0);
      return true;
    }

    /// <summary>
    /// gets the current sprite of the button
    /// </summary>
    /// <returns>a ui sprite for the button</returns>
    public override UISprite CurrentSprite()
    {
      return NormalSprite;
    }

    /// <summary>
    /// draws the button
    /// </summary>
    /// <param name="gameTime">the Gametime object</param>
    /// <param name="spriteBatch">the spritebatch object</param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (!IsActive) return;
      CurrentSprite().Draw(spriteBatch, Bounds, 0);
      Text.Draw(gameTime, spriteBatch);
    }
  }
}
