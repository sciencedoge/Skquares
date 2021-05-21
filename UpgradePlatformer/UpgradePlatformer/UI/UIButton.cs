using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class UIButton : UIElement
    {
        UIAction onClick = new UIAction(() => { });
        public bool Disabled;
        public void Update(GameTime gameTime) { throw new NotImplementedException(); }

        /// <summary>
        /// creates a UIButton
        /// </summary>
        /// <param name="bounds"></param>
        public UIButton(Rectangle bounds)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Calls the onclick function if Disabled is false
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public void WhenClicked(Point at)
        {
            if (Disabled) return;

            onClick();
        }
    }
}
