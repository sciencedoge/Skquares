using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.FSM;

namespace UpgradePlatformer.Graphics
{
    class AnimationFSM
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
            animations = Copy.animations;
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

    class Animation
    {
        public List<Sprite> sprites;
        private int sprite, counter;
        private readonly int framesPerSprite;
        
        /// <summary>
        /// creates an animation
        /// </summary>
        /// <param name="Sprites">the sprites in the animation</param>
        /// <param name="FramesPerSprite">the ammount of frames to spend on the sprite</param>
        public Animation(List<Sprite> Sprites, int FramesPerSprite) {
            sprites = Sprites;
            framesPerSprite = FramesPerSprite;
        }

        /// <summary>
        /// updates the animation frame
        /// </summary>
        /// <param name="gameTime">a GameTime object</param>
        public void Update(GameTime gameTime) {
            if ((counter ++ % framesPerSprite) == 0) sprite ++;
            sprite %= (sprites.Count);
        }

        /// <summary>
        /// draws the current frame of the animation
        /// </summary>
        /// <param name="spriteBatch">a SpriteBatch object</param>
        /// <param name="position">the position of the sprite</param>
        /// <param name="rotation">the rotation of the sprite</param>
        /// <param name="size">the size of the sprite</param>
        public void Draw(SpriteBatch spriteBatch, Point position, float rotation, Vector2 size) {
            sprites[sprite].Draw(spriteBatch, position, rotation, size);
        }
    }
}
