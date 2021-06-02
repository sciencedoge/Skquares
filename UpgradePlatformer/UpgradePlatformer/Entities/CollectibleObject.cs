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
        protected Rectangle SpriteBounds;
        protected int value;
        protected int Bob;
        protected Graphics.Sprite sprite;
        protected Point spriteSize;

        protected Tile tile;

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
        public CollectibleObject(int value, Rectangle hitbox, EntityKind kind, Tile t) : base(kind)
        {
            this.value = value;
            this.hitbox = hitbox;
            this.IsActive = true;
            this.spriteSize = hitbox.Size;
            this.Bob = 3;
            tile = t;
        }
        
        /// <summary>
        /// updates the bounds of the sprite
        /// </summary>
        public void UpdateSprite() {
            this.sprite = new Sprite(
                SpriteBounds,
                new Vector2(0, 0),
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
                Vector2 frameBob = new Vector2(0, Bob * (float)Math.Cos(gt.TotalGameTime.TotalMilliseconds / 100));
                sprite.Draw(sb, hitbox.Location + frameBob.ToPoint(), 0, spriteSize.ToVector2());
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
                    LevelManager.Instance.Collect(tile);
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// updates the sprite
        /// </summary>
        /// <param name="gameTime">a GameTime object</param>
        public override void Update(GameTime gameTime) { }

        /// <summary>
        /// processes intersections with objects
        /// </summary>
        /// <param name="objects">all entity objects</param>
        /// <returns>the ammount of money earned</returns>
        public override int Intersects(List<EntityObject> objects) {
            foreach (EntityObject o in objects) {
                if (o.Kind == EntityKind.PLAYER)
                    return Intersects(o);
            }
            return 0;
        }
    }
}
