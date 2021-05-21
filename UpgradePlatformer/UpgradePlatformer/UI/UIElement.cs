using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    abstract class UIElement
    {
        public Rectangle Bounds;

        /// <summary>
        /// Do some random stuff before draw
        /// </summary>
        /// <param name="gameTime">a GameTime Object</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Runs when the UIElement is clicked
        /// </summary>
        /// <param name="at">where the button UIElement was clicked 0,0 being the top corner</param>
        public abstract void WhenClicked(Point at);
    }
}
