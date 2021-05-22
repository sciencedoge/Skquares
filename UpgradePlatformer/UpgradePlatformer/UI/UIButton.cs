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
        private static Rectangle BUTTON_NORMAL_SPRITE = new Rectangle(0, 0, 15, 13);
        private static Rectangle BUTTON_CLICKED_SPRITE = new Rectangle(15, 0, 15, 13);
        private static Rectangle BUTTON_DISABLED_SPRITE = new Rectangle(30, 0, 15, 13);
        private static Rectangle BUTTON_NORMAL_CENTER = new Rectangle(4, 2, 8, 8);
        private static Rectangle BUTTON_CLICKED_CENTER = new Rectangle(19, 2, 8, 8);
        private static Rectangle BUTTON_DISABLED_CENTER = new Rectangle(34, 2, 8, 8);
        
        // Vars
        UISprite NormalSprite;
        UISprite ClickedSprite;
        UISprite DisabledSprite;
        UIAction onClick = new UIAction(() => { });
        public bool Disabled;

        public int ClickTimeout = 5;

        private int ClickTime;

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
        public UIButton(Texture2D texture, Rectangle bounds)
        {
            Bounds = bounds;
            NormalSprite = new UISprite(texture, BUTTON_NORMAL_SPRITE, BUTTON_NORMAL_CENTER, new Vector2(0, 0), Color.White);
            ClickedSprite = new UISprite(texture, BUTTON_CLICKED_SPRITE, BUTTON_CLICKED_CENTER, new Vector2(0, 0), Color.White);
            DisabledSprite = new UISprite(texture, BUTTON_DISABLED_SPRITE, BUTTON_DISABLED_CENTER, new Vector2(0, 0), Color.White);
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
            return NormalSprite;
        }
    }
}
