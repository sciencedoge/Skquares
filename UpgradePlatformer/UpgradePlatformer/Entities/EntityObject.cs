using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Upgrade_Stuff;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
    abstract class EntityObject {

        public bool IsActive;
        public abstract void Update(GameTime gameTime, EventManager eventManager, InputManager inputManager, LevelManager levelManager,
            SoundManager soundManager);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract int Intersects(List<EntityObject> objects);
        public EntityKind Kind;
        public EntityObject(EntityKind kind) {
            Kind = kind;
            IsActive = true;
        }
    }
    enum EntityKind {
        PLAYER,
        ENEMY,
        COIN,
        UPGRADE,
    }

}