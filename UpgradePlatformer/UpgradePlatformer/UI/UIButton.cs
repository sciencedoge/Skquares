using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class UIButton : UIElement
    {
        public delegate void OnClick();
        OnClick onClick = new OnClick(() => { });
        public bool Disabled;
        public void Update(GameTime gameTime) { throw new NotImplementedException(); }

        /// <summary>
        /// Calls a function if Disabled == false
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public void WhenClicked(Point at)
        {
            if (Disabled) return;

            onClick();
        }
    }
}
