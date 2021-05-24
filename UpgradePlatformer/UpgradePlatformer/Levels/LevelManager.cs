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
        
        public LevelManager(Texture2D texture, GraphicsDeviceManager graphics)
        {
            Levels = new List<Level>();
            activeLevel = 0;
            Load(texture, "menu", graphics);
            Load(texture, "collision test", graphics);
            Load(texture, "EGGMAN", graphics);
            Load(texture, "EGGMEN", graphics);
            Load(texture, "MARO", graphics);            
        }
        
        public void Load(Texture2D texture, String Name, GraphicsDeviceManager graphics)
        {
            Levels.Add(new Level(texture, Name, graphics));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Levels[activeLevel].Draw(spriteBatch);
        }

        public void Next()
        {
            activeLevel++;
            activeLevel = activeLevel % Levels.Count;
        }
        public void Prev()
        {
            activeLevel--;
            if (activeLevel < 0)
                activeLevel = Levels.Count - 1;
        }

        public List<Tile> GetCollisions(Rectangle r) => Levels[activeLevel].GetCollisions(r);
        public Level ActiveLevel() => Levels[activeLevel];
        public String ActiveLevelName() => Levels[activeLevel].Name;
        public int ActiveLevelNum() => activeLevel;
    }
}
