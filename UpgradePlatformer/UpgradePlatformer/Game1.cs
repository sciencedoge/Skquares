using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.UI;
using UpgradePlatformer.Entities;
using UpgradePlatformer.FSM;
using UpgradePlatformer.Graphics;

namespace UpgradePlatformer
{

    //HEADER=========================================================
    //Authors: Sami Chamberlain, Preston Precourt
    //Date: 5/19/2021
    //===============================================================
    public class Game1 : Game
    {
        private const string ASSET_NAME_SPRITESHEET = "SpriteSheet";

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private UIManager _uiManager;
        private InputManager _inputManager;
        private EventManager _eventManager;
        private LevelManager _levelManager;
        private EntityManager _entityManager;
        private SpriteFont _font;
        private FiniteStateMachine _stateMachine;
        private Player player;
        private UIButton playButton, continueButton;
        private UIText TitleText, PauseText;
#if DEBUG
        private UIText Stats;
        double frameRate = 0.0;
        int frameCounter;
#endif

        private Texture2D _spriteSheetTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
             
        }

        protected override void Initialize()
        {
            base.Initialize();
            // setup resolution
            _graphics.PreferredBackBufferHeight = 660;
            _graphics.PreferredBackBufferWidth = 660;
            _graphics.ApplyChanges();

            // setup manager scripts 
            _uiManager = new UIManager();
            _inputManager = new InputManager();
            _eventManager = new EventManager();
            _levelManager = new LevelManager(_spriteSheetTexture, _graphics);
            _entityManager = new EntityManager(_spriteSheetTexture, _graphics, _levelManager);

            // create connections in state machine
            Flag playButtonPressMenu = new Flag(0, 1);
            Flag escapeKeyPressGame = new Flag(1, 2);
            Flag escapeKeyPressEscape = new Flag(1, 1);
            Flag PlayerDeathGame = new Flag(2, 3);
            Flag playButtonPressRespawn = new Flag(0, 1);

            // create state machine states
            StateMachineState Menu = new StateMachineState(new List<Flag> { playButtonPressMenu });                 // 0
            StateMachineState Game = new StateMachineState(new List<Flag> { escapeKeyPressGame, PlayerDeathGame }); // 1
            StateMachineState Escape = new StateMachineState(new List<Flag> { escapeKeyPressEscape });              // 2
            StateMachineState Respawn = new StateMachineState(new List<Flag> { playButtonPressRespawn });           // 3

            // create state machine
            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn});
            
            // create ui elements
            int ButtonWidth = 200;
            playButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            playButton.onClick = new UIAction(() =>
            {
                _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                _entityManager.RespawnPlayer();
            });
            playButton.Text.Text = "Play";
            _uiManager.Add(playButton);
            
            continueButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 350, ButtonWidth, 40));
            continueButton.onClick = new UIAction(() =>
            {
                _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
            });
            continueButton.Text.Text = "Continue";
            _uiManager.Add(continueButton);

            TitleText = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 2, Color.White);
            TitleText.Text = "platformergamething";
            TitleText.Centered = true;
            _uiManager.Add(TitleText);

            PauseText = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 2, Color.White);
            PauseText.Text = "paused";
            PauseText.Centered = true;
            _uiManager.Add(PauseText);

            // create event listeners
            EventAction Action_Button_Press = new EventAction((uint e) =>
            {
                _stateMachine.SetFlag((int)e);
                _eventManager.Push(new Event("LEVEL_SHOW", 1, new Point(0, 0)));
                return true;
            });
            _eventManager.AddListener(Action_Button_Press, "STATE_MACHINE");

            EventAction Action_State_Machine = new EventAction((uint e) =>
            {
                if (e == (uint)Keys.Escape) _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
                else if (e == (uint)Keys.Enter) _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
#if DEBUG
                else if (_stateMachine.currentState == 0) return false;
                else if (e == (uint)Keys.Q) _levelManager.Prev();
                else if (e == (uint)Keys.E) _levelManager.Next();
#endif
                else return false;
                return true;
            });
            _eventManager.AddListener(Action_State_Machine, "KEY_DOWN");

            EventAction Action_Level_Show = new EventAction((uint e) =>
            {
                _levelManager.SetLevel((int)e);
                return true;
            });
            _eventManager.AddListener(Action_Level_Show, "LEVEL_SHOW");

            // EventAction Action_Level_Next = new EventAction((uint e) =>
            // {
            //     _levelManager.Next();
            //     return true;
            // });
            // _eventManager.AddListener(Action_Level_Next, "LEVEL_NEXT");
#if DEBUG
            // UIButton b = new UIButton(_spriteSheetTexture, _font, new Rectangle(250, 10, 40, 40));
            // b.Text.Text = "<";
            // UIButton c = new UIButton(_spriteSheetTexture, _font, new Rectangle(300, 10, 40, 40));
            // c.Text.Text = ">";
            // b.onClick = new UIAction(() => _levelManager.Prev());
            // c.onClick = new UIAction(() => _levelManager.Next());
            // _uiManager.Add(b);
            // _uiManager.Add(c);
            Stats = new UIText(_font, new Rectangle(0, 0, 0, 0), 1, Color.White);
            _uiManager.Add(Stats);
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _font = Content.Load<SpriteFont>("Fonts/Poland");
        }

        protected override void Update(GameTime gameTime)
        {
            // update managers
            _inputManager.Update(_eventManager);
            if (_stateMachine.currentState == 1) _entityManager.Update(gameTime, _eventManager, _inputManager);
            _uiManager.Update(gameTime, _eventManager);
            _levelManager.Update();

            // StateMachine related Updates
            playButton.IsActive = (_stateMachine.currentState == 0) || (_stateMachine.currentState == 3);
            TitleText.IsActive = (_stateMachine.currentState == 0);
            Sprite.Dim = (_stateMachine.currentState == 2) || (_stateMachine.currentState == 3);
            continueButton.IsActive = (_stateMachine.currentState == 2);
            PauseText.IsActive = (_stateMachine.currentState == 2);
#if DEBUG
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
            frameCounter = 0;
            Stats.Text = frameRate.ToString("F2") + "\nLVL:" + _levelManager.ActiveLevelNum() + "\nHP:" + _entityManager.GetPlayerHp()
            + "\nMoney:" + _entityManager.PlayerMoney;
#endif

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            frameCounter++;
#endif
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _levelManager.Draw(_spriteBatch);

            if (_stateMachine.currentState != 0) {
                _entityManager.Draw(gameTime, _spriteBatch);
            }

            _uiManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
