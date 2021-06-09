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
    class Boss : LivingObject
    {
        // particle stuffs
        private ParticleSystem ps;
        private ParticleProps props;

        private Point spawnPoint;
        private bool currentlyColliding;

        /// <summary>
        /// returns or sets the
        /// spawn point of the Boss
        /// </summary>
        public Point SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }

        /// <summary>
        /// returns or sets whether or not the
        /// Boss is currently colliding with something
        /// </summary>
        public bool Colliding
        {
            get { return currentlyColliding; }
            set { currentlyColliding = value; }
        }
        public bool Flip;

        /// <summary>
        /// creates an Boss object
        /// </summary>
        /// <param name="maxHp">the Boss hp</param>
        /// <param name="damage">the damage the Boss deals</param>
        /// <param name="hitbox">the starting hitbox of the Boss</param>
        /// <param name="jumpsLeft">the amount of jumps the Boss has</param>
        public Boss(int maxHp, int damage, Rectangle hitbox, int jumpsLeft)
            :base(maxHp, damage, hitbox, jumpsLeft, EntityKind.BOSS)
        {
            gravity = new Vector2(0, 0.8f);
            jumpVelocity = new Vector2(0, -22f);
            this.jumpsLeft = jumpsLeft;

            spawnPoint = new Point(hitbox.X, hitbox.Y);
            currentlyColliding = false;
            this.animation = new AnimationFSM(AnimationManager.Instance.animations[3]);
            ps = new ParticleSystem();
        }

        /// <summary>
        /// processes a floor collision
        /// </summary>
        public override void OnFloorCollide()
        {   
            this.jumpsLeft = 1;
            for (int i = 0; i < 30; i++)
            {
                ps.Emit(props);
            }
        }

        /// <summary>
        /// processes gravity for the enemy
        /// </summary>
        public override void ApplyGravity(GameTime gt)
        {
            // fixes different speeds on different window sizes
            position += velocity * ((float)gt.ElapsedGameTime.TotalSeconds * 60f);
            velocity += gravity * ((float)gt.ElapsedGameTime.TotalSeconds * 60f);
            velocity.X *= 0.70f;

            // locks enemy in room
            if (position.X > Sprite.graphics.PreferredBackBufferWidth - hitbox.Width)
                position.X = Sprite.graphics.PreferredBackBufferWidth - hitbox.Width;
            else if (position.X < 0 + hitbox.Width)
                position.X = 0 + hitbox.Width;
        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);
            ps.Draw(sb);
        }

        /// <summary>
        /// processes a frame of movement for an enemy
        /// </summary>
        /// <param name="gameTime">a GameTime object</param>
        public override void Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            cooldown = Math.Max(cooldown, 0);
            if (IsActive)
            {
                hitbox.Location = position.ToPoint();
                spriteSize = hitbox.Size;

                props = new ParticleProps()
                {
                    Position = new Vector2(hitbox.Center.X, hitbox.Bottom),
                    Velocity = new Vector2(0, .5f),
                    VelocityVariation = new Vector2(30f, 0f),
                    StartColor = Color.Gray,
                    EndColor = Color.Black,
                    SizeStart = 7.0f,
                    SizeEnd = 1.0f,
                    LifeTime = 3f
                };
                ps.Update(gameTime);

                //Removing this for now to customize how gravity affects the boss
                //ApplyGravity(gameTime);
            }
        }

        

        

        /// <summary>
        /// processes intersections for an enemy, does nothing
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public override int Intersects(List<EntityObject> objects) {return 0; }

        /// <summary>
        /// removes light for the boss to make it look better
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void DrawLightMap(SpriteBatch spriteBatch) { }
    }
}
