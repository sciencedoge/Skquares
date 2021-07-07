using Microsoft.Xna.Framework;
using System;

namespace UpgradePlatformer.Input
{
  struct EventListener
  {
    public String LookingFor;
    public EventAction Action;
    /// <summary>
    /// creates an event listener
    /// </summary>
    /// <param name="action">the action to run</param>
    /// <param name="eventKind">the event to look for</param>
    public EventListener(EventAction action, String eventKind)
    {
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
