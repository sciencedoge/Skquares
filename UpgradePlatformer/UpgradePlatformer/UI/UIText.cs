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
        public UIText(SpriteFont font, Rectangle bounds, Color c)
        {
            Bounds = bounds;
            Font = font;
            Text = "";
            color = c;
        }

        /// <summary>
        /// Calls the onclick function if Disabled is false
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public override void WhenClicked(Point at) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Bounds.Location.ToVector2(), color);
        }

        public override UISprite CurrentSprite() { return null; }
    }
}
