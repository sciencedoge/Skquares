using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public uint lastWorld;
    }
}