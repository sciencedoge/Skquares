using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    class UIGroup : UIElement
    {
        public List<UIElement> UIElements;
        public UIGroup(List<UIElement> elements, Rectangle bounds) {
            UIElements = elements;
            Bounds = bounds;
        }
        public override UISprite CurrentSprite() { return null; }

        public override void Update(GameTime gameTime) {
            foreach (UIElement e in UIElements)
            {
                e.Update(gameTime);
            }
        }

        public override void WhenClicked(Point position)
        {
            foreach (UIElement e in UIElements)
            {
                if (e.Bounds.Contains(position) && e.IsActive)
                {
                    e.WhenClicked(position - e.Bounds.Location);
                }
            }
        }
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