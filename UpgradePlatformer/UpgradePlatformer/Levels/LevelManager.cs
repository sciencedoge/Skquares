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
        
        public LevelManager(Texture2D texture)
        {
            Levels = new List<Level>();
            activeLevel = 0;
            Load(texture, "EGGMAN");
            Load(texture, "EGGMEN");
            Load(texture, "RUN");
        }

        public void Load(Texture2D texture, String Name)
        {
            Levels.Add(new Level(texture, Name));
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
    }
}
