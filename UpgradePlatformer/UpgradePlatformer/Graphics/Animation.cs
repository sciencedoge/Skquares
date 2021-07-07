using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UpgradePlatformer.Graphics
{
  public class Animation
  {
    static Random rand;
    public List<Sprite> sprites;
    private int sprite;
    private float counter;
    private readonly int framesPerSprite;

    /// <summary>
    /// creates an animation
    /// </summary>
    /// <param name="Sprites">the sprites in the animation</param>
    /// <param name="FramesPerSprite">the ammount of frames to spend on the sprite</param>
    public Animation(List<Sprite> Sprites, int FramesPerSprite)
    {
      sprites = Sprites;
      framesPerSprite = FramesPerSprite;
    }

    /// <summary>
    /// creates an animation
    /// </summary>
    /// <param name="Sprites">the sprites in the animation</param>
    /// <param name="FramesPerSprite">the ammount of frames to spend on the sprite</param>
    public Animation(Animation Copy)
    {
      if (rand == null) rand = new Random();
      sprites = Copy.sprites;
      framesPerSprite = Copy.framesPerSprite;
      counter += rand.Next(1, 1000);
    }

    /// <summary>
    /// updates the animation frame
    /// </summary>
    /// <param name="gameTime">a GameTime object</param>
    public void Update(GameTime gameTime)
    {
      counter += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
      while (counter > framesPerSprite) { sprite++; counter -= framesPerSprite; }
      sprite %= (sprites.Count);
    }

    /// <summary>
    /// draws the current frame of the animation
    /// </summary>
    /// <param name="spriteBatch">a SpriteBatch object</param>
    /// <param name="position">the position of the sprite</param>
    /// <param name="rotation">the rotation of the sprite</param>
    /// <param name="size">the size of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Point position, float rotation, Vector2 size)
    {
      sprites[sprite].Draw(spriteBatch, position, rotation, size);
    }
  }
}
