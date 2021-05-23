﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpgradePlatformer.Levels
{
    class Level
    {
        public String Name;

        int TileWidth, TileHeight;
        TileTheme tileTheme;
        Tile[,] TileMap;

        public Tile[,] Tiles
        {
            get { return TileMap; }
        }

        public void Load(Texture2D texture, String name)
        {
            Name = name;
            FileStream stream = new FileStream("Content/Levels/" + name + ".level_Finished", FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();

            TileMap = new Tile[TileWidth, TileHeight];

            for (int x = 0; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y] = new Tile(texture, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            reader.Close();
            stream.Close();
        }

        public Level(Texture2D texture, String name)
        {
            Load(texture, name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y].Draw(spriteBatch, new Vector2(y * (Tile.WINDOW_SIZE / 30) + 8, x * (Tile.WINDOW_SIZE / 30) + 8));
        }

        /// <summary>
        /// colliding rectangles for the map
        /// </summary>
        /// <param name="r">the rectangle to check</param>
        /// <returns>a list of colliding rectangles</returns>
        public List<Tile> GetCollisions(Rectangle r, int RedundancySize = 4)
        {
            List<Tile> Tiles = new List<Tile>();

            Point centerTile = r.Center / new Point(Tile.WINDOW_SIZE / 30);

            for (int x = Math.Max(centerTile.X - RedundancySize, 0); x < Math.Min(centerTile.X + RedundancySize, TileWidth); x++)
                for (int y = Math.Max(centerTile.Y - RedundancySize, 0); y < Math.Min(centerTile.Y + RedundancySize, TileHeight); y++)
                    if (TileMap[x, y].Position.Intersects(r)) Tiles.Add(TileMap[x, y]);
            return Tiles;
        }
    }

    enum TileTheme
    {
        UGLY_MAP_IDK,
    }
}
