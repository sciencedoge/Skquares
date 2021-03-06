using Microsoft.Xna.Framework;
using UpgradePlatformer.Entities;

namespace UpgradePlatformer.Weapon
{
  //HEADER==========================================
  //Name: Sami Chamberlain, Preston Precourt
  //Date: 5/30/2021
  //Purpose: Creates bullets for the weapon
  //================================================
  class Bullet : Projectile
  {
    /// <summary>
    /// Creates a bullet object
    /// </summary>
    public Bullet(Vector2 path, Vector2 location, float rotation, int cap, int hits)
        : base(path, location, rotation, cap, hits) { }


    /// <summary>
    /// Checks for intersections
    /// </summary>
    public override void Intersects()
    {
      foreach (Enemy e in EntityManager.Instance.Enemies())
      {
        if (hitbox.Intersects(e.Hitbox))
        {
          if (e.IsActive)
          {
            e.TakeDamage(EntityManager.Instance.Player().Damage);

            isActive = false;
            if (e.CurrentHP <= 0)
            {
              e.IsActive = false;
            }
            return;
          }
        }
      }

      Boss b = EntityManager.Instance.Boss();
      if (b == null)
        return;
      if (hitbox.Intersects(b.Hitbox))
      {
        if (b.IsActive)
        {
          b.TakeDamage(EntityManager.Instance.Player().Damage);

          isActive = false;
          if (b.CurrentHP <= 0)
          {
            b.IsActive = false;
          }
          return;
        }
      }
    }
  }
}
