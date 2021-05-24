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
        private static Rectangle TILE_SPRITE = new Rectangle(0, 13, 15, 15);
        private static Color[] COLORS = { Color.Green, Color.Brown, Color.Beige, Color.Gray, Color.Orange,  Color.Orange,  Color.White, Color.Orange, Color.Transparent,};
        private Sprite Sprite;
        public Vector2 TileSize;
        public int Kind;
        public float Rotation;
        public int CollisionKind;
        public Rectangle Position;
        private Vector2 TileCenter;

        public Tile(Texture2D texture, int kind, int rotation, int collision, Vector2 tileSize)
        {
            Kind = kind;
            TileSize = tileSize;
            TileCenter = new Vector2(TileSize.X / 2, TileSize.Y / 2);
            Sprite = new Sprite(texture, TILE_SPRITE, TileCenter, COLORS[kind - 1]);
            Rotation = (MathF.PI * 0.5f) * rotation;
            CollisionKind = collision;
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Position = new Rectangle(position.ToPoint() + TileSize.ToPoint() - TileCenter.ToPoint(), TileSize.ToPoint());
            Sprite.Draw(spriteBatch, Position.Location, Rotation, Position.Size.ToVector2());
        }
    }
}
