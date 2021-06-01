using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Levels
{
    class Level
    {
        public String Name;
        int TileWidth, TileHeight;
        Tile[,] TileMap;
        public bool Light;
        private UpgradeType upgrade;

        public List<LevelCollectedEntity> Collected;
        public Tile[,] Tiles
        {
            get { return TileMap; }
        }

        /// this is important dont change
        const int DEF_SIZE = 630;

        /// <summary>
        /// Loads a level into the level var
        /// </summary>
        /// <param name="name">the name of the level</param>
        public void Load(String name, bool light)
        {
            Light = light;
            Name = name;
            FileStream stream = new FileStream("Content/Levels/" + name + ".level_Finished", FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();

            Vector2 tileSize = new Vector2(DEF_SIZE / TileWidth, DEF_SIZE / TileHeight);

            TileMap = new Tile[TileWidth, TileHeight];

            Tile empty = new Tile(9, 0, 0, 9, new Vector2(1), null);

            for (int y = 0; y < TileHeight; y++)
                TileMap[0, y] = new Tile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), tileSize, empty);
            
            for (int x = 1; x < TileWidth; x++)
                for (int y = 0; y < TileHeight; y++)
                    TileMap[x, y] = new Tile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), tileSize, TileMap[x - 1, y]);
            reader.Close();
            stream.Close();
        }

        /// <summary>
        /// creates a level from a file
        /// </summary>
        /// <param name="name">the name of the file</param>
        public Level(String name, bool Light, UpgradeType ug)
        {
            Collected = new List<LevelCollectedEntity>();
            upgrade = ug;

            Load(name, Light);
        }

        /// <summary>
        /// draws the level
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch Object</param>
        public void Draw(SpriteBatch spriteBatch, bool background)
        {
            Vector2 tileSize = new Vector2(DEF_SIZE / TileWidth, DEF_SIZE / TileHeight);
            for (int x = 0; x < TileHeight; x++)
                for (int y = 0; y < TileWidth; y++)
                    TileMap[y, x].Draw(spriteBatch, new Vector2(x,y) * tileSize, background);
        }

        /// <summary>
        /// draws the active level in the active world
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public void DrawLightMap(SpriteBatch spriteBatch)
        {
            Vector2 tileSize = new Vector2(DEF_SIZE / TileWidth, DEF_SIZE / TileHeight);
            for (int x = 0; x < TileHeight; x++)
                for (int y = 0; y < TileWidth; y++)
                    TileMap[y, x].DrawLightMap(spriteBatch, new Vector2(x, y) * tileSize);
        }
        /// <summary>
        /// Gets Spawners for the level
        /// </summary>
        /// <returns>the spawners</returns>
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
        /// <param name="RedundancySize">the size of tiles to check</param>
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

        /// <summary>
        /// loads entitys in the current level
        /// </summary>
        /// <param name="player">wether to add a player</param>
        public void LoadEntities(bool player)
        {
            for (int x = 0; x < TileHeight; x++) {
                for (int y = 0; y < TileWidth; y++)
                {
                    Tiles[x, y].UpdatePos(new Vector2((y) * TileMap[0, 0].TileSize.X, (x) * TileMap[0, 0].TileSize.Y));
                    Tile t = Tiles[x, y];
                    if (!t.Spawner)
                        continue;
                    bool q = false;
                    foreach (LevelCollectedEntity e in Collected) {
                        if (e.tile.Position == t.Position) q = true;
                    }
                    if (q) continue;
                    EntityObject o = null;
                    if (t.SpawnerKind == 4) o = (EntityObject)new Torch(0, new Rectangle(t.Position.Location, new Point(15, 15)), t);
                    else if (t.SpawnerKind == 3 && UpgradeManager.Instance.CanBeLearned(upgrade).Count != 0) o = (EntityObject)new Pillar(10, new Rectangle(t.Position.Location.X, t.Position.Y - 15, 15, 15), UpgradeManager.Instance.CanBeLearned(upgrade)[0], t);
#if DEBUG
                    else if (t.SpawnerKind == 2 && player) o = (EntityObject)new Player(30, 0, new Rectangle(t.Position.Location, new Point(25, 25)), 2);
#else
                    else if (t.SpawnerKind == 2 && player) o = (EntityObject)new Player(3, 2, new Rectangle(t.Position.Location, new Point(25, 25)), 2);
#endif
                    else if (t.SpawnerKind == 1) o = (EntityObject)new Enemy(10, 1, new Rectangle(t.Position.Location, new Point(25, 25)), 1);
                    else if (t.SpawnerKind == 0) o = (EntityObject)new Coin(1, new Rectangle(t.Position.Location, new Point(15, 15)), t);
                    if (o != null)
                        EntityManager.Instance.Spawn(o);
                }
            }
        }
    }

    enum TileTheme
    {
        UGLY_MAP_IDK,
    }
}
