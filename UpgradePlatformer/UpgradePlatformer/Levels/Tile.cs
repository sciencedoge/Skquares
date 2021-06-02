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
        private static Rectangle TILE_EMPTY = new Rectangle(49, 30, 60, 60);
        private static Sprite[,] AllSprites = new Sprite[20, 20];
        private static Color[] COLORS = { Color.White, Color.Green, Color.LightGray, Color.White, Color.White, Color.White, Color.White, Color.Orange, Color.White, Color.White };
        private Sprite Sprite;
        private Vector2 TileCenter;
        public Vector2 TileSize;
        public int Kind;
        public int CollisionKind;
        public Rectangle Position;
        public bool Spawner;
        public int SpawnerKind;

        /// <summary>
        /// creates a tile object
        /// </summary>
        /// <param name="kind">the tile kind</param>
        /// <param name="rotation">the rotation of the tile</param>
        /// <param name="collision">the collision kind of the tile</param>
        /// <param name="spawner">the spawner id of the tile</param>
        /// <param name="tileSize">the size of the tile</param>
        /// <param name="above">the tile above this one</param>
        public Tile(int kind, int rotation, int collision, int spawner, Vector2 tileSize, Tile[,] around)
        {
            if (AllSprites[0, 0] == null)
                for (int i = 0; i < 20; i++)
                    for (int j = 0; j < 20; j++)
                        AllSprites[j, i] = new Sprite(new Rectangle(16 * i, 13 + 16 * j, 16, 16), new Vector2(0), Color.White);
            Kind = kind;
            TileSize = tileSize;
            if (spawner != 9) {
                Spawner = true;
                SpawnerKind = spawner - 1000;
            }
            TileCenter = new Vector2(0, 0);
            CollisionKind = collision;
        }
        public void Setup(Tile[,] around)
        {

            Sprite = AllSprites[0, 6].Copy();
            if (around == null)
            {
                Sprite.TintColor = COLORS[Kind - 1];
                return;
            }
            Point pos = new Point();
            if (around[1, 2].Kind != 9 && around[1, 2].Kind != 5)
                pos.X += 1;
            if (around[1, 0].Kind != 9 && around[1, 0].Kind != 5)
                pos.X += 2;
            if (around[2, 1].Kind != 9)
                pos.Y += 1;
            if (around[0, 1].Kind != 9)
                pos.Y += 2;
            if (Kind == 1 || Kind == 2) Sprite = AllSprites[5, 3 + pos.X].Copy();
            else if (Kind == 3) Sprite = AllSprites[4 + pos.Y, 7 + pos.X].Copy();
            else if (Kind == 4) Sprite = AllSprites[0 + pos.Y, 11 + pos.X].Copy();
            else if (Kind == 5) Sprite = AllSprites[0, 2].Copy();
            else if (Kind == 7) Sprite = AllSprites[0 + pos.Y, 7 + pos.X].Copy();
            else if (Kind == 8) Sprite = AllSprites[1 + pos.Y, 2].Copy();
            Sprite.TintColor = COLORS[Kind - 1];
        }

        public Tile() { }

        /// <summary>
        /// draws a tile
        /// </summary>
        /// <param name="spriteBatch">the Sprite Batch object</param>
        /// <param name="position">the position of the tile on the map</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool background)
        {
            if (background && Kind == 2)
            {
                Sprite s = AllSprites[3, 14].Copy();
                s.TintColor = Color.Gray;
                UpdatePos(position);
                s.Draw(spriteBatch, Position.Location, 0, Position.Size.ToVector2());

            }
            if (background != (CollisionKind == 9) || Kind == 9)
                return;
            Sprite.TintColor = !background ? Color.White : Color.Gray;
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
            if (Kind == 8)
            {
                UpdatePos(position);
                Sprite sp = new Sprite(TILE_EMPTY, new Vector2(32), Color.Orange);
                sp.Draw(spriteBatch, Position.Location, 0, Position.Size.ToVector2() * 4);
            }
            if (Kind < 9 && !(CollisionKind == 9))
                return;
            UpdatePos(position);
            Sprite s = new Sprite(TILE_EMPTY, new Vector2(32), Color.White);
            s.Draw(spriteBatch, Position.Location, 0, Position.Size.ToVector2() * 4);
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
