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
        /// creates an enemy object
        /// </summary>
        /// <param name="maxHp">the enemys hp</param>
        /// <param name="damage">the damage the enemy deals</param>
        /// <param name="hitbox">the starting hitbox of the enemy</param>
        /// <param name="jumpsLeft">the ammount of jumps the enemy has</param>
        public Enemy(int maxHp, int damage, Rectangle hitbox, int jumpsLeft)
            :base(maxHp, damage, hitbox, jumpsLeft, EntityKind.ENEMY)
        {
            this.jumpsLeft = jumpsLeft;

            spawnPoint = new Point(hitbox.X, hitbox.Y);
            currentlyColliding = false;
            this.animation = new AnimationFSM(AnimationManager.Instance.animations[1]);
        }

        /// <summary>
        /// processes a floor collision
        /// </summary>
        public override void OnFloorCollide()
        {   
            this.jumpsLeft = 1;
        }

        /// <summary>
        /// processes gravity for the enemy
        /// </summary>
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

          //// locks enemy in room
            if (position.X > Sprite.graphics.PreferredBackBufferWidth - hitbox.Width)
                position.X = Sprite.graphics.PreferredBackBufferWidth - hitbox.Width;
            else if (position.X < 0 + hitbox.Width)
                position.X = 0 + hitbox.Width;
        }
        
        /// <summary>
        /// processes a frame of movement for an enemy
        /// </summary>
        /// <param name="gameTime">a GameTime object</param>
        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                hitbox.Location = position.ToPoint();
                spriteSize = hitbox.Size;
                ApplyGravity();
            }
        }

        /// <summary>
        /// processes intersections for an enemy, does nothing
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public override int Intersects(List<EntityObject> objects) {return 0; }
    }
}
