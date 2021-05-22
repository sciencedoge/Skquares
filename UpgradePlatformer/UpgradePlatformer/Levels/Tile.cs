﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Levels
{
    class Tile
    {
        private static Rectangle TILE_SPRITE = new Rectangle(0, 13, 16, 16);
        private static Color[] COLORS = { Color.Green, Color.Brown, Color.Beige, Color.Gray, Color.Orange,  Color.Orange,  Color.Orange, Color.Orange, Color.Transparent,};
        private Sprite Sprite;
        public int Kind;
        public float Rotation;
        public int CollisionKind;

        public Tile(Texture2D texture, int kind, int rotation, int collision)
        {
            Kind = kind;
            Sprite = new Sprite(texture, TILE_SPRITE, new Vector2(8f, 8f), COLORS[kind - 1]);
            Rotation = (MathF.PI * 0.5f) * rotation;
            CollisionKind = collision;
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            Sprite.Draw(spriteBatch, new Point((int)Position.X, (int)Position.Y), Rotation);
        }
    }
}
