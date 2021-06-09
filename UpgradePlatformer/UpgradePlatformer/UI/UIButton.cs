using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;

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
        public UIAction onClick = new UIAction((i) => { });
        public bool Disabled = false;
        private Color NormalTextColor, InvertedTextColor;
        public int ClickTimeout = 5;
        private int ClickTime;
        public Sprite Icon;

        /// <summary>
        /// sets the icon for the button
        /// </summary>
        /// <param name="icon">a sprite of the icon</param>
        public void SetIcon(Sprite icon) {
            Icon = icon;
        }

        /// <summary>
        /// Updates the Button
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            ClickTime = Math.Max(0, ClickTime - 1);
            Text.Update(gameTime);
            Text.color = InvertedTextColor;
            if (Disabled || ClickTime != 0) return;
            Text.color = NormalTextColor;
        }

        /// <summary>
        /// creates a UIButton
        /// </summary>
        /// <param name="bounds">What the coords of the button are</param>
        public UIButton(SpriteFont font, Rectangle bounds)
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
        /// Calls the onClick function if Disabled is false
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
            if (Disabled) return DisabledSprite;
            if (ClickTime != 0) return ClickedSprite;
            if (Focused) return FocusedSprite;
            return NormalSprite;
        }

        /// <summary>
        /// draws the button
        /// </summary>
        /// <param name="gameTime">the Gametime object</param>
        /// <param name="spriteBatch">the spritebatch object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Text.Bounds = Bounds;
            Text.Bounds.Inflate(-5, -5);
            Text.Bounds.X += Icon != null ? Text.Bounds.Height + 5 : 0;
            Text.Bounds.Width -= Icon != null ? Text.Bounds.Height + 5 : 0;

            CurrentSprite().Draw(spriteBatch, Bounds, 0);
            Text.Draw(gameTime, spriteBatch);
            if (Icon != null)
                Icon.Draw(spriteBatch, Bounds.Location + new Point(5, 5 - Bounds.Height), 0, new Vector2(Bounds.Height - 10));
        }
    }
}
