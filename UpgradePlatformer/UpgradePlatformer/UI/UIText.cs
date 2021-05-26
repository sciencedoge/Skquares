using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    class UIText : UIElement
    {
        public String Text;

        public SpriteFont Font;
        public int ClickTimeout = 5;
        private int ClickTime;
        private int Scale;
        public bool Centered = false;

        public Color color;

        /// <summary>
        /// Updates the Button
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            ClickTime = Math.Max(0, ClickTime - 1);
        }

        /// <summary>
        /// creates a UIButton
        /// </summary>
        /// <param name="bounds">What the coords of the button are</param>
        public UIText(SpriteFont font, Rectangle bounds, int scale, Color c)
        {
            Bounds = bounds;
            Font = font;
            Text = "";
            color = c;
            Scale = scale;
        }

        /// <summary>
        /// Calls the onclick function if Disabled is false
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public override void WhenClicked(Point at) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;
            float sizeX = Font.MeasureString(Text).X * Scale / 2;
            float sizeY = Font.MeasureString(Text).Y * Scale / 2;
            if (Centered)
                spriteBatch.DrawString(Font, Text, new Vector2((Bounds.Left + Bounds.Right) / 2f - sizeX, (Bounds.Top + Bounds.Bottom) / 2f - sizeY), color, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
            else
                spriteBatch.DrawString(Font, Text, Bounds.Location.ToVector2(), color, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
        }

        public override UISprite CurrentSprite() { return null; }
    }
}
