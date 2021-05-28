using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Input;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
    //Header=====================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for enemies
    //===========================================
    class Enemy : LivingObject
    {
        private Point spawnPoint;
        private bool currentlyColliding;

        /// <summary>
        /// returns or sets the
        /// spawn point of the enemy
        /// </summary>
        public Point SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }

        /// <summary>
        /// returns or sets whether or not the
        /// enemy is currently colliding with something
        /// </summary>
        public bool Colliding
        {
            get { return currentlyColliding; }
            set { currentlyColliding = value; }
        }
        public bool Flip;

        /// <summary>
        /// Creates an enemy object
        /// </summary>
        /// <param name="maxHp">the max hp of the enemy</param>
        /// <param name="damage">the damage that the enemy deals</param>
        /// <param name="hitbox">the hitbox of the enemy</param>
        /// <param name="texture">the texture of the enemy</param>
        public Enemy(int maxHp, int damage, Rectangle hitbox, int jumpsLeft)
            :base(maxHp, damage, hitbox, jumpsLeft, EntityKind.ENEMY)
        {
            gravity = new Vector2(0, 0.2f);
            jumpVelocity = new Vector2(0, -2f);
            this.jumpsLeft = jumpsLeft;

            spawnPoint = new Point(hitbox.X, hitbox.Y);
            currentlyColliding = false;
            this.sprite.Position = new Rectangle(0, 29, 15, 15);
        }
        public override void OnFloorCollide()
        {
            this.jumpsLeft = 1;
        }

        public override void ApplyGravity()
        {
            base.ApplyGravity();
            if (position.Y > Sprite.graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = Sprite.graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;

                jumpsLeft = 1;
                this.Colliding = false;
            }

            if (position.X > Sprite.graphics.PreferredBackBufferWidth + hitbox.Width)
                position.X = 0 - hitbox.Width;

            if (position.X < 0 - hitbox.Width)
                position.X = Sprite.graphics.PreferredBackBufferWidth + hitbox.Width;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                hitbox.Location = position.ToPoint();
                spriteSize = hitbox.Size;
                ApplyGravity();
                sprite.effects = SpriteEffects.None;
                if (Flip)
                    sprite.effects = SpriteEffects.FlipHorizontally;
            }
        }
        public override int Intersects(List<EntityObject> objects) {return 0; }
    }
}
