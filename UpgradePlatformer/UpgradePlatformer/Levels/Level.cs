using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public void Load(Texture2D texture, String Name)
        {
            FileStream stream = new FileStream("Content/Levels/" + Name + ".level_Finished", FileMode.Open);
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

        public Level(Texture2D texture, String Name)
        {
            Load(texture, Name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y].Draw(spriteBatch, new Vector2(y * TileWidth, x * TileHeight));
        }
    }

    enum TileTheme
    {
        UGLY_MAP_IDK,
    }
}
