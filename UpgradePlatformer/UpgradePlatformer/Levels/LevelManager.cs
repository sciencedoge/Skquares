using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Levels
{
    class LevelManager
    {
        List<Level> Levels;
        int activeLevel;
        int _activeLevel;
        
        public void Update() {
            activeLevel = _activeLevel;
        }

        public LevelManager(Texture2D texture, GraphicsDeviceManager graphics)
        {
            Levels = new List<Level>();
            _activeLevel = 0;
            Load(texture, "menu", graphics);
            Load(texture, "MARO", graphics);
            Load(texture, "collision test", graphics);
            Load(texture, "EGGMAN", graphics);
            Load(texture, "EGGMEN", graphics);       
        }
        
        public void Load(Texture2D texture, String Name, GraphicsDeviceManager graphics)
        {
            Levels.Add(new Level(texture, Name, graphics));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Levels[activeLevel].Draw(spriteBatch);
        }

        public void SetLevel(int id) {
            if (id < 0) {
                _activeLevel = Levels.Count - 1;
            }
            else
            _activeLevel = id % (Levels.Count);
        }

        public void Next()
        {
            SetLevel(activeLevel + 1);
        }

        public void Prev()
        {
            SetLevel(activeLevel - 1);
        }

        public List<Tile> GetCollisions(Rectangle r) => Levels[activeLevel].GetCollisions(r);
        public Level ActiveLevel() => Levels[activeLevel];
        public String ActiveLevelName() => Levels[activeLevel].Name;
        public int ActiveLevelNum() => activeLevel;
    }
}
