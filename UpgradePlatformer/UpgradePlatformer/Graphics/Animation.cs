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
        public AnimationFSM(FiniteStateMachine fsm, List<Animation> Animations)
        {
            finiteStateMachine = fsm;
            animations = Animations;
        }
        
        public AnimationFSM(AnimationFSM Copy)
        {
            finiteStateMachine = Copy.finiteStateMachine.Copy();
            animations = Copy.animations;
        }
        
        public void Update(GameTime gameTime)
        {
            foreach (Animation a in animations)
            {
                a.Update(gameTime);
            }
        }

        public void SetFlag(int id)
        {
            finiteStateMachine.SetFlag(id);
        }

        public void Draw(SpriteBatch spriteBatch, Point position, float rotation, Vector2 size)
        {
            animations[finiteStateMachine.currentState].Draw(spriteBatch, position, rotation, size);
        }
    }

    class Animation
    {
        public List<Sprite> sprites;
        private int sprite, counter;
        private int framesPerSprite;

        public Animation(List<Sprite> Sprites, int FramesPerSprite) {
            sprites = Sprites;
            framesPerSprite = FramesPerSprite;
        }
        public void Update(GameTime gameTime) {
            if ((counter ++ % framesPerSprite) == 0) sprite ++;
            sprite = sprite % (sprites.Count);
        }

        public void Draw(SpriteBatch spriteBatch, Point position, float rotation, Vector2 size) {
            sprites[sprite].Draw(spriteBatch, position, rotation, size);
        }
    }
}
