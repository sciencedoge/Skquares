using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Input;

namespace UpgradePlatformer.Levels
{
    class LevelManager
    {
        /// <summary>
        /// Singleton Stuff
        /// </summary>
        private static readonly Lazy<LevelManager>
            lazy =
            new Lazy<LevelManager>
                (() => new LevelManager());
        public static LevelManager Instance { get { return lazy.Value; } }

        public List<World> Worlds;
        int activeWorld;
        int _activeWorld;
        
        /// <summary>
        /// Updates the level manager
        /// </summary>
        public void Update() {
            if (activeWorld != _activeWorld) {
                activeWorld = _activeWorld;
                EntityManager.Instance.Clean(true);
                Worlds[activeWorld].LoadEntities(true);
            }
            ActiveWorld().Update();
        }

        /// <summary>
        /// creates a level manager
        /// </summary>
        public LevelManager()
        {
            Worlds = new List<World>();
            _activeWorld = 0;
            Worlds.Add(new World(new List<string>{"DEATH_MENU"}, 0, false));
            Worlds.Add(new World(new List<string>{"clouds2", "clouds1Fix", "clouds3"}, 0, false));
            Worlds.Add(new World(new List<string>{"cave1", "cave2", "cave3", "cave4"}, 0, true));
        }

        /// <summary>
        /// draws the active level in the active world
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Worlds[activeWorld].Draw(spriteBatch);
        }

        /// <summary>
        /// draws the active level in the active world
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public void DrawLightMap(SpriteBatch spriteBatch)
        {
            Worlds[activeWorld].DrawLightMap(spriteBatch);
        }

        /// <summary>
        /// sets the active world
        /// </summary>
        /// <param name="id">the world id</param>
        public void SetWorld(int id) {
            if (id < 0) {
                _activeWorld = Worlds.Count - 1;
            }
            else
            _activeWorld = id % (Worlds.Count);
        }

        /// <summary>
        /// goes to the next world
        /// </summary>
        public void Next()
        {
            SetWorld(activeWorld + 1);
        }

        /// <summary>
        /// goes to the previous world
        /// </summary>
        public void Prev()
        {
            SetWorld(activeWorld - 1);
        }
        
        /// <summary>
        /// adds a tile to the list of collected tiles
        /// </summary>
        /// <param name="t"></param>
        public void Collect(Tile t) {
            ActiveLevel().Collected.Add(new LevelCollectedEntity(t));
        }

        /// <summary>
        /// sets the active level
        /// </summary>
        /// <param name="id">the id of the level</param>
        public void SetLevel(int id) => ActiveWorld().SetLevel(id);
        /// <summary>
        /// gets the spawners in the active level
        /// </summary>
        /// <returns>the spawners</returns>
        public List<Tile> Spawners() => Worlds[activeWorld].GetSpawners();
        /// <summary>
        /// gets collisions for the level
        /// </summary>
        /// <param name="r">the rectangle of tiles</param>
        /// <returns></returns>
        public List<Tile> GetCollisions(Rectangle r) => Worlds[activeWorld].GetCollisions(r);
        /// <summary>
        /// gets the active level
        /// </summary>
        /// <returns>the level</returns>
        public Level ActiveLevel() => ActiveWorld().ActiveLevel();
        /// <summary>
        /// gets the active world
        /// </summary>
        /// <returns>the world</returns>
        public World ActiveWorld() => Worlds[activeWorld];
        /// <summary>
        /// gets the active level name
        /// </summary>
        /// <returns>the active levels name</returns>
        public String ActiveLevelName() => ActiveWorld().ActiveLevelName();
        /// <summary>
        /// gets the id of the active level
        /// </summary>
        /// <returns>the id</returns>
        public int ActiveLevelNum() => ActiveWorld().ActiveLevelNum();
        /// <summary>
        /// gets the id of the active world
        /// </summary>
        /// <returns>the id</returns>
        public int ActiveWorldNum() => activeWorld;
        public bool Light => ActiveLevel().Light;
    }
}
