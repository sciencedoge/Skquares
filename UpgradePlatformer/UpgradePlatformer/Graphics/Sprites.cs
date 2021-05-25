﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Graphics
{
    public class Sprite
    {
        public static bool Dim;
        public Texture2D Texture;
        public SpriteEffects effects = SpriteEffects.None;
        public Rectangle Position;
        public Vector2 Origin;
        public Color TintColor;
        
        /// <summary>
        /// creates a sprite object
        /// </summary>
        /// <param name="texture">a Texture2D of the sprite sheet</param>
        /// <param name="position">the position of the texture on the sheet</param>
        /// <param name="origin">the center of the Sprite</param>
        /// <param name="tintColor">the color to tint the sprite</param>
        public Sprite(Texture2D texture, Rectangle position, Vector2 origin, Color tintColor)
        {
            Texture = texture;
            Position = position;
            Origin = origin;
            TintColor = tintColor;
        }
        
        /// <summary>
        /// renders a sprite object
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch Object</param>
        /// <param name="renderPosistion">where to put the orgin of the sprite</param>
        /// <param name="rotation">the rotation of the sprite in RADIANS</param>
        public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation)
        {
            Draw(spriteBatch, renderPosition, rotation, Position.Size.ToVector2());
        }

        public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation, Vector2 Size)
        {
            Color c = TintColor;
            if (Dim)
            {
                c.R /= 2;
                c.G /= 2;
                c.B /= 2;
            }
            Rectangle renderRect = Position;
            renderRect.Location = renderPosition + (Origin / 2).ToPoint();
            renderRect.Size = Size.ToPoint();
            spriteBatch.Draw(Texture, renderRect, Position, c, rotation, Origin, effects, 0f);
        }
    }
}
