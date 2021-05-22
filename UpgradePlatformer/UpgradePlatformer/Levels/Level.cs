using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpgradePlatformer.Levels
{
    class Level
    {
        int TileWidth, TileHeight;
        TileTheme tileTheme;
        Tile[,] TileMap;

        public void Load(String Name)
        {
            FileStream stream = new FileStream("Content/Levels/" + Name, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();

            for (int x = 0; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y] = new Tile(reader.ReadInt32(), reader.ReadInt32(), 100);
            for (int x = 0; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y].CollisionKind = (TileCollision)reader.ReadInt32();
        }

        public Level(String Name)
        {
            Load(Name);
        }
    }

    enum TileTheme
    {
        UGLY_MAP_IDK,
    }
}
