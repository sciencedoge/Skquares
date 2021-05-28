﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.UI
{
    abstract class UIElement
    {
        public UIElement nextFocus, prevFocus;
        public bool Focused = false;
        public bool IsActive = true;
        public Rectangle Bounds;
        
        /// <summary>
        /// Gets the current Sprite
        /// </summary>
        /// <returns>the Current Sprite</returns>
        public abstract UISprite CurrentSprite();
        
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

        /// <summary>
        ///  updates the focused uielements
        /// </summary>
        /// <param name="at"></param>
        public virtual void WhenMoved(Point at) {
            Focused = true;
            UIManager.Instance.focused = this;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
                CurrentSprite().Draw(spriteBatch, Bounds, 0);
        }
    }
}
