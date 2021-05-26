using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Levels
{
    class Level
    {
        public String Name;
        int TileWidth, TileHeight;
        Tile[,] TileMap;
        GraphicsDeviceManager Graphics;

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

            Vector2 tileSize = new Vector2(Graphics.PreferredBackBufferWidth / TileWidth, (Graphics.PreferredBackBufferHeight - 40) / TileHeight);

            TileMap = new Tile[TileWidth, TileHeight];

            Tile empty = new Tile(texture, 9, 0, 0, 9, new Vector2(1), null);

            for (int y = 0; y < TileHeight; y++)
                TileMap[0, y] = new Tile(texture, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), tileSize, empty);
            
            for (int x = 1; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y] = new Tile(texture, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), tileSize, TileMap[x - 1, y]);
            reader.Close();
            stream.Close();
        }

        public Level(Texture2D texture, String name, GraphicsDeviceManager graphics)
        {
            Graphics = graphics;
            Load(texture, name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < TileHeight; x++)
                for (int y = 0; y < TileWidth; y++)
                    TileMap[y, x].Draw(spriteBatch, new Vector2((x) * TileMap[0, 0].TileSize.X, (y) * TileMap[0, 0].TileSize.Y) + new Vector2());
        }

        public List<Tile> GetSpawners()
        {
            List<Tile> result = new List<Tile>();
            for (int x = 0; x < TileHeight; x++)
                for (int y = 0; y < TileWidth; y++)
                    if (TileMap[x, y].Spawner) result.Add(TileMap[x, y]);
            return result;
        }

        /// <summary>
        /// colliding rectangles for the map
        /// </summary>
        /// <param name="r">the rectangle to check</param>
        /// <returns>a list of colliding rectangles</returns>
        public List<Tile> GetCollisions(Rectangle r, int RedundancySize = 1)
        {
            List<Tile> Tiles = new List<Tile>();

            Point centerTile = r.Center / TileMap[0, 0].TileSize.ToPoint();

            for (int x = Math.Max(centerTile.X - RedundancySize, 0); x < Math.Min(centerTile.X + RedundancySize, TileWidth); x++)
                for (int y = Math.Max(centerTile.Y - RedundancySize, 0); y < Math.Min(centerTile.Y + RedundancySize, TileHeight); y++)
                    if (TileMap[y, x].Position.Intersects(r)) Tiles.Add(TileMap[y, x]);
            return Tiles;
        }

        public void LoadEntities(EntityManager em, Texture2D texture, GraphicsDeviceManager device, bool player)
        {
            for (int x = 0; x < TileHeight; x++) {
                for (int y = 0; y < TileWidth; y++)
                {
                    Tiles[x, y].UpdatePos(new Vector2((y) * TileMap[0, 0].TileSize.X, (x) * TileMap[0, 0].TileSize.Y));
                    Tile t = Tiles[x, y];
                    if (!t.Spawner)
                        continue;
                    EntityObject o = null;
                    if (t.Kind == 2 && player) o = (EntityObject)new Player(int.MaxValue, 2, new Rectangle(t.Position.Location, new Point(25, 25)), texture, device, 2);
                    else if (t.Kind == 1) o = (EntityObject)new Enemy(10, 2, new Rectangle(t.Position.Location, new Point(25, 25)), texture, device, 1);
                    else if (t.Kind == 0) o = (EntityObject)new Coin(1, texture, new Rectangle(t.Position.Location, new Point(15, 15)));
                    if (o != null)
                        em.Spawn(o, t.Kind);
                }
            }
        }
    }

    enum TileTheme
    {
        UGLY_MAP_IDK,
    }
}
