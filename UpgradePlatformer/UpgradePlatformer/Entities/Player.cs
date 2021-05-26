using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;

namespace UpgradePlatformer.Entities
{
    //Header=====================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/22/2021
    //Purpose: Creates functionality for players
    //===========================================
    class Player : LivingObject
    {

        //Fields
        private const int NORMAL_HITBOX_WIDTH = 25, NORMAL_HITBOX_HEIGHT = 25, DUCK_HITBOX_HEIGHT = 15, DUCK_HITBOX_WIDTH = 27;
        private bool keyUp, keyDown, keyLeft, keyRight;
        private bool ducking;

        private static int maxJumps = 1;

        //screen bounds stuff
        private GraphicsDeviceManager _graphics;


        /// <summary>
        /// Creates a player object
        /// </summary>
        public Player(int maxHp, int damage, Rectangle hitbox,
            Texture2D texture, GraphicsDeviceManager device, int jumpsLeft)
            : base(maxHp, damage, hitbox, texture, jumpsLeft) 
        {
            this.jumpsLeft = jumpsLeft;

            this._graphics = device;
        }

        //Methods

        /// <summary>
        /// Checks if the player intersects with another living object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Intersects(List<Enemy> obj)
        {
            if (isActive)
            {
                foreach(Enemy enemy in obj)
                {
                    if (this.hitbox.Intersects(enemy.Hitbox))
                    {
                        this.TakeDamage(enemy.Damage);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the player's position based on the
        /// key pressed
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="keys"></param>
        public void Update(GameTime gt, EventManager eventManager, InputManager inputManager, LevelManager levelManager)
        {
            if (isActive)
            {
                CheckForInput(inputManager, eventManager);

                if (keyDown)
                {
                    ducking = true;
                }
                else
                {
                    ducking = false;
                    if (keyRight)
                    {
                        velocity.X += speedX;
                    }

                    if (keyLeft)
                    {
                        velocity.X -= speedX;
                    }

                    if (keyUp)
                    {
                        //check for ground collision
                        if (jumpsLeft > 0 && velocity.Y >= -4f)
                        {
                            velocity.Y += jumpVelocity.Y;
                        }
                        else if (!(velocity.Y >= -4f))
                        {
                            keyUp = false;
                            jumpsLeft -= 1;
                        }
                    }
                }
                Update(gt, levelManager, eventManager);
            }           
        }
        
        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="gt"></param>
        public override void Update(GameTime gt, LevelManager lm, EventManager em)
        {
            ApplyGravity(lm, em);
            hitbox.Location = position.ToPoint();
            if (ducking)
            {
                hitbox.Size = new Point(DUCK_HITBOX_WIDTH, DUCK_HITBOX_HEIGHT);
            }
            else hitbox.Size = (new Point(NORMAL_HITBOX_WIDTH, NORMAL_HITBOX_HEIGHT));
            spriteSize = (hitbox.Size.ToVector2() * GetVelocitySize()).ToPoint(); 
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public override void ApplyGravity(LevelManager lm, EventManager em)
        {
            base.ApplyGravity(lm, em);

            if (position.Y > _graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = _graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;
            }

            if (position.X > _graphics.PreferredBackBufferWidth) {
                em.Push(new Event("LEVEL_SHOW", (uint)lm.ActiveLevelNum() + 1, new Point()));
                position.X = 0 + hitbox.Width;
            }

            if (position.X < 0) {
                em.Push(new Event("LEVEL_SHOW", (uint)lm.ActiveLevelNum() - 1, new Point()));
                position.X = _graphics.PreferredBackBufferWidth - hitbox.Width;
            }

            if (position.Y >= _graphics.PreferredBackBufferHeight - hitbox.Height) jumpsLeft = 2;
        }
        
        public Vector2 GetVelocitySize()
        {
            Vector2 result = new Vector2(1, 1);
            if (velocity.Y != 0) result.Y = 1 / (((Math.Abs(velocity.X)) + 1) / 4);
            if (result.Y > 1) result.Y = 1;
            if (velocity.X != 0) result.X = 1 / (((Math.Abs(velocity.Y)) + 1) / 4);
            if (result.X > 1) result.X = 1;
            return result;
        }

        public override void OnFloorCollide()
        {
            jumpsLeft = maxJumps;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckForInput(InputManager inputManager, EventManager eventManager)
        {
            inputManager.Update(eventManager);
            Event dev = eventManager.Pop("KEY_DOWN");
            Event uev = eventManager.Pop("KEY_UP");
            if (dev != null)
            {
                Keys down = (Keys)dev.Data;
                if (down == Keys.W) keyUp = true;
                else if (down == Keys.A) { keyLeft = true; sprite.effects = SpriteEffects.None; }
                else if (down == Keys.S) keyDown = true;
                else if (down == Keys.D) { keyRight = true; sprite.effects = SpriteEffects.FlipHorizontally; }
            }
            if (uev != null)
            {
                Keys up = (Keys)uev.Data;
                if (keyUp && up == Keys.W) { keyUp = false; jumpsLeft -= 1; }
                else if (up == Keys.A) keyLeft = false; 
                else if (up == Keys.S) keyDown = false;
                else if (up == Keys.D) keyRight = false;
            }
        }
        
        /// <summary>
        /// Respawns the player
        /// </summary>
        public void Respawn()
        {
            this.currentHp = maxHp;
            this.isActive = true;
            resetPosition();
        }
    }
}
