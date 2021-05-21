using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class UIElement
    {
        public Rectangle Bounds;

        /// <summary>
        /// Do some random stuff before draw
        /// </summary>
        /// <param name="gameTime">a GameTime Object</param>
        public void Update(GameTime gameTime) { throw new NotImplementedException(); }

        /// <summary>
        /// Runs when the UIElement is clicked
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public void WhenClicked(Point at) { throw new NotImplementedException(); }
    }
}
