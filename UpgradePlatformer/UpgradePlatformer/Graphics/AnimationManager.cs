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
            Flag LeftIMove = new Flag(2, 0);
            Flag LeftIDone = new Flag(5, 3);
            Flag LeftSIdle = new Flag(4, 5);
            Flag LeftILookRight = new Flag(1, 5);
            Flag RightSLookLeft = new Flag(0, 1);
            Flag RightSMove = new Flag(2, 2);
            Flag RightMLookLeft = new Flag(0, 3);
            Flag RightMStop = new Flag(3, 0);
            Flag RightIMove = new Flag(2, 0);
            Flag RightIDone = new Flag(5, 2);
            Flag RightSIdle = new Flag(4, 4);
            Flag RightILookLeft = new Flag(0, 4);

            // states
            StateMachineState LeftS = new StateMachineState(new List<Flag> { LeftSLookRight, LeftSMove, LeftSIdle });
            Animation AniLeftS = new Animation(new List<Sprite> { AllSprites[1, 0].Copy() }, 2);
            StateMachineState LeftM = new StateMachineState(new List<Flag> { LeftMLookRight, LeftMStop });
            Animation AniLeftM = new Animation(new List<Sprite> { AllSprites[1, 0].Copy(), AllSprites[2, 0].Copy() }, 2);
            StateMachineState LeftI = new StateMachineState(new List<Flag> { LeftIMove, LeftILookRight, LeftIDone });
            Animation AniLeftI = new Animation(new List<Sprite> { AllSprites[1, 0].Copy(), AllSprites[3, 0].Copy(), AllSprites[4, 0].Copy() }, 30);
            StateMachineState RightS = new StateMachineState(new List<Flag> { RightSLookLeft, RightSMove, RightSIdle });
            Animation AniRightS = new Animation(new List<Sprite> { AllSprites[1, 0].Copy() }, 2);
            AniRightS.sprites[0].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightM = new StateMachineState(new List<Flag> { RightMLookLeft, RightMStop });
            Animation AniRightM = new Animation(new List<Sprite> { AllSprites[1, 0].Copy(), AllSprites[2, 0].Copy() }, 2);
            AniRightM.sprites[0].effects = SpriteEffects.FlipHorizontally;
            AniRightM.sprites[1].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightI = new StateMachineState(new List<Flag> { RightIMove, RightILookLeft, RightIDone});
            Animation AniRightI = new Animation(new List<Sprite> { AllSprites[1, 0].Copy(), AllSprites[3, 0].Copy(), AllSprites[4, 0].Copy() }, 30);
            AniRightI.sprites[0].effects = SpriteEffects.FlipHorizontally;
            AniRightI.sprites[1].effects = SpriteEffects.FlipHorizontally;
            AniRightI.sprites[2].effects = SpriteEffects.FlipHorizontally;

            // init
            FiniteStateMachine playerFSM = new FiniteStateMachine(new List<StateMachineState> { RightS, LeftS, RightM, LeftM, RightI, LeftI });
            animations.Add(new AnimationFSM(playerFSM, new List<Animation> { AniRightS, AniLeftS, AniRightM, AniLeftM, AniRightI, AniLeftI }));
        }

        public void AddEnemyFSM(Sprite[,] AllSprites)
        {

            // ==========
            // enemyr fsm
            // ==========
            // flags
            Flag LeftSLock = new Flag(2, 3);
            Flag LeftSRight = new Flag(1, 0);
            Flag LeftLLose = new Flag(3, 1);
            Flag LeftLRight = new Flag(1, 2);
            Flag LeftIDone = new Flag(5, 1);
            Flag LeftSIdle = new Flag(4, 5);
            Flag RightSLock = new Flag(2, 2);
            Flag RightSLeft = new Flag(0, 1);
            Flag RightLLose = new Flag(3, 0);
            Flag RightLLeft = new Flag(0, 2);
            Flag RightIDone = new Flag(5, 0);
            Flag RightSIdle = new Flag(4, 4);

            // states
            StateMachineState LeftS = new StateMachineState(new List<Flag> { LeftSLock, LeftSRight, LeftSIdle });
            Animation AniLeftS = new Animation(new List<Sprite> { AllSprites[1, 1].Copy() }, 2);
            StateMachineState LeftL = new StateMachineState(new List<Flag> { LeftLLose, LeftLRight });
            Animation AniLeftL = new Animation(new List<Sprite> { AllSprites[2, 1].Copy() }, 2);
            StateMachineState LeftI = new StateMachineState(new List<Flag> { LeftIDone });
            Animation AniLeftI = new Animation(new List<Sprite> { AllSprites[1, 1].Copy(), AllSprites[3, 1].Copy(), AllSprites[4, 1].Copy() }, 60);
            StateMachineState RightS = new StateMachineState(new List<Flag> { RightSLeft, RightSLock, RightSIdle });
            Animation AniRightS = new Animation(new List<Sprite> { AllSprites[1, 1].Copy() }, 2);
            AniRightS.sprites[0].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightL = new StateMachineState(new List<Flag> { RightLLeft, RightLLose });
            Animation AniRightL = new Animation(new List<Sprite> { AllSprites[2, 1].Copy() }, 2);
            AniRightL.sprites[0].effects = SpriteEffects.FlipHorizontally;
            StateMachineState RightI = new StateMachineState(new List<Flag> { RightIDone });
            Animation AniRightI = new Animation(new List<Sprite> { AllSprites[1, 1].Copy(), AllSprites[3, 1].Copy(), AllSprites[4, 1].Copy() }, 60);
            AniRightI.sprites[0].effects = SpriteEffects.FlipHorizontally;
            AniRightI.sprites[1].effects = SpriteEffects.FlipHorizontally;
            AniRightI.sprites[2].effects = SpriteEffects.FlipHorizontally;

            // init
            FiniteStateMachine enemyFSM = new FiniteStateMachine(new List<StateMachineState> { RightS, LeftS, RightL, LeftL, RightI, LeftI });
            animations.Add(new AnimationFSM(enemyFSM, new List<Animation> { AniRightS, AniLeftS, AniRightL, AniLeftL, AniRightI, AniLeftI }));
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
            Animation AniShoot = new Animation(new List<Sprite> { AllSprites[5, 0].Copy() }, 2);
            StateMachineState Look = new StateMachineState(new List<Flag> { LookShoot });
            Animation AniLook = new Animation(new List<Sprite> { AllSprites[6, 0].Copy() }, 2);

            // init
            FiniteStateMachine diamondFSM = new FiniteStateMachine(new List<StateMachineState> { Shoot, Look });
            animations.Add(new AnimationFSM(diamondFSM, new List<Animation> { AniShoot, AniLook}));
        }
    }
}
