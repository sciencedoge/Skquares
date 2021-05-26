﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Levels
{
    class World
    {
        List<Level> Levels;
        int activeLevel;
        int _activeLevel;
        
        public void Update(EntityManager em, Texture2D texture, GraphicsDeviceManager device) {
            if (activeLevel != _activeLevel) {
                activeLevel = _activeLevel;
                em.Clean();
                Levels[activeLevel].LoadEntities(em, texture, device);
            }
        }

        public void LoadEntities(EntityManager em, Texture2D texture, GraphicsDeviceManager device) =>
            Levels[activeLevel].LoadEntities(em, texture, device);
        public World(Texture2D texture, List<String> levels, GraphicsDeviceManager graphics, int defaultLevel)
        {
            Levels = new List<Level>();
            _activeLevel = defaultLevel;
            foreach (string level in levels)
                Load(texture, level, graphics);
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

        public List<Tile> GetSpawners() => Levels[activeLevel].GetSpawners();
        public List<Tile> GetCollisions(Rectangle r) => Levels[activeLevel].GetCollisions(r);
        public Level ActiveLevel() => Levels[activeLevel];
        public String ActiveLevelName() => Levels[activeLevel].Name;
        public int ActiveLevelNum() => activeLevel;
    }
}
