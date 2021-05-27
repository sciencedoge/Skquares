using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Entities
{
    //HEADER==========================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for collectibles
    //================================================
    class CollectibleObject : EntityObject
    {
        //Fields
        private Rectangle SpriteBounds = new Rectangle(0, 7, 5, 5);
        protected int value;
        protected Graphics.Sprite sprite;
        protected Rectangle hitbox;
        protected Point spriteSize;

        //properties
        /// <summary>
        /// returns the value of the
        /// collectible
        /// </summary>
        public int Value
        {
            get { return value; }
        }

        /// <summary>
        /// returns the hitbox of the object
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        //Const
        /// <summary>
        /// Creates a CollectibleObject object
        /// </summary>
        public CollectibleObject(int value, Rectangle hitbox, EntityKind kind) : base(kind)
        {
            this.value = value;
            this.hitbox = hitbox;
            this.IsActive = true;
            this.spriteSize = hitbox.Size;
            this.sprite = new Sprite(
                SpriteBounds,
                new Vector2(SpriteBounds.X - (SpriteBounds.Width / 2),
                SpriteBounds.Y - (SpriteBounds.Height / 2)),
                Color.White);
        }

        /// <summary>
        /// Draws the sprite to the screen
        /// </summary>
        /// <param name="sb">_spriteBatch</param>
        /// <param name="gt">gameTime</param>
        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            //if the sprite is active, draws it to the screen
            if (IsActive)
            {
                Vector2 Bob = new Vector2(0, 3 * (float)Math.Cos(gt.TotalGameTime.TotalMilliseconds / 100));
                sprite.Draw(sb, hitbox.Location + Bob.ToPoint() - new Point(hitbox.Width, hitbox.Height / 2), 0, spriteSize.ToVector2());
            }
        }

        /// <summary>
        /// Checks if an intersection happened with a LivingObject
        /// (will be changed to player when implemented)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Intersects(EntityObject o)
        {
            if (o.Kind != EntityKind.PLAYER) return 0;
            LivingObject obj = (LivingObject)o;
            if (IsActive && obj != null)
            {
                if (this.hitbox.Intersects(obj.Hitbox))
                {
                    //the coin is no longer active
                    IsActive = false;
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public override void Update(GameTime gameTime) { }
        public override int Intersects(List<EntityObject> objects) {
            int result = 0;
            foreach (EntityObject o in objects) result += Intersects(o);
            return result;
        }
    }
}
