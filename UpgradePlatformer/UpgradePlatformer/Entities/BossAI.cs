using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.Weapon;
using UpgradePlatformer.Music;
namespace UpgradePlatformer.Entities
{
  //HEADER===========================================
  //Names: Sami Chamberlain, Preston Precourt
  //Date: 6/3/2021
  //PUrpose: Creates the AI for the boss Entity
  //=================================================
  class BossAI
  {

    //Fields
    private Boss boss;
    private Player player;

    private Random random;
    private int phase;

    private int fireballChance;

    private List<Fireball> fireballs;

    private double secondsSinceJump;
    private bool initiated;

    public int Count => fireballs.Count;
    public bool Landed;

    /// <summary>
    /// Creates the BossAI object
    /// </summary>
    public BossAI(Player player, Boss boss)
    {
      this.boss = boss;
      this.player = player;

      random = new Random();
      fireballs = new List<Fireball>();

      fireballChance = 200;
      phase = 0;

      secondsSinceJump = 0;

      initiated = false;
    }

    /// <summary>
    /// Determines the phases and random chance of summoning fireballs
    /// </summary>
    public void Update(GameTime gt)
    {
      boss = EntityManager.Instance.Boss();
      if (boss == null) return;

      player = EntityManager.Instance.Player();

      if (player == null) return;

      Vector2 distance = FindDistance();

      if (boss.MaxHP != boss.CurrentHP) { initiated = true; }

      if (distance.X > 300 && initiated == false) return;
      else { initiated = true; }



      Intersects();

      if (boss.Hitbox.Y + 350 > player.Hitbox.Y)
      {
        boss.ApplyGravity(gt);
      }
      else
      {
        boss.X *= 20;
        boss.X += player.X - player.Hitbox.Width;
        boss.X /= 21;
        secondsSinceJump += gt.ElapsedGameTime.TotalMilliseconds;
      }

      if (random.Next(1, 101) == 100)
      {
        JumpAttack(gt);
      }

      //alters the y position slightly to allow for the boss to fall
      if (secondsSinceJump > 2000)
      {
        while (secondsSinceJump > 0)
        {
          boss.Y++;
          secondsSinceJump -= 100;
        }
        secondsSinceJump = 0;
      }


      if (!boss.IsActive && fireballs.Count > 0)
      {
        fireballs.Clear();
      }
      if (boss.IsActive)
      {
        if (boss.CurrentHP <= 50
        && phase == 0)
        {
          phase = 1;
          fireballChance = 50;
        }
        else if (boss.CurrentHP <= 10
            && phase == 1)
        {
          phase = 2;
          fireballChance = 10;
        }

        ShootFireball();

        for (int i = fireballs.Count - 1; i >= 0; i--)
        {
          if (fireballs[i].isActive)
          {
            fireballs[i].Update(gt);
          }
          else
          {
            fireballs.Remove(fireballs[i]);
          }
        }


      }
    }

    /// <summary>
    /// Draws fireballs to the screen
    /// </summary>
    /// <param name="sb"></param>
    public void Draw(SpriteBatch sb)
    {
      foreach (Fireball b in fireballs)
      {
        if (b.isActive)
        {
          b.Draw(sb);
        }
      }
    }

    //Methods
    /// <summary>
    /// Allows the boss to shoot a fireball
    /// </summary>
    public void ShootFireball()
    {
      if (random.Next(0, fireballChance) == fireballChance - 1)
      {
        SoundManager.Instance.PlaySFX("shoot");

        Vector2 bulletPos = new Vector2(boss.Position.X + (boss.Hitbox.Width / 2),
                                        boss.Position.Y + (boss.Hitbox.Height / 2));

        Vector2 distance = FindDistance();

        float rot = MathF.Atan(distance.Y / distance.X);
        if (distance.X < 0)
        {
          rot += MathF.PI;
        }

        fireballs.Add(new Fireball(distance, bulletPos, rot));
      }
    }

    /// <summary>
    /// Finds the distance between the boss and the player
    /// </summary>
    /// <returns></returns>
    public Vector2 FindDistance()
    {
      float distX = boss.hitbox.Center.X - player.hitbox.Center.X;
      float distY = boss.hitbox.Center.Y - player.hitbox.Center.Y;

      return new Vector2(distX, distY);
    }

    /// <summary>
    /// Performs the boss' jump attack on the player
    /// </summary>
    public void JumpAttack(GameTime gt)
    {
      //check for ground collision
      while (boss.Velocity.Y > -16f && boss.JumpsLeft > 0)
      {
        if (boss.JumpsLeft > 0 && boss.Velocity.Y >= -16f)
        {
          boss.Velocity = new Vector2(boss.Velocity.X, boss.Velocity.Y + boss.JumpVelocity.Y);
        }

        boss.JumpsLeft--;
        Landed = false;
      }

    }

    /// <summary>
    /// checks for boss intersections
    /// </summary>
    public void Intersects()
    {
      if (boss.Hitbox.Intersects(player.Hitbox))
      {
        player.TakeDamage(2);
      }
    }
  }
}
