using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.FSM;

namespace UpgradePlatformer.Graphics
{
  public class AnimationFSM
  {
    public FiniteStateMachine finiteStateMachine;
    public List<Animation> animations;

    /// <summary>
    /// creates a fsm based animation
    /// </summary>
    /// <param name="fsm">the fsm for the animation</param>
    /// <param name="Animations">the animations per state machine state</param>
    public AnimationFSM(FiniteStateMachine fsm, List<Animation> Animations)
    {
      finiteStateMachine = fsm;
      animations = Animations;
    }

    /// <summary>
    /// copys a fsm animation
    /// </summary>
    /// <param name="Copy">the fsm animation to copy</param>
    public AnimationFSM(AnimationFSM Copy)
    {
      finiteStateMachine = Copy.finiteStateMachine.Copy();
      animations = new List<Animation>();
      foreach (Animation a in Copy.animations)
        animations.Add(new Animation(a));
    }

    /// <summary>
    /// updates the animations in the animation list
    /// </summary>
    /// <param name="gameTime">a GameTime object</param>
    public void Update(GameTime gameTime)
    {
      foreach (Animation a in animations)
      {
        a.Update(gameTime);
      }
    }

    /// <summary>
    /// sets off a flag on the fsm
    /// </summary>
    /// <param name="id">the id of the flag to set off</param>
    public void SetFlag(int id)
    {
      finiteStateMachine.SetFlag(id);
    }

    /// <summary>
    /// draws the current frame of the current animation
    /// </summary>
    /// <param name="spriteBatch">a SpriteBatch object</param>
    /// <param name="position">the position of the sprite</param>
    /// <param name="rotation">the rotation of the sprite</param>
    /// <param name="size">the size of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Point position, float rotation, Vector2 size)
    {
      animations[finiteStateMachine.currentState].Draw(spriteBatch, position, rotation, size);
    }
  }
}
