using System;
using System.Collections.Generic;
using System.Text;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Input;

namespace UpgradePlatformer.Weapon
{
    //HEADER===================================================
    //Names: Sami Chamberlain, Preston Precourt
    //Date: 5/30/2021
    //Purpose: Gives functionality for the player's weapon
    //=========================================================
    class Weapon
    {

        //Fields
        private AnimationFSM animation;
        private bool isActive;
        private float rotation;
        private Vector2 position;

        private List<Bullet> bullets;

        private SpriteEffects effect;

        private Rectangle spriteBounds;

        private Point MousePos; //using this for now (not very familiar with current input system)
        private bool Click;

        private ButtonState b;
        private ButtonState prevB;

        //Properties

        /// <summary>
        /// returns whether or not the weapon is active
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }

        }

        /// <summary>
        /// Gets or sets the position of the weapon
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Ctor

        /// <summary>
        /// Creates a new Weapon object
        /// </summary>
        public Weapon(Vector2 position)
        {
            this.position = position;

            spriteBounds = new Rectangle(17, 14, 14, 14);

            this.animation = AnimationManager.Instance.animations[2];
            this.isActive = false;

            this.effect = SpriteEffects.None;

            bullets = new List<Bullet>();
        }

        //Methods
        
        public Vector2 FindDistance()
        {
            float distX = position.X - MousePos.X;
            float distY = position.Y - MousePos.Y;

            return new Vector2(distX, distY);
        }

        /// <summary>
        /// finds the current rotation of the weapon
        /// </summary>
        /// <returns></returns>
        public float FindRotation(Vector2 dist)
        {           

            float distance = (float)Math.Atan(dist.Y / dist.X);

            if(dist.X <= 0)
            {
                distance += MathF.PI;
            }

            return (float)Math.Atan(dist.Y / dist.X);
        }

        /// <summary>
        /// Draws the weapon to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                for (int i = bullets.Count - 1; i > 0; i--)
                {
                    if (bullets[i].isActive)
                    {
                        bullets[i].Draw(sb);
                    }
                }
                animation.Draw(sb, position.ToPoint(), rotation, new Vector2(14));
            }
        }

        /// <summary>
        /// Updates the weapon
        /// </summary>
        public void Update()
        {
            CheckForInput();

            for (int i = bullets.Count - 1; i > 0; i--)
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Update();
                }
                else
                {
                    bullets.Remove(bullets[i]);
                }
            }

            Vector2 path = FindDistance();

            rotation = FindRotation(path);

            if (Click)
            {
                bullets.Add(new Bullet(path, Position + new Vector2(7)));      
            }
        }

        /// <summary>
        /// checks for input and reacts accordingly
        /// </summary>
        public void CheckForInput()
        {
            Event mev = EventManager.Instance.Pop("MOUSE_MOVE");
            if (mev != null)
            {
                MousePos = mev.MousePosition;
            }
            Event rjev = EventManager.Instance.Pop("RGAME_PAD_JOYSTICK");
            if (rjev != null)
            {
                if (Vector2.Distance(rjev.MousePosition.ToVector2(), new Vector2()) > 2)
                    MousePos = rjev.MousePosition + Position.ToPoint();
            }
            Event dev = EventManager.Instance.Pop("MOUSE_DOWN");

            if (dev != null && dev.Data == 0)
            {
                Click = true;
            }
            Event uev = EventManager.Instance.Pop("MOUSE_UP");
                b = ButtonState.Pressed;
                if (!SingleMousePress())
                {
                    bullets.Add(new Bullet(path, Position));
                }                    
            }
            else
            {
                b = ButtonState.Released;
            }

            prevB = b;
        }

        /// <summary>
        /// Returns whether or not a single mouse press needs to be done
        /// </summary>
        /// <returns>true - yes; false- no</returns>
        private bool SingleMousePress()
        {
            if (prevB == b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            if (uev != null && uev.Data == 0)
            {
                Click = false;
            }
        }
    }
}
