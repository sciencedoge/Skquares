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
                    AllSprites[j, i] = new Sprite(new Rectangle(16 * i, 13 + 16 * j, 16, 16), new Vector2(0), Color.White);
            animations = new List<AnimationFSM>();
            AddPlayerFSM(AllSprites);
            AddEnemyFSM(AllSprites);
            AddDiamondFSM(AllSprites);
        }
        public void AddPlayerFSM(Sprite[,] AllSprites)
        {

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

        public void AddEnemyFSM(Sprite[,] AllSprites)
        {

            // ==========
            // enemyr fsm
            // ==========
            // flags
            Flag LeftSLock = new Flag(2, 1);
            Flag LeftSRight = new Flag(1, 2);
            Flag LeftLLose = new Flag(3, 0);
            Flag LeftLRight = new Flag(1, 3);
            Flag RightSLock = new Flag(2, 3);
            Flag RightSLeft = new Flag(0, 0);
            Flag RightLLose = new Flag(3, 2);
            Flag RightLLeft = new Flag(0, 1);

            // states
            StateMachineState LeftS = new StateMachineState(new List<Flag> { LeftSLock, LeftSRight });
            Animation AniLeftS = new Animation(new List<Sprite> { AllSprites[1, 0].Copy() }, 2);
            StateMachineState LeftL = new StateMachineState(new List<Flag> { LeftLLose, LeftLRight });
            Animation AniLeftL = new Animation(new List<Sprite> { AllSprites[0, 4].Copy() }, 2);
            StateMachineState RightS = new StateMachineState(new List<Flag> { RightSLeft, RightSLock });
            Animation AniRightS = new Animation(new List<Sprite> { AllSprites[1, 0].Copy() }, 2);
            AniRightS.sprites[0].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightL = new StateMachineState(new List<Flag> { RightLLeft, RightLLose });
            Animation AniRightL = new Animation(new List<Sprite> { AllSprites[0, 4].Copy() }, 2);
            AniRightL.sprites[0].effects = SpriteEffects.FlipHorizontally;

            // init
            FiniteStateMachine enemyFSM = new FiniteStateMachine(new List<StateMachineState> { LeftS, LeftL, RightS, RightL });
            animations.Add(new AnimationFSM(enemyFSM, new List<Animation> { AniLeftS, AniLeftL, AniRightS, AniRightL, }));
        }

        public void AddDiamondFSM(Sprite[,] AllSprites)
        {

            // ==========
            // enemyr fsm
            // ==========
            // flags
            Flag LookShoot = new Flag(0, 0);
            Flag ShootLook = new Flag(1, 1);

            // states
            StateMachineState Shoot = new StateMachineState(new List<Flag> { ShootLook });
            Animation AniShoot = new Animation(new List<Sprite> { AllSprites[3, 1].Copy() }, 2);
            StateMachineState Look = new StateMachineState(new List<Flag> { LookShoot });
            Animation AniLook = new Animation(new List<Sprite> { AllSprites[3, 1].Copy() }, 2);

            // init
            FiniteStateMachine diamondFSM = new FiniteStateMachine(new List<StateMachineState> { Shoot, Look });
            animations.Add(new AnimationFSM(diamondFSM, new List<Animation> { AniShoot, AniLook}));
        }
    }
}
