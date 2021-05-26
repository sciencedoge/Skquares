using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Levels
{
    class LevelManager
    {
        List<World> Worlds;
        int activeWorld;
        int _activeWorld;
        
        public void Update(EntityManager em, Texture2D texture, GraphicsDeviceManager device) {
            if (activeWorld != _activeWorld) {
                activeWorld = _activeWorld;
                Worlds[activeWorld].Update(em, texture, device);
                em.Clean();
                Worlds[activeWorld].LoadEntities(em, texture, device);
            }
            ActiveWorld().Update(em, texture, device);
        }

        public LevelManager(Texture2D texture, GraphicsDeviceManager graphics)
        {
            Worlds = new List<World>();
            _activeWorld = 0;
            Load(texture, new List<string>{"DEATH_MENU"}, graphics, 0);
            Load(texture, new List<string>{"cave"}, graphics, 0);
            Load(texture, new List<string>{"clouds2", "clouds1", "clouds3"}, graphics, 0);
        }
        
        public void Load(Texture2D texture, List<String> Names, GraphicsDeviceManager graphics, int level)
        {
            Worlds.Add(new World(texture, Names, graphics, level));
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

        public void SetLevel(int id) => ActiveWorld().SetLevel(id);
        public List<Tile> spawners() => Worlds[activeWorld].GetSpawners();
        public List<Tile> GetCollisions(Rectangle r) => Worlds[activeWorld].GetCollisions(r);
        public Level ActiveLevel() => ActiveWorld().ActiveLevel();
        public World ActiveWorld() => Worlds[activeWorld];
        public String ActiveLevelName() => ActiveWorld().ActiveLevelName();
        public int ActiveLevelNum() => activeWorld;
    }
}
