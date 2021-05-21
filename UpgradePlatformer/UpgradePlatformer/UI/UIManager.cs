using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class UIManager
    {
        List<UIElement> UIElements;

        /// <summary>
        /// Do some random stuff before draw
        /// </summary>
        /// <param name="gameTime">a gameTime Object</param>
        public void Update(GameTime gameTime)
        {
            foreach (UIElement e in UIElements)
            {
                e.Update(gameTime);
            }
        }
        
        /// <summary>
        /// processes a click anywhere on the screen
        /// </summary>
        /// <param name="position">the position the screen was clicked</param>
        /// <returns>true if the click is used</returns>
        public bool ProcessClick(Point position)
        {
            foreach (UIElement e in UIElements)
            {
                if (e.Bounds.Contains(position))
                {
                    e.WhenClicked(position - e.Bounds.Location);
                    return true;
                }
            }
            return false;
        }
    }
}
