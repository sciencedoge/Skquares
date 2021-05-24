using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Input
{
    class EventManager
    {
        // the maximum events before theyre overridden
        public const byte MAX_EVENTS = 10;

        private List<InputEvent> Events;

        public EventManager()
        {
            Events = new List<InputEvent>();
        }


        /// <summary>
        /// adds an event to the bottom of the stack
        /// </summary>
        /// <param name="e"></param>
        public void Push(InputEvent e)
        {
            Events.Insert(0, e);
            if (Events.Count > MAX_EVENTS) Events.RemoveAt(Events.Count - 1);
        }

        /// <summary>
        /// Pops the last InputEvent added
        /// </summary>
        /// <param name="filter">filters for this kind of event</param>
        /// <returns>The InputEvent</returns>
        public InputEvent Pop(String filter = "ANY")
        {
            if (Events.Count == 0) return null;
            // Get first Event
            InputEvent e = Events[0];

            // remove event from Stack
            Events.Remove(e);

            // check if filter is not needed
            if (filter == "ANY") return e;

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

    /// <summary>
    /// stores input data
    /// Data for Input Event by Event Kind
    ///
    /// - MOUSE_DOWN
    ///     - Button Number
    /// - MOUSE_UP
    ///     - Button Number
    /// - KEY_DOWN
    ///     - Key Value in Some encoding
    /// - KEY_UP
    ///    - Key Value in Some encoding
    /// </summary>
    class InputEvent
    {
        public String Kind;
        public uint Data;
        public Point MousePosition;

        public InputEvent(String kind, uint data, Point mousePosition)
        {
            Kind = kind;
            Data = data;
            MousePosition = mousePosition;
        }
    }
}
