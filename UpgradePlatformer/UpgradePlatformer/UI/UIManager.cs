using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Input;

namespace UpgradePlatformer.UI
{
  public delegate void UIAction(int i);
  class UIManager
  {
    /// <summary>
    /// Singleton stuff
    /// </summary>
    private static readonly Lazy<UIManager>
        lazy =
        new Lazy<UIManager>
            (() => new UIManager());
    public static UIManager Instance { get { return lazy.Value; } }

    public List<UIElement> UIElements;
    public UIElement focused;

    /// <summary>
    /// creates the ui manager
    /// </summary>
    public UIManager()
    {
      UIElements = new List<UIElement>();
    }

    /// <summary>
    /// Do some random stuff before draw
    /// </summary>
    /// <param name="gameTime">a gameTime Object</param>
    public void Update(GameTime gameTime)
    {
      Event ev = EventManager.Instance.Pop("MOUSE_DOWN");

      if (ev != null)
      {
        if (!ProcessClick(ev.MousePosition, ev.Data))
          EventManager.Instance.Push(ev);
      }

      foreach (UIElement e in UIElements)
        e.ResetActive();


      foreach (UIElement e in UIElements)
      {
        e.Update(gameTime);
      }
    }

    /// <summary>
    /// Draws ALL UIElements
    /// </summary>
    /// <param name="gameTime"></param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      foreach (UIElement e in UIElements)
      {
        e.Draw(gameTime, spriteBatch);
      }
    }

    /// <summary>
    /// processes a click anywhere on the screen
    /// </summary>
    /// <param name="position">the position the screen was clicked</param>
    /// <returns>true if the click is used</returns>
    public bool ProcessClick(Point position, uint button)
    {
      position -= Sprite.GetOrigin();
      position.X = (int)(position.X / Sprite.GetScale());
      position.Y = (int)(position.Y / Sprite.GetScale());
      if (button != 0) return false;
      foreach (UIElement e in UIElements)
      {
        if (e.Bounds.Contains(position) && e.IsActive)
        {
          return e.WhenClicked(position - e.Bounds.Location);
        }
      }
      return false;
    }
    public void MouseMove(Point position)
    {
      position -= Sprite.GetOrigin();
      position.X = (int)(position.X / Sprite.GetScale());
      position.Y = (int)(position.Y / Sprite.GetScale());
      foreach (UIElement e in UIElements)
      {
        if (e.Bounds.Contains(position) && e.IsActive)
        {
          e.WhenMoved(position - e.Bounds.Location);
        }
      }
    }

    /// <summary>
    /// adds a uielement to the list
    /// </summary>
    /// <param name="element">the element to add</param>
    public void Add(UIElement element)
    {
      UIElements.Add(element);
    }

    /// <summary>
    /// navigates the ui
    /// </summary>
    /// <param name="reverse">weter to go backwards</param>
    public void Nav(bool reverse)
    {
      UIElement start = focused;
      do
      {
        focused.Focused = false;
        if (reverse && focused.prevFocus != null)
        {
          focused.prevFocus.Focused = true;
          focused = focused.prevFocus;
        }
        else if (focused.nextFocus != null)
        {
          focused.nextFocus.Focused = true;
          focused = focused.nextFocus;
        }
      } while (focused.IsActive == false && focused != start);
    }

    /// <summary>
    /// finds the first active ui element
    /// </summary>
    /// <returns>the uielement or null if none active</returns>
    public UIElement GetActive()
    {
      foreach (UIElement e in UIElements)
      {
        UIElement n = e.GetActive();
        if (n != null) return n;
      }
      return null;
    }

    /// <summary>
    /// selects a uiElement
    /// </summary>
    /// <param name="reverse">wether to go backwards</param>
    public void Select(bool reverse)
    {
      if (reverse)
        EventManager.Instance.Push(new Event("STATE_MACHINE", 4, new Point()));
      else
        focused.WhenClicked(new Point(0));
    }

    /// <summary>
    /// setup the focus loop for the uielements
    /// </summary>
    /// <param name="elements">the elements in order</param>
    public static void SetupFocusLoop(List<UIElement> elements)
    {
      for (int i = 1; i < elements.Count; i++)
      {
        elements[i - 1].nextFocus = elements[i];
        elements[i].prevFocus = elements[i - 1];
      }
      elements[elements.Count - 1].nextFocus = elements[0];
      elements[0].prevFocus = elements[elements.Count - 1];
      return;
    }
  }
}
