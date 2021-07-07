using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UpgradePlatformer.Graphics
{
  struct ParticleProps
  {
    public Vector2 Position;
    public Vector2 Velocity, VelocityVariation;
    public Color StartColor, EndColor;
    public float SizeStart, SizeEnd;
    public float LifeTime;
  }
  class ParticleSystem
  {
    private struct Particle
    {
      public Vector2 Position;
      public Vector2 Velocity;
      public Color StartColor, EndColor;
      public float Rotation;
      public float SizeStart, SizeEnd;

      public float LifeTime;
      public float LifeRemaining;

      public bool IsActive;
    }
    private List<Particle> Pool;
    private Random r;
    public ParticleSystem()
    {
      Pool = new List<Particle>();
      r = new Random();
    }

    public void Update(GameTime gt)
    {
      List<Particle> l = new List<Particle>();
      for (int i = 0; i < Pool.Count; i++)
      {
        Particle p = Pool[i];
        if (!p.IsActive)
          continue;
        if (p.LifeRemaining <= 0.0f)
        {
          p.IsActive = false;
          continue;
        }

        p.LifeRemaining -= (float)gt.ElapsedGameTime.TotalSeconds;
        p.Position += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.Rotation += 0.01f * (float)gt.ElapsedGameTime.TotalSeconds;
        l.Add(p);
      }
      Pool = l;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      Texture2D _texture;

      _texture = new Texture2D(Sprite.graphics.GraphicsDevice, 1, 1);
      _texture.SetData(new Color[] { Color.White });

      for (int i = 0; i < Pool.Count; i++)
      {
        Particle p = Pool[i];
        if (!p.IsActive)
          continue;

        float lc = p.LifeRemaining / p.LifeTime;
        Color c = new Color((p.StartColor.ToVector3() * lc) + (p.EndColor.ToVector3() * (1f - lc)));
        float s = p.SizeStart * lc + p.SizeEnd * (1 - lc);
        Rectangle hitbox = new Rectangle();
        hitbox.Location = (p.Position * new Vector2(Sprite.GetScale())).ToPoint();
        hitbox.Location += Sprite.GetOrigin();
        hitbox.Y += (int)(40 * Sprite.GetScale());
        hitbox.Size = new Vector2(s * Sprite.GetScale()).ToPoint();

        spriteBatch.Draw(_texture, hitbox, new Rectangle(0, 0, 1, 1), c, p.Rotation, new Vector2(0.5f), SpriteEffects.None, 0);
      }
    }

    public void Emit(ParticleProps props)
    {
      Particle p = new Particle();
      p.IsActive = true;
      p.Position = props.Position;
      p.Rotation = (float)r.NextDouble() * 2.0f * MathF.PI;

      p.Velocity = props.Velocity;
      p.Velocity.X += props.VelocityVariation.X * ((float)r.NextDouble() - 0.5f);
      p.Velocity.Y += props.VelocityVariation.Y * ((float)r.NextDouble() - 0.5f);

      p.StartColor = props.StartColor;
      p.EndColor = props.EndColor;

      p.LifeTime = props.LifeTime;

      p.LifeRemaining = props.LifeTime;
      p.SizeStart = props.SizeStart;
      p.SizeEnd = props.SizeEnd;

      Pool.RemoveAll((p) => !p.IsActive);
      Pool.Add(p);
    }
  }
}
