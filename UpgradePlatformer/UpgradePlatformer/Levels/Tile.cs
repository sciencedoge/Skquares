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
        private static Rectangle TILE_SPRITE = new Rectangle(0, 12, 16, 16);
        private static Rectangle TILE_SPRITE_GOAL = new Rectangle(16, 29, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD = new Rectangle(32, 29, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD_MID = new Rectangle(32, 45, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD_BOT = new Rectangle(16, 45, 16, 16);
        private static Rectangle TILE_SPRITE_SPIKE = new Rectangle(32, 13, 16, 16);
        private static Color[] COLORS = { Color.Green, Color.Brown, Color.Beige, Color.Gray, Color.White,  Color.White,  Color.White, Color.Orange, Color.Transparent};
        private Sprite Sprite;
        private Vector2 TileCenter;
        public Vector2 TileSize;
        public int Kind;
        public float Rotation;
        public int CollisionKind;
        public Rectangle Position;
        public bool Spawner;

        public Tile(int kind, int rotation, int collision, int spawner, Vector2 tileSize, Tile above)
        {
            Kind = kind;
            TileSize = tileSize;
            if (spawner != 9) {
                Spawner = true;
                Kind = spawner - 1000;
                return;
            }
            TileCenter = new Vector2(TileSize.X / 2, TileSize.Y / 2);
            if (Kind == 9 && above != null)
            {
                if (above.Kind == 7) {
                    Sprite = new Sprite(TILE_SPRITE_CLOUD_BOT, TileCenter, COLORS[6]);
                    Kind = 10;
                }
            }
            else if (Kind == 7)
            {
                if (above.Kind != 9 && !above.Spawner) Sprite = new Sprite(TILE_SPRITE_CLOUD_MID, TileCenter, COLORS[kind - 1]);
                else Sprite = new Sprite(TILE_SPRITE_CLOUD, TileCenter, COLORS[kind - 1]);
            }
            else if (Kind == 5) Sprite = new Sprite(TILE_SPRITE_SPIKE, TileCenter, COLORS[kind - 1]);
            else if (Kind == 6)
                Sprite = new Sprite(TILE_SPRITE_GOAL, TileCenter, COLORS[kind - 1]);
            else
                Sprite = new Sprite(TILE_SPRITE, TileCenter, COLORS[kind - 1]);
            Rotation = (MathF.PI * 0.5f) * rotation;
            CollisionKind = collision;
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Kind == 9 || Spawner)
                return;
            UpdatePos(position);
            Sprite.Draw(spriteBatch, Position.Location, Rotation, Position.Size.ToVector2());
        }
        public void UpdatePos(Vector2 position)
        {
            Position = new Rectangle(position.ToPoint() + TileSize.ToPoint() - TileCenter.ToPoint(), TileSize.ToPoint());
        }
    }
}
