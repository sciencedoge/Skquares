using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer.Levels
{
    [Serializable]
    public class Tile
    {
        private static Rectangle TILE_SPRITE = new Rectangle(0, 13, 16, 16);
        private static Rectangle TILE_EMPTY = new Rectangle(48, 29, 32, 32);
        private static Rectangle TILE_SPRITE_GOAL = new Rectangle(16, 29, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD = new Rectangle(32, 29, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD_MID = new Rectangle(32, 45, 16, 16);
        private static Rectangle TILE_SPRITE_CLOUD_BOT = new Rectangle(16, 45, 16, 16);
        private static Rectangle TILE_SPRITE_SPIKE = new Rectangle(32, 13, 16, 16);
        private static Rectangle TILE_SPRITE_LAVA_TOP = new Rectangle(0, 45, 16, 16);
        private static Rectangle TILE_SPRITE_LAVA_BOT = new Rectangle(0, 61, 16, 16);
        private static Rectangle TILE_SPRITE_PLATFORM = new Rectangle(32, 61, 16, 16);
        private static Color[] COLORS = { Color.Green, Color.Green, Color.Beige, Color.Gray, Color.White, Color.White, Color.White, Color.Orange, Color.White };
        private Sprite Sprite;
        private Vector2 TileCenter;
        public Vector2 TileSize;
        public int Kind;
        public int CollisionKind;
        public Rectangle Position;
        public bool Spawner;

        /// <summary>
        /// creates a tile object
        /// </summary>
        /// <param name="kind">the tile kind</param>
        /// <param name="rotation">the rotation of the tile</param>
        /// <param name="collision">the collision kind of the tile</param>
        /// <param name="spawner">the spawner id of the tile</param>
        /// <param name="tileSize">the size of the tile</param>
        /// <param name="above">the tile above this one</param>
        public Tile(int kind, int rotation, int collision, int spawner, Vector2 tileSize, Tile above)
        {
            Kind = kind;
            TileSize = tileSize;
            if (spawner != 9) {
                Spawner = true;
                Kind = spawner - 1000;
                return;
            }
            TileCenter = new Vector2(0);
            if (Kind == 9)
            {
                Sprite = new Sprite(TILE_EMPTY, TileCenter, COLORS[kind - 1]);
                if (above != null)
                    if (above.Kind == 7)
                    {
                        Sprite = new Sprite(TILE_SPRITE_CLOUD_BOT, TileCenter, COLORS[6]);
                        Kind = 10;
                    }
            }
            else if (Kind == 1)
            {
                Sprite = new Sprite(TILE_SPRITE_PLATFORM, TileCenter, COLORS[3]);
            }
            else if (Kind == 8)
            {
                if (above == null) Sprite = new Sprite(TILE_SPRITE_LAVA_TOP, TileCenter, COLORS[6]);
                if (above.Kind != 9 && !above.Spawner) Sprite = new Sprite(TILE_SPRITE_LAVA_BOT, TileCenter, COLORS[6]);
                else Sprite = new Sprite(TILE_SPRITE_LAVA_TOP, TileCenter, COLORS[6]);
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
            CollisionKind = collision;
        }

        public Tile() { }

        /// <summary>
        /// draws a tile
        /// </summary>
        /// <param name="spriteBatch">the Sprite Batch object</param>
        /// <param name="position">the position of the tile on the map</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Kind == 9 || Spawner)
                return;
            UpdatePos(position);
            Sprite.Draw(spriteBatch, Position.Location, 0, Position.Size.ToVector2());
        }

        /// <summary>
        /// draws a tile
        /// </summary>
        /// <param name="spriteBatch">the Sprite Batch object</param>
        /// <param name="position">the position of the tile on the map</param>
        public void DrawLightMap(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Kind < 9)
                return;
            UpdatePos(position);
            Sprite s = new Sprite(TILE_EMPTY, TileCenter, Color.White);
            s.Draw(spriteBatch, Position.Location, 0, Position.Size.ToVector2() * 6);
        }

        /// <summary>
        /// sets the positon of the tile for collisions
        /// </summary>
        /// <param name="position">the position</param>
        public void UpdatePos(Vector2 position)
        {
            Position = new Rectangle(position.ToPoint(), TileSize.ToPoint());
        }
    }
}
