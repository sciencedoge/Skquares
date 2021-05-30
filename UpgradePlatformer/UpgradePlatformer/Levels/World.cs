using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Levels
{
    class World
    {
        public List<Level> Levels;
        int activeLevel;
        int _activeLevel;
        bool Light = false;
        
        /// <summary>
        /// updates the level if needed
        /// </summary>
        public void Update() {
            if (activeLevel != _activeLevel) {
                activeLevel = _activeLevel;
                EntityManager.Instance.Clean(false);
                Levels[activeLevel].LoadEntities(false);
            }
        }

        /// <summary>
        /// loads the entitys in the active level
        /// </summary>
        /// <param name="player">wheter to spawn a player</param>
        public void LoadEntities(bool player) =>
            Levels[activeLevel].LoadEntities(player);

        /// <summary>
        /// creates a world object
        /// </summary>
        /// <param name="levels">the levels in the world</param>
        /// <param name="defaultLevel">the default level</param>
        public World(List<String> levels, int defaultLevel, bool light)
        {
            Light = light;
            Levels = new List<Level>();
            _activeLevel = defaultLevel;
            foreach (string level in levels)
                Load(level);
        }
        
        /// <summary>
        /// loads a level into memory
        /// </summary>
        /// <param name="Name"></param>
        public void Load(String Name)
        {
            Levels.Add(new Level(Name, Light));
        }

        /// <summary>
        /// dtaws the active levl
        /// </summary>
        /// <param name="spriteBatch">a SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Levels[activeLevel].Draw(spriteBatch);
        }

        /// <summary>
        /// draws the active level in the active world
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public void DrawLightMap(SpriteBatch spriteBatch)
        {
            Levels[activeLevel].DrawLightMap(spriteBatch);
        }

        /// <summary>
        /// sets the active level
        /// </summary>
        /// <param name="id">the id of the level</param>
        public void SetLevel(int id) {
            if (id < 0) {
                _activeLevel = Levels.Count - 1;
            }
            else
            _activeLevel = id % (Levels.Count);
        }

        /// <summary>
        /// goes to the next level
        /// </summary>
        public void Next()
        {
            SetLevel(activeLevel + 1);
        }

        /// <summary>
        /// goes to the previous level
        /// </summary>
        public void Prev()
        {
            SetLevel(activeLevel - 1);
        }

        /// <summary>
        /// gets the spawners of the active level
        /// </summary>
        /// <returns>the spawner tiles</returns>
        public List<Tile> GetSpawners() => Levels[activeLevel].GetSpawners();
        /// <summary>
        /// gets collisions for the current level
        /// </summary>
        /// <param name="r">a rectangle to check</param>
        /// <returns>the colliding tiles</returns>
        public List<Tile> GetCollisions(Rectangle r) => Levels[activeLevel].GetCollisions(r);
        /// <summary>
        /// gets the active level
        /// </summary>
        /// <returns>the active level</returns>
        public Level ActiveLevel() => Levels[activeLevel];
        /// <summary>
        /// gets the active levels name
        /// </summary>
        /// <returns>the name</returns>
        public String ActiveLevelName() => Levels[activeLevel].Name;
        /// <summary>
        /// gets the id of the active level
        /// </summary>
        /// <returns>the id</returns>
        public int ActiveLevelNum() => activeLevel;
    }
}
