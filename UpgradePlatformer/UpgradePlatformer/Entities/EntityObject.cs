using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Upgrade_Stuff;

namespace UpgradePlatformer.Entities
{
    abstract class EntityObject {

        public abstract void Update(GameTime gameTime, EventManager eventManager, InputManager inputManager, LevelManager levelManager);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public EntityKind Kind;
        public EntityObject(EntityKind kind) {
            Kind = kind;
        }
    }
    enum EntityKind {
        PLAYER,
        ENEMY,
        COIN,
        UPGRADE,
    }

}