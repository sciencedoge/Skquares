using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    public delegate string UITextUpdate();
    class UIText : UIElement
    {
        public String Text;
        public SpriteFont Font;
        public int ClickTimeout = 5;
        public UITextUpdate update = new UITextUpdate(() => "");
        public int Scale;
        public bool Centered = false;
        public Color color;

        /// <summary>
        /// Updates the Text
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            string s = update();
            if (s != "")
                Text = s;
        }

        /// <summary>
        /// creates a UIText object
        /// </summary>
        /// <param name="bounds">What the coords of the text are</param>
        public UIText(SpriteFont font, Rectangle bounds, int scale, Color c)
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
            float sizeX = Font.MeasureString(Text).X * Scale / 2;
            float sizeY = Font.MeasureString(Text).Y * Scale / 2;
            if (Centered)
                spriteBatch.DrawString(Font, Text, new Vector2((Bounds.Left + Bounds.Right) / 2f - sizeX, (Bounds.Top + Bounds.Bottom) / 2f - sizeY), color, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
            else
                spriteBatch.DrawString(Font, Text, Bounds.Location.ToVector2(), color, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
        }

        public override void WhenClicked(Point at) { }
        public override UIElement GetActive() { return null; }
        public override UISprite CurrentSprite() { return null; }
    }
}
