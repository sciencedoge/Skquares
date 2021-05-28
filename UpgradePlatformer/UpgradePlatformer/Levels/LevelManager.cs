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
        private static readonly Lazy<LevelManager>
            lazy =
            new Lazy<LevelManager>
                (() => new LevelManager());
        public static LevelManager Instance { get { return lazy.Value; } }
        List<World> Worlds;
        int activeWorld;
        int _activeWorld;
        
        public void Update() {
            if (activeWorld != _activeWorld) {
                activeWorld = _activeWorld;
                EntityManager.Instance.Clean(true);
                Worlds[activeWorld].LoadEntities(true);
            }
            ActiveWorld().Update();
        }

        public LevelManager()
        {
            Worlds = new List<World>();
            _activeWorld = 0;
            Load(new List<string>{"DEATH_MENU"}, 0);
            Load(new List<string>{"clouds2", "clouds1", "clouds3"}, 0);
            Load(new List<string>{"cave", "coinHeaven"}, 1);
        }

        public void Load(List<String> Names, int level)
        {
            Worlds.Add(new World(Names, level));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Worlds[activeWorld].Draw(spriteBatch);
        }

        public void SetWorld(int id) {
            if (id < 0) {
                _activeWorld = Worlds.Count - 1;
            }
            else
            _activeWorld = id % (Worlds.Count);
        }

        public void Next()
        {
            SetWorld(activeWorld + 1);
        }

        public void Prev()
        {
            SetWorld(activeWorld - 1);
        }
        
        /// <summary>
        /// adds a tile to the list
        /// </summary>
        /// <param name="t"></param>
        public void Collect(Tile t) {
            ActiveLevel().Collected.Add(new LevelCollectedEntity(t));
        }

        public void SetLevel(int id) => ActiveWorld().SetLevel(id);
        public List<Tile> spawners() => Worlds[activeWorld].GetSpawners();
        public List<Tile> GetCollisions(Rectangle r) => Worlds[activeWorld].GetCollisions(r);
        public Level ActiveLevel() => ActiveWorld().ActiveLevel();
        public World ActiveWorld() => Worlds[activeWorld];
        public String ActiveLevelName() => ActiveWorld().ActiveLevelName();
        public int ActiveLevelNum() => ActiveWorld().ActiveLevelNum();
        public int ActiveWorldNum() => activeWorld;
    }
}
