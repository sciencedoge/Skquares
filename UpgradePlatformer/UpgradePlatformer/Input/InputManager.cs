using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Input
{
    class InputManager {
        private static readonly Lazy<InputManager>
            lazy =
            new Lazy<InputManager>
                (() => new InputManager());
        public static InputManager Instance { get { return lazy.Value; } }
        public MouseState mouseState;
        public MouseState prevMouseState;
        public KeyboardState kbState;
        public KeyboardState prevKbState;
        public GamePadState padState;
        public GamePadState prevPadState;

        public InputManager()
        {
            mouseState = new MouseState();
            kbState = new KeyboardState();
            padState = new GamePadState();
        }

        /// <summary>
        /// Updates the InputEvent List
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update()
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevKbState = kbState;
            kbState = Keyboard.GetState();
            prevPadState = padState;
            padState = GamePad.GetState(0);

            EventManager.Instance.Push(new Event("GAME_PAD_JOYSTICK", 0, (padState.ThumbSticks.Left * 5).ToPoint()));

            if (prevPadState.Buttons.A != padState.Buttons.A)
            {
                EventManager.Instance.Push(new Event(CheckChangeType(prevPadState.Buttons.A, "KEY_UP", "KEY_DOWN"), (uint)Keys.W, mouseState.Position));
                if (padState.Buttons.A == ButtonState.Pressed)
                    EventManager.Instance.Push(new Event("SELECT", 0, mouseState.Position));
            }

            if (prevPadState.Buttons.B != padState.Buttons.B && padState.Buttons.B == ButtonState.Pressed)
                EventManager.Instance.Push(new Event("SELECT", 1, mouseState.Position));

            if (prevPadState.Buttons.Start != padState.Buttons.Start)
                EventManager.Instance.Push(new Event(CheckChangeType(prevPadState.Buttons.Start, "KEY_UP", "KEY_DOWN"), (uint)Keys.Escape, mouseState.Position));
            
            if (prevPadState.DPad.Up != padState.DPad.Up && padState.DPad.Up == ButtonState.Pressed)
                EventManager.Instance.Push(new Event("NAV", 0, mouseState.Position));

            if (prevPadState.DPad.Down != padState.DPad.Down && padState.DPad.Down == ButtonState.Pressed)
                EventManager.Instance.Push(new Event("NAV", 1, mouseState.Position));

            if (prevMouseState.Position != mouseState.Position)
                EventManager.Instance.Push(new Event("MOUSE_MOVE", 0, mouseState.Position));
            if (prevMouseState.LeftButton != mouseState.LeftButton)
                EventManager.Instance.Push(new Event(CheckChangeType(prevMouseState.LeftButton, "MOUSE_UP", "MOUSE_DOWN"), 0, mouseState.Position));
            if (prevMouseState.RightButton != mouseState.RightButton)
                EventManager.Instance.Push(new Event(CheckChangeType(prevMouseState.RightButton, "MOUSE_UP", "MOUSE_DOWN"), 1, mouseState.Position));
            if (prevMouseState.MiddleButton != mouseState.MiddleButton)
                EventManager.Instance.Push(new Event(CheckChangeType(prevMouseState.MiddleButton, "MOUSE_UP", "MOUSE_DOWN"), 2, mouseState.Position));

            List<Keys> newKeys = new List<Keys>(kbState.GetPressedKeys());
            foreach (Keys k in prevKbState.GetPressedKeys())
            {
                if (!newKeys.Contains(k))
                {
                    EventManager.Instance.Push(new Event("KEY_UP", (uint)k, mouseState.Position));
                }
                else newKeys.Remove(k);
            }
            foreach (Keys k in newKeys)
            {
                EventManager.Instance.Push(new Event("KEY_DOWN", (uint)k, mouseState.Position));
            }
        }

        /// <summary>
        /// Helper function for Update decideds event kinds from changes
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        public String CheckChangeType(ButtonState prev, String up, String down)
        {
            if (prev == ButtonState.Pressed)
            {
                return up;
            }
            return down;
        }
    }
}