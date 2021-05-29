using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Levels
{
    /// <summary>
    /// stores collected entitys within levels
    /// </summary>
    class LevelCollectedEntity
    {
        public Tile tile;
        public LevelCollectedEntity(Tile t) {
            tile = t;
        }
    }
}