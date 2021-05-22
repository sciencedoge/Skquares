using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Levels
{
    class LevelManager
    {
        List<Level> Levels;
        int activeLevel;
        
        public LevelManager()
        {
            activeLevel = 0;
            Load("EGGMAN");
        }

        public void Load(String Name)
        {
            Levels.Add(new Level(Name));
        }
    }
}
