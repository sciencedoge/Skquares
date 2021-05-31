using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Saves
{
    /// <summary>
    /// Container for your save game data.
    /// Put the variables you need here, as long as it's serializable.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public bool muted;
        public bool valid;
        public bool fullscreen;
        public uint lastWorld;
        public int money;
        public List<LevelCollectedEntity> collectedEntities;
    }
}