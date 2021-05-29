using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    class UIToggle : UIElement
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
        public UIAction onClick = new UIAction((i) => { });
        public bool Disabled = false;
        private Color NormalTextColor, InvertedTextColor;
        public int ClickTimeout = 5;
        public bool toggled;

        /// <summary>
        /// Updates the Toggle
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            Text.Update(gameTime);
            Text.color = InvertedTextColor;
            if (Disabled || toggled) return;
            Text.color = NormalTextColor;
        }

        /// <summary>
        /// creates a UIToggle
        /// </summary>
        /// <param name="bounds">What the coords of the button are</param>
        public UIToggle(SpriteFont font, Rectangle bounds)
        {
            Bounds = bounds;
            Text = new UIText(font, Bounds, 1, Color.Black)
            {
                Centered = true
            };
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
            if (!IsActive || Disabled) return;

            toggled = !toggled;
            if (toggled) onClick(1);
            else onClick(0);
        }

        /// <summary>
        /// gets the current sprite of the object
        /// </summary>
        /// <returns>the sprite</returns>
        public override UISprite CurrentSprite()
        {
            if (Disabled) return DisabledSprite;
            if (toggled) return ClickedSprite;
            if (Focused) return FocusedSprite;
            return NormalSprite;
        }

        /// <summary>
        /// draws the toggle
        /// </summary>
        /// <param name="gameTime">the gametime object</param>
        /// <param name="spriteBatch">the spritebatch object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CurrentSprite().Draw(spriteBatch, Bounds, 0);
            Text.Draw(gameTime, spriteBatch);
        }
    }
}
