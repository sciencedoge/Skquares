using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Entities
{
    //HEADER=======================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for coins - 
    //a basic collectible
    //=============================================
    class Torch : CollectibleObject
    {
        private ParticleSystem ps;
        private ParticleProps props;

        //Constructor
        public Torch(int value, Rectangle hitbox, Tile t)
            : base(value, hitbox, EntityKind.TORCH, t) 
        {
            SpriteBounds = new Rectangle(80, 13, 16, 16);
            UpdateSprite();
            Bob = 0;
            this.hitbox = t.Position;
            ps = new ParticleSystem();
            props = new ParticleProps()
            {
                Position = new Vector2(hitbox.Center.X + 3, hitbox.Location.Y),
                Velocity = new Vector2(0, -5f),
                VelocityVariation = new Vector2(1.5f, 1.5f),
                StartColor = Color.Orange,
                EndColor = Color.Black,
                SizeStart = 5.0f,
                SizeEnd = 1.0f,
                LifeTime = 5.0f
            };
        }

        double t = 0;

        /// <summary>
        /// updates the Coin
        /// </summary>
        public override void Update(GameTime gt)
        {
            ps.Update(gt);
            spriteSize = hitbox.Size;

            t += gt.ElapsedGameTime.TotalMilliseconds;

            while ( t > 1000 )
            {
                t -= 1000;
                ps.Emit(props);
            }
        }

        /// <summary>
        /// draws the particles
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gt"></param>
        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);
            ps.Draw(sb);
        }

        public override int Intersects(EntityObject o)
        {
            return 0;
        }
    }
}
