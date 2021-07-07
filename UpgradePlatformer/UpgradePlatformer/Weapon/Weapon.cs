using System;
using System.Collections.Generic;
using UpgradePlatformer.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Input;
using UpgradePlatformer.Entities;
using UpgradePlatformer.Music;

namespace UpgradePlatformer.Weapon
{
  //HEADER===================================================
  //Names: Sami Chamberlain, Preston Precourt
  //Date: 5/30/2021
  //Purpose: Gives functionality for the player's weapon
  //=========================================================
  public class Weapon
  {

    //Fields
    private AnimationFSM animation;
    private bool isActive;
    private float rotation;
    private Vector2 position;
    private List<Bullet> bullets;
    private WeaponStats Stats;
    private Rectangle spriteBounds;
    public Point MousePos; //using this for now (not very familiar with current input system)
    private bool Click;
    private double Knockback;
    private Vector2 path;

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

    public int Count => bullets.Count;

    //Ctor

    /// <summary>
    /// Creates a new Weapon object
    /// </summary>
    public Weapon(Vector2 position, WeaponStats stats)
    {
      Stats = stats;
      this.position = position;

      spriteBounds = new Rectangle(17, 14, 14, 14);

      this.animation = AnimationManager.Instance.animations[2];
      this.isActive = false;

      bullets = new List<Bullet>();
    }

    //Methods

    public Vector2 FindDistance()
    {
      float distX = position.X - MousePos.X;
      float distY = position.Y - (MousePos.Y - (40 * Sprite.GetScale()));

      return new Vector2(distX, distY);
    }

    /// <summary>
    /// finds the current rotation of the weapon
    /// </summary>
    /// <returns></returns>
    public float FindRotation(Vector2 dist)
    {

      float distance = MathF.Atan(dist.Y / dist.X);

      if (dist.X < 0)
      {
        distance += MathF.PI;
      }

      //System.Diagnostics.Debug.WriteLine(distance * (180/MathF.PI));
      return distance;
    }

    /// <summary>
    /// Draws the weapon to the screen
    /// </summary>
    /// <param name="sb"></param>
    public void Draw(SpriteBatch sb)
    {
      if (isActive)
      {
        animation.Draw(sb, position.ToPoint(), rotation, new Vector2(14));

        for (int i = bullets.Count - 1; i > 0; i--)
        {
          if (bullets[i].isActive)
          {
            bullets[i].Draw(sb);
          }
        }
      }
    }

    /// <summary>
    /// Updates the weapon
    /// </summary>
    public void Update(GameTime gt)
    {
      bullets.RemoveAll((s) => !s.isActive);
      for (int i = bullets.Count - 1; i > 0; i--)
        bullets[i].Update(gt);

      if (Knockback > 0)
      {
        EntityManager.Instance.Player().Velocity = path / Vector2.Distance(path, new Vector2(0, 0)) * Stats.knockBack;
        Knockback -= gt.ElapsedGameTime.TotalSeconds * 60;
        return;
      }
      path = FindDistance();

      CheckForInput();

      rotation = FindRotation(path);

      if (Click)
      {
        Knockback = Stats.knockBackTime;
        Bullet bullet = new Bullet(path * Stats.projSpeed, new Vector2(position.X - 7, position.Y - 7), rotation, Stats.cap, Stats.pierce);
        Click = false;
        bullets.Add(bullet);
        SoundManager.Instance.PlaySFX("shoot");
        animation.SetFlag(1);
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
        MousePos = mev.MousePosition - Sprite.GetOrigin();
      }
      Event rjev = EventManager.Instance.Pop("RGAME_PAD_JOYSTICK");
      if (rjev != null)
      {
        if (Vector2.Distance(rjev.MousePosition.ToVector2(), new Vector2()) > 2)
        {
          rjev.MousePosition.Y *= -1;
          MousePos = rjev.MousePosition * new Point(15) + EntityManager.Instance.Player().hitbox.Center + new Point(0, (int)(40f * Sprite.GetScale()));
        }
      }
      Event jev = EventManager.Instance.Pop("GAME_PAD_JOYSTICK");
      if (jev != null)
      {
        if (Vector2.Distance(jev.MousePosition.ToVector2(), new Vector2()) > 2)
        {
          jev.MousePosition.Y *= -1;
          MousePos = jev.MousePosition * new Point(15) + EntityManager.Instance.Player().hitbox.Center + new Point(0, (int)(40f * Sprite.GetScale()));
        }
      }
      Event dev = EventManager.Instance.Pop("MOUSE_DOWN");

      if (dev != null && dev.Data == 0)
      {
        Click = true;
        animation.SetFlag(1);
      }
    }

    /// <summary>
    /// kills all bullets
    /// </summary>
    public void Clean()
    {
      bullets = new List<Bullet>();
    }
  }
}
