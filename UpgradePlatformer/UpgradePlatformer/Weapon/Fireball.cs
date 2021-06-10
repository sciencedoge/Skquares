using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Weapon
{
    //HEADER===========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 6/4/2021
    //Purpose: Creates the fireball boss projectile
    //=================================================
    class Fireball : Projectile
    {
        // particle stuffs
        private ParticleSystem ps;
        private ParticleProps props;

        /// <summary>
        /// Creates a fireball object
        /// </summary>
        public Fireball(Vector2 path, Vector2 location, float rotation)
            :base(path, location, rotation, 10000)
        {
            spriteBounds = new Rectangle(28, 0, 7, 7);
            UpdateSprite();
            ps = new ParticleSystem();
            props = new ParticleProps()
            {
                Position = new Vector2(hitbox.Center.X + 2, hitbox.Location.Y),
                Velocity = 0.1f * path,
                VelocityVariation = new Vector2(1.5f, 1.5f),
                StartColor = Color.Orange,
                EndColor = Color.Black,
                SizeStart = 7.0f,
                SizeEnd = 1.0f,
                LifeTime = 0.5f
            };
        }

        double t = 0;
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            ps.Update(gt);

            t += gt.ElapsedGameTime.TotalMilliseconds;
            while (t > 100)
            {
                props.Position = new Vector2(hitbox.Center.X, hitbox.Center.Y);
                ps.Emit(props);
                t -= 100;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            ps.Draw(sb);
        }

        /// <summary>
        /// Checks for intersections
        /// </summary>
        public override void Intersects()
        {
            Player p = EntityManager.Instance.Player();

            if (hitbox.Intersects(p.Hitbox))
            {
                if (p.IsActive)
                {
                    p.TakeDamage(2);
                    SoundManager.Instance.PlaySFX("damage");

                    isActive = false;
                    if (p.CurrentHP <= 0)
                    {
                        p.IsActive = false;
                    }
                    return;
                }
            }
        }
    }
}