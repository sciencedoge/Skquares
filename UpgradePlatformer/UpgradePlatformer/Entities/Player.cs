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

        public Weapon.Weapon weapon;
        private static int MaxJumps => UpgradeManager.Instance.GetAmmnt(UpgradeType.EXTRA_JUMP) + 1;

        private bool landed;

        private int sameVelocityFrames;

        /// <summary>
        /// Gets or sets whether the player is landed
        /// </summary>
        public bool Landed
        {
            get { return landed; }
            set { landed = value; }
        }

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
            this.animation = new AnimationFSM(AnimationManager.Instance.animations[0]);
            this.jumpsLeft = jumpsLeft;

            landed = true;

            weapon = new Weapon.Weapon(new Vector2(this.X + hitbox.Width,
                this.Y - hitbox.Height));

            sameVelocityFrames = 0;
        }

        //Methods

        /// <summary>
        /// Checks if the player intersects with another living object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>0</returns>
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
        /// <param name="gt">updates the players position</param>
        public override void Update(GameTime gt)
        {
            if (IsActive)
            {
                CheckForInput();

                if (this.damage > 0 && weapon.IsActive == false)
                {
                    weapon.IsActive = true;
                }

                if (weapon.IsActive)
                {
                    weapon.Position = new Vector2(this.X +  hitbox.Width,
                this.Y -  hitbox.Height);

                    weapon.Update();
                }

                cooldown--;
                cooldown = Math.Max(cooldown, 0);


                if (keyDown || Joystick.Y < -.5)
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
                    if (Joystick.X < 0)
                        animation.SetFlag(0);
                    if (Joystick.X > 0)
                        animation.SetFlag(1);
                    if (Joystick.X != 0)
                        animation.SetFlag(2);
                    else
                        animation.SetFlag(3);

                    if (keyUp)
                    {
                        
                        this.landed = false;
                        if(jumpsLeft > 0)
                        {                            
                            SoundManager.Instance.PlaySFX("jump");
                        }                       
                        //check for ground collision
                        if (jumpsLeft > 0 && velocity.Y >= -4f)
                        {
                            velocity.Y += jumpVelocity.Y;
                            if(velocity.Y == jumpVelocity.Y)
                            {
                                sameVelocityFrames++;
                            }
                            else
                            {
                                sameVelocityFrames = 0;
                            }

                            if(sameVelocityFrames >= 10)
                            {
                                jumpsLeft = 0;
                                keyUp = false;
                            }
                        }
                        else if (!(velocity.Y >= -4f))
                        {
                            keyUp = false;
                            
                            jumpsLeft -= 1;
                        }
                    }
                }
                ApplyGravity();

                if(Velocity.Y > 1 && landed)
                {
                    landed = false;
                }
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
        /// Used for drawing the weapon to the screen
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gt"></param>
        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);

            if (weapon.IsActive)
            {
                weapon.Draw(sb);
            }
        }

        /// <summary>
        /// Applies gravity to the player
        /// </summary>
        public override void ApplyGravity()
        {
            base.ApplyGravity();

            if (position.X >= 630) {
                EventManager.Instance.Push(new Event("LEVEL_SHOW", (uint)LevelManager.Instance.ActiveLevelNum() + 1, new Point()));
                position.X = 0 + hitbox.Width;
            }

            if (position.X <= 0) {
                EventManager.Instance.Push(new Event("LEVEL_SHOW", (uint)LevelManager.Instance.ActiveLevelNum() - 1, new Point()));
                position.X = 630 - hitbox.Width;
            }
        }

        /// <summary>
        /// resizes the player based on velocity
        /// </summary>
        /// <returns>a size vector</returns>
        public Vector2 GetVelocitySize()
        {
            Vector2 result = new Vector2(1, 1);
            if (velocity.Y != 0) result.Y = 1 / (((Math.Abs(velocity.X)) + 1) / 4);
            if (result.Y > 1) result.Y = 1;
            if (velocity.X != 0) result.X = 1 / (((Math.Abs(velocity.Y)) + 1) / 4);
            if (result.X > 1) result.X = 1;
            return result;
        }

        /// <summary>
        /// resets max jumps
        /// </summary>
        public override void OnFloorCollide()
        {
            jumpsLeft = MaxJumps;
        }

        /// <summary>
        /// processes input events
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
            if (jev != null) {
                EventManager.Instance.Push(jev);
                Joystick = jev.MousePosition.ToVector2() / 5;
            }
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
            ResetPosition();
        }

        private static Rectangle TILE_EMPTY = new Rectangle(49, 30, 60, 60);
        public void DrawLightMap(SpriteBatch spriteBatch)
        {
            Sprite s = new Sprite(TILE_EMPTY, new Vector2(0, 0), Color.White);
            s.Draw(spriteBatch, hitbox.Location - hitbox.Size, 0, hitbox.Size.ToVector2() * 4);
        }
    }
}
