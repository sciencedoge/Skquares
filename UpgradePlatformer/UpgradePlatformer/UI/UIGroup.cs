using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    public class UIGroup : UIElement
    {
        public List<UIElement> UIElements;
        public UIGroup(List<UIElement> elements, Rectangle bounds) {
            UIElements = elements;
            Bounds = bounds;
        }

        public UIGroup() {
            UIElements = new List<UIElement>();
            Bounds = new Rectangle(0, 0, 630, 630);
        }

        public void Add(UIElement e) {
            UIElements.Add(e);
        }

        public override UISprite CurrentSprite() { return null; }

        /// <summary>
        /// resets the active uiElements
        /// </summary>
        public override void ResetActive()
        {
            foreach (UIElement e in UIElements)
            {
                e.ResetActive();
            }
        }

        /// <summary>
        /// gets the active element
        /// </summary>
        /// <returns>the element</returns>
        public override UIElement GetActive()
        {
            foreach (UIElement e in UIElements)
            {
                UIElement n = e.GetActive();
                if (n != null) return n;
            }
            return null;
        }

        /// <summary>
        /// updates the UIGroup
        /// </summary>
        /// <param name="gameTime">a GameTime object</param>
        public override void Update(GameTime gameTime) {
            foreach (UIElement e in UIElements)
            {
                e.IsActive = e.IsActive || IsActive;
                e.Update(gameTime);
            }
        }

        /// <summary>
        /// processes a mouse move event
        /// </summary>
        /// <param name="position">where the mouse was moved</param>
        public override void WhenMoved(Point position)
        {
            foreach (UIElement e in UIElements)
            {
                e.Focused = false;
                if (e.Bounds.Contains(position) && e.IsActive)
                {
                    e.WhenMoved(position - e.Bounds.Location);
                }
            }
        }

        /// <summary>
        /// processes a mouse click event
        /// </summary>
        /// <param name="position">where the mouse was</param>
        public override bool WhenClicked(Point position)
        {
            foreach (UIElement e in UIElements)
            {
                if (e.Bounds.Contains(position) && e.IsActive)
                {
                    return e.WhenClicked(position - e.Bounds.Location);
                }
            }
            return false;
        }

        /// <summary>
        /// draws the UIElements in the group
        /// </summary>
        /// <param name="gameTime">the GameTime object</param>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;
            foreach (UIElement e in UIElements)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }
    }

}