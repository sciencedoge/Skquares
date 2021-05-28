﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Music;
using UpgradePlatformer.Upgrade_Stuff;

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
        private Vector2 Joystick;
        private bool ducking;

        private static int maxJumps => UpgradeManager.Instance.GetAmmnt(UpgradeType.XtraJump) + 1;

        /// <summary>
        /// returns whether or not the player is ducking
        /// </summary>
        public bool Ducking
        {
            get { return ducking; }
        }

        /// <summary>
        /// Creates a player object
        /// </summary>
        public Player(int maxHp, int damage, Rectangle hitbox, int jumpsLeft)
            : base(maxHp, damage, hitbox, jumpsLeft, EntityKind.PLAYER)
        {
            this.jumpsLeft = jumpsLeft;
        }

        //Methods

        /// <summary>
        /// Checks if the player intersects with another living object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int Intersects(List<EntityObject> obj)
        {
            if (IsActive)
            {
                foreach(EntityObject o in obj)
                {
                    if (o == null) continue;
                    if (o.Kind != EntityKind.ENEMY)
                        continue;
                    if (this.hitbox.Intersects(((Enemy)o).Hitbox))
                    {
                        this.TakeDamage(((Enemy)o).Damage);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Updates the player's position based on the
        /// key pressed
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="keys"></param>
        public override void Update(GameTime gt)
        {
            if (IsActive)
            {
                cooldown--;
                cooldown = Math.Max(cooldown, 0);

                CheckForInput();

                if (keyDown)
                {
                    ducking = true;
                }
                else
                {
                    ducking = false;
                    if (keyRight)
                    {
                        Joystick.X = 1;
                    }
                    if (keyLeft)
                    {
                        Joystick.X = -1;
                    }
                    velocity.X += speedX * Joystick.X;

                    if (keyUp)
                    {
                        if(jumpsLeft > 0)
                        {
                            SoundManager.Instance.PlaySFX("jump");
                        }                       
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
                ApplyGravity();
                hitbox.Location = position.ToPoint();
                if (ducking)
                {
                    hitbox.Size = new Point(DUCK_HITBOX_WIDTH, DUCK_HITBOX_HEIGHT);
                }
                else hitbox.Size = (new Point(NORMAL_HITBOX_WIDTH, NORMAL_HITBOX_HEIGHT));
                spriteSize = (hitbox.Size.ToVector2() * GetVelocitySize()).ToPoint();
            }           
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public override void ApplyGravity()
        {
            base.ApplyGravity();

            if (position.Y > Sprite.graphics.PreferredBackBufferHeight - hitbox.Height + 9)
            {
                position.Y = Sprite.graphics.PreferredBackBufferHeight - hitbox.Height + 9;
                velocity.Y = 0;
            }

            if (position.X > Sprite.graphics.PreferredBackBufferWidth) {
                EventManager.Instance.Push(new Event("LEVEL_SHOW", (uint)LevelManager.Instance.ActiveLevelNum() + 1, new Point()));
                position.X = 0 + hitbox.Width;
            }

            if (position.X < 0) {
                EventManager.Instance.Push(new Event("LEVEL_SHOW", (uint)LevelManager.Instance.ActiveLevelNum() - 1, new Point()));
                position.X = Sprite.graphics.PreferredBackBufferWidth - hitbox.Width;
            }

            if (position.Y >= Sprite.graphics.PreferredBackBufferHeight - hitbox.Height) jumpsLeft = 2;
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
        public void CheckForInput()
        {
            InputManager.Instance.Update();
            Event dev = EventManager.Instance.Pop("KEY_DOWN");
            Event uev = EventManager.Instance.Pop("KEY_UP");
            Event jev = EventManager.Instance.Pop("GAME_PAD_JOYSTICK");
            if (dev != null)
            {
                Keys down = (Keys)dev.Data;
                if (down == Keys.W) keyUp = true;
                else if (down == Keys.A) { keyLeft = true;  }
                else if (down == Keys.S) keyDown = true;
                else if (down == Keys.D) { keyRight = true;  }
            }
            if (uev != null)
            {
                Keys up = (Keys)uev.Data;
                if (keyUp && up == Keys.W) { keyUp = false; jumpsLeft -= 1; }
                else if (up == Keys.A) keyLeft = false; 
                else if (up == Keys.S) keyDown = false;
                else if (up == Keys.D) keyRight = false;
            }
            if (jev != null)
                Joystick = jev.MousePosition.ToVector2() / 5;
            //Joystick *= .5;
            sprite.effects = SpriteEffects.None;
            if (Joystick.X > 0)
                sprite.effects = SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// Respawns the player
        /// </summary>
        public void Respawn()
        {
            currentHp = maxHp;
            IsActive = true;
            Joystick = new Vector2();
            keyUp = false;
            keyDown = false;
            keyLeft = false;
            keyRight = false;
            resetPosition();
        }
    }
}
