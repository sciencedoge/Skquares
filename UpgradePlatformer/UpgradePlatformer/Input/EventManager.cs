using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Input
{
    public delegate bool EventAction(Event e);

    class EventManager
    {
        private static readonly Lazy<EventManager>
            lazy =
            new Lazy<EventManager>
                (() => new EventManager());
        public static EventManager Instance { get { return lazy.Value; } }
        
        public const byte MAX_EVENTS = 20; // the maximum events before theyre overridden
        private List<Event> Events;
        private List<EventListener> Listeners;

        /// <summary>
        /// creates the event manager
        /// </summary>
        public EventManager()
        {
            Events = new List<Event>();
            Listeners = new List<EventListener>();
        }

        public void Cleanup(GameTime gameTime)
        {
            Events.Clear();
        }

        /// <summary>
        /// adds a listener to an event
        /// </summary>
        /// <param name="e">the action to run takes the event as an arg</param>
        /// <param name="eventKind">the event Kind to look for</param>
        public void AddListener(EventAction e, String eventKind) {
            Listeners.Add(new EventListener(e, eventKind));
        }
        
        /// <summary>
        /// adds an event to the top of the stack
        /// </summary>
        /// <param name="e">the event</param>
        public void Push(Event e)
        {
            foreach (EventListener el in Listeners)
                if (el.LookingFor == e.Kind)
                    if (el.Action(e))
                        return;
            Events.Insert(0, e);
            if (Events.Count > MAX_EVENTS) Events.RemoveAt(Events.Count - 1);
        }

        /// <summary>
        /// Pops the top of the stack that matches the filter
        /// </summary>
        /// <param name="filter">filters for this kind of event</param>
        /// <returns>The InputEvent</returns>
        public Event Pop(String filter = "ANY")
        {
            if (Events.Count == 0) return null;
            // Get first Event
            Event e = Events[0];

            // remove event from Stack
            Events.Remove(e);

            // check if filter is not needed
            if (filter == "ANY") return e;

            // I was board so I made it recursive
            if (e.Kind != filter)
            {
                Event old = e;
                e = Pop(filter);
                Events.Add(old);
            }

            return e;
        }
    }

    struct EventListener
    {
        public String LookingFor;
        public EventAction Action;
        /// <summary>
        /// creates an event listener
        /// </summary>
        /// <param name="action">the action to run</param>
        /// <param name="eventKind">the event to look for</param>
        public EventListener(EventAction action, String eventKind) {
            Action = action;
            LookingFor = eventKind;
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
    public class Event
    {
        public String Kind;
        public uint Data;
        public Point MousePosition;

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="kind">the kind of event</param>
        /// <param name="data">the data for the event</param>
        /// <param name="mousePosition">the position of the mouse sometimes</param>
        public Event(String kind, uint data, Point mousePosition)
        {
            Kind = kind;
            Data = data;
            MousePosition = mousePosition;
        }
    }
}
