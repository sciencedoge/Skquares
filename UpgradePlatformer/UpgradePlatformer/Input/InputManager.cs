using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer
{
    class InputManager
    {
        public MouseState mouseState;
        public MouseState prevMouseState;
        public KeyboardState kbState;
        public KeyboardState prevKbState;
        private List<InputEvent> Events;

        public InputManager()
        {
            Events = new List<InputEvent>();
            mouseState = new MouseState();
            kbState = new KeyboardState();
        }

        /// <summary>
        /// Updates the InputEvent List
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (prevMouseState.LeftButton != mouseState.LeftButton)
            {
                Events.Add(new InputEvent(CheckChangeType(prevMouseState.LeftButton, InputEventKind.MOUSE_UP, InputEventKind.MOUSE_DOWN), 0, mouseState.Position));
            }
            if (prevMouseState.RightButton != mouseState.RightButton)
            {
                Events.Add(new InputEvent(CheckChangeType(prevMouseState.RightButton, InputEventKind.MOUSE_UP, InputEventKind.MOUSE_DOWN), 1, mouseState.Position));
            }
            if (prevMouseState.MiddleButton != mouseState.MiddleButton)
            {
                Events.Add(new InputEvent(CheckChangeType(prevMouseState.MiddleButton, InputEventKind.MOUSE_UP, InputEventKind.MOUSE_DOWN), 2, mouseState.Position));
            }
        }
        
        /// <summary>
        /// Helper function for Update decideds event kinds from changes
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        public InputEventKind CheckChangeType(ButtonState prev, InputEventKind up, InputEventKind down)
        {
            if (prev == ButtonState.Pressed)
            {
                return up;
            }
            return down;
        }

        /// <summary>
        /// adds an event to the bottom of the stack
        /// </summary>
        /// <param name="e"></param>
        public void Push(InputEvent e)
        {
            Events.Insert(0, e);
        }

        /// <summary>
        /// Pops the last InputEvent added
        /// </summary>
        /// <param name="filter">filters for this kind of event</param>
        /// <returns>The InputEvent</returns>
        public InputEvent Pop(InputEventKind filter = InputEventKind.ANY)
        {
            if (Events.Count == 0) return null;
            // Get first Event
            InputEvent e = Events[0];

            // remove event from Stack
            Events.Remove(e);

            // check if filter is not needed
            if (filter == InputEventKind.ANY) return e;

            // I was board so I made it recursive
            if (e.Kind != filter)
            {
                InputEvent old = e;
                e = Pop(filter);
                Events.Add(old);
            }

            return e;
        }
    }

    class InputEvent
    {
        public InputEventKind Kind;
        public uint Data;
        public Point MousePosition;

        public InputEvent(InputEventKind kind, uint data, Point mousePosition)
        {
            Kind = kind;
            Data = data;
            MousePosition = mousePosition;
        }
    }

    enum InputEventKind
    {
        ANY,
        MOUSE_DOWN,
        MOUSE_UP,
        KEY_DOWN,
        KEY_UP,
    }
}
