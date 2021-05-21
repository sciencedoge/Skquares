using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class Sprite
    {
        Texture2D Texture;
        Rectangle Position;
        Vector2 Origin;
        Color TintColor;

        public Sprite(Texture2D texture, Rectangle position, Vector2 origin, Color tintColor)
        {
            /*
             * creates a sprite object
             * 
             * texture: a Texture2D of the sprite sheet
             * position: the position of the texture on the sheet
             * origin: the center of the Sprite
             * tintColor: the color to tint the sprite
             */
            Texture = texture;
            Position = position;
            Origin = origin;
            TintColor = tintColor;
        }

        public void Draw(SpriteBatch spriteBatch, Point renderPosition, float rotation)
        {
            /*
             * renders a sprite object
             * 
             * spriteBatch: a SpriteBatch Object
             * renderPosistion: where to put the orgin of the sprite
             * the rotation of the sprite in RADIANS
             */
            Rectangle renderRect = Position;
            renderRect.Location = renderPosition;
            spriteBatch.Draw(Texture, renderRect, Position, TintColor, rotation, Origin, SpriteEffects.None, 0f);
        }
    }
}
