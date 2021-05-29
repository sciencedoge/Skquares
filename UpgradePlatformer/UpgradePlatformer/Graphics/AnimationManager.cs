using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UpgradePlatformer.FSM;

namespace UpgradePlatformer.Graphics
{
    class AnimationManager
    {
        /// <summary>
        /// Singleton Stuff
        /// </summary>
        private static readonly Lazy<AnimationManager>
            lazy =
            new Lazy<AnimationManager>
                (() => new AnimationManager());
        public static AnimationManager Instance { get { return lazy.Value; } }

        public List<AnimationFSM> animations;

        /// <summary>
        /// creates the animation Manager and all animations
        /// </summary>
        public AnimationManager()
        {
            Sprite[,] AllSprites = new Sprite[20, 20];
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    AllSprites[j, i] = new Sprite(new Rectangle(16 * i, 13 + 16 * j, 16, 16), new Vector2(8, 8), Color.White);
            animations = new List<AnimationFSM>();

            // ==========
            // player fsm
            // ==========
            // flags
            Flag LeftSLookRight = new Flag(1, 0);
            Flag LeftSMove = new Flag(2, 3);
            Flag LeftMLookRight = new Flag(1, 2);
            Flag LeftMStop = new Flag(3, 1);
            Flag RightSLookLeft = new Flag(0, 1);
            Flag RightSMove = new Flag(2, 2);
            Flag RightMLookLeft = new Flag(0, 3);
            Flag RightMStop = new Flag(3, 0);

            // states
            StateMachineState LeftS = new StateMachineState(new List<Flag> { LeftSLookRight, LeftSMove });
            Animation AniLeftS = new Animation(new List<Sprite> { AllSprites[0, 1].Copy() }, 2);
            StateMachineState LeftM = new StateMachineState(new List<Flag> { LeftMLookRight, LeftMStop });
            Animation AniLeftM = new Animation(new List<Sprite> { AllSprites[0, 1].Copy(), AllSprites[0, 3].Copy() }, 2);
            StateMachineState RightS = new StateMachineState(new List<Flag> { RightSLookLeft, RightSMove });
            Animation AniRightS = new Animation(new List<Sprite> { AllSprites[0, 1].Copy() }, 2);
            AniRightS.sprites[0].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightM = new StateMachineState(new List<Flag> { RightMLookLeft, RightMStop });
            Animation AniRightM = new Animation(new List<Sprite> { AllSprites[0, 1].Copy(), AllSprites[0, 3].Copy() }, 2);
            AniRightM.sprites[0].effects = SpriteEffects.FlipHorizontally;
            AniRightM.sprites[1].effects = SpriteEffects.FlipHorizontally;

            // init
            FiniteStateMachine playerFSM = new FiniteStateMachine(new List<StateMachineState> { RightS, LeftS, RightM, LeftM });

            animations.Add(new AnimationFSM(playerFSM, new List<Animation> { AniRightS, AniLeftS, AniRightM, AniLeftM }));
        }
    }
}
