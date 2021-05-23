using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Levels
{
    class Tile
    {
        private static Rectangle TILE_SPRITE = new Rectangle(0, 14, 14, 14);
        private static Color[] COLORS = { Color.Green, Color.Brown, Color.Beige, Color.Gray, Color.Orange,  Color.Orange,  Color.White, Color.Orange, Color.Transparent,};
        private Sprite Sprite;
        public Point TileSize;
        public int Kind;
        public float Rotation;
        public int CollisionKind;
        public Rectangle Position;

        public Tile(Texture2D texture, int kind, int rotation, int collision, Point tileSize)
        {
            Kind = kind;
            Sprite = new Sprite(texture, TILE_SPRITE, tileSize.ToVector2() * 0.5f, COLORS[kind - 1]);
            Rotation = (MathF.PI * 0.5f) * rotation;
            CollisionKind = collision;
            TileSize = tileSize;
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Position = new Rectangle(position.ToPoint() + (TileSize.ToVector2() * 0.5f).ToPoint(), TileSize);
            Sprite.Draw(spriteBatch, new Point((int)position.X, (int)position.Y), Rotation, TileSize.ToVector2());
        }
    }
}
