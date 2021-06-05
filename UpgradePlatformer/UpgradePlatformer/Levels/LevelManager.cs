using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Input;
using UpgradePlatformer.Upgrade_Stuff;
using UpgradePlatformer.Music;

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
        public List<Texture2D> BackDrops;
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
            Worlds.Add(new World(0, 2, 0, false, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE }));
            Worlds.Add(new World(1, 3, 0, false, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE, UpgradeType.EXTRA_JUMP }));
            Worlds.Add(new World(2, 3, 0, true, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE, UpgradeType.WEAPON, UpgradeType.EXTRA_JUMP }));
        }

        /// <summary>
        /// draws the active level in the active world
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch, bool background)
        {
            if (background)
            {
                Rectangle r = Sprite.GetRect();
                r.Location += new Vector2(0, 40 * Sprite.GetScale()).ToPoint();
                spriteBatch.Draw(BackDrops[activeWorld], r, Color.White);
            }
            Worlds[activeWorld].Draw(spriteBatch, background);
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

            switch (_activeWorld)
            {
                case 1:
                    SoundManager.Instance.PlayMusic("clouds");
                    break;
                case 2:
                    SoundManager.Instance.PlayMusic("cave");
                    break;
            }
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
        /// 
        /// </summary>
        public void Reset()
        {
            Worlds = new List<World>();
            _activeWorld = 0;
            Worlds.Add(new World(new List<string> { "0_1", "0_2" }, 0, false, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE }));
            Worlds.Add(new World(new List<string> { "1_1", "1_2", "1_3" }, 0, false, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE, UpgradeType.EXTRA_JUMP }));
            Worlds.Add(new World(new List<string> { "2_1", "2_2", "2_3", "2_4" }, 0, true, new List<UpgradeType> { UpgradeType.NONE, UpgradeType.NONE, UpgradeType.WEAPON, UpgradeType.EXTRA_JUMP }));
        }

        /// <summary>
        /// adds a tile to the list of collected tiles
        /// </summary>
        /// <param name="t"></param>
        public void Collect(Tile t) {
            ActiveLevel().Collected.Add(new LevelCollectedEntity(t));
        }

        public bool UpdateCheck() => _activeWorld != activeWorld || ActiveWorld().CheckUpdate();

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
