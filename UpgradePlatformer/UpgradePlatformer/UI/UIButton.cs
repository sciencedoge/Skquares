using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    class UIButton : UIElement
    {
        // Setup constants for sprites
        private static Rectangle BUTTON_NORMAL_SPRITE = new Rectangle(0, 0, 7, 7);
        private static Rectangle BUTTON_CLICKED_SPRITE = new Rectangle(7, 0, 7, 7);
        private static Rectangle BUTTON_DISABLED_SPRITE = new Rectangle(15, 0, 7, 7);
        private static Rectangle BUTTON_FOCUSED_SPRITE = new Rectangle(21, 0, 7, 7);
        private static Rectangle BUTTON_NORMAL_CENTER = new Rectangle(3, 3, 1, 1);
        private static Rectangle BUTTON_CLICKED_CENTER = new Rectangle(9, 3, 1, 1);
        private static Rectangle BUTTON_DISABLED_CENTER = new Rectangle(15, 3, 1, 1);
        private static Rectangle BUTTON_FOCUSED_CENTER = new Rectangle(24, 3, 1, 1);

        // Vars
        UISprite NormalSprite, ClickedSprite, DisabledSprite, FocusedSprite;
        public UIText Text;
        public UIAction onClick = new UIAction(() => { });
        public bool Disabled = false;
        private Color NormalTextColor, InvertedTextColor;
        public int ClickTimeout = 5;
        private int ClickTime;

        /// <summary>
        /// Updates the Button
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            ClickTime = Math.Max(0, ClickTime - 1);
            Text.color = InvertedTextColor;
            if (Disabled || ClickTime != 0) return;
            Text.color = NormalTextColor;
        }

        /// <summary>
        /// creates a UIButton
        /// </summary>
        /// <param name="bounds">What the coords of the button are</param>
        public UIButton(Texture2D texture, SpriteFont font, Rectangle bounds)
        {
            Bounds = bounds;
            Text = new UIText(font, Bounds, 1, Color.Black);
            Text.Centered = true;
            NormalTextColor = Color.White;
            InvertedTextColor = Color.Black;
            NormalSprite = new UISprite(BUTTON_NORMAL_SPRITE, BUTTON_NORMAL_CENTER, new Vector2(0, 0), Color.White);
            ClickedSprite = new UISprite(BUTTON_CLICKED_SPRITE, BUTTON_CLICKED_CENTER, new Vector2(0, 0), Color.White);
            DisabledSprite = new UISprite(BUTTON_DISABLED_SPRITE, BUTTON_DISABLED_CENTER, new Vector2(0, 0), Color.White);
            FocusedSprite = new UISprite(BUTTON_FOCUSED_SPRITE, BUTTON_FOCUSED_CENTER, new Vector2(0, 0), Color.White);
        }

        /// <summary>
        /// Calls the onclick function if Disabled is false
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public override void WhenClicked(Point at)
        {
            if (Disabled || ClickTime != 0) return;

            ClickTime = ClickTimeout;
            onClick();
        }

        public override UISprite CurrentSprite()
        {
            if (Disabled) return DisabledSprite;
            if (ClickTime != 0) return ClickedSprite;
            if (Focused) return FocusedSprite;
            return NormalSprite;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive) {
                CurrentSprite().Draw(spriteBatch, Bounds, 0);
                Text.Draw(gameTime, spriteBatch);
            }
        }
    }
}
