using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.UI;
using UpgradePlatformer.Entities;
using UpgradePlatformer.FSM;

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
        private UIButton playButton;
        private UIText Title;
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
            _uiManager = new UIManager();
            _inputManager = new InputManager();
            _eventManager = new EventManager();

            _graphics.PreferredBackBufferHeight = 660;
            _graphics.PreferredBackBufferWidth = 660;
            _graphics.ApplyChanges();
            _levelManager = new LevelManager(_spriteSheetTexture, _graphics);
            _entityManager = new EntityManager(_spriteSheetTexture, _graphics, _levelManager);

            Flag playButtonPressMenu = new Flag(0, 1);
            Flag escapeKeyPressGame = new Flag(1, 2);
            Flag escapeKeyPressEscape = new Flag(1, 1);
            Flag PlayerDeathGame = new Flag(2, 3);
            Flag playButtonPressRespawn = new Flag(0, 1);

            StateMachineState Menu = new StateMachineState(new List<Flag> { playButtonPressMenu });                // 0
            StateMachineState Game = new StateMachineState(new List<Flag> { escapeKeyPressGame, PlayerDeathGame });// 1
            StateMachineState Escape = new StateMachineState(new List<Flag> { escapeKeyPressEscape });             // 2
            StateMachineState Respawn = new StateMachineState(new List<Flag> { playButtonPressRespawn });          // 3

            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn});
            int ButtonWidth = 200;
            playButton = new UIButton(_spriteSheetTexture, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 200, ButtonWidth, 40));
            playButton.onClick = new UIAction(() => {
                _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                _entityManager.RespawnPlayer();
            });
            _uiManager.Add(playButton);
            Title = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 1, Color.White);
            Title.Text = "platformergamethingthatllhaveupgrades";
            Title.Centered = true;
            _uiManager.Add(Title);

            EventAction a1 = new EventAction((uint e) =>
            {
                _stateMachine.SetFlag((int)e);
                return true;
            });

            _eventManager.AddListener(a1, "STATE_MACHINE");

            EventAction a2 = new EventAction((uint e) =>
            {
                if (e == (uint)Keys.Escape) {
                    _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
                    return true;
                }
                return false;
            });

            _eventManager.AddListener(a2, "KEY_DOWN");

#if DEBUG
            UIButton b = new UIButton(_spriteSheetTexture, new Rectangle(250, 10, 40, 40));
            UIButton c = new UIButton(_spriteSheetTexture, new Rectangle(300, 10, 40, 40));
            b.onClick = new UIAction(() => _levelManager.Prev());
            c.onClick = new UIAction(() => _levelManager.Next());
            _uiManager.Add(b);
            _uiManager.Add(c);
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
            _inputManager.Update(_eventManager);
            if (_stateMachine.currentState == 1)
                _entityManager.Update(gameTime, _eventManager, _inputManager);
            _uiManager.Update(gameTime, _eventManager);
            playButton.IsActive = (_stateMachine.currentState == 0) || (_stateMachine.currentState == 3);
            Title.IsActive = (_stateMachine.currentState == 0) || (_stateMachine.currentState == 3);

#if DEBUG
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
            frameCounter = 0;
            Stats.Text = frameRate.ToString("F2") + "\n" + _levelManager.ActiveLevelName() + ":" + _levelManager.ActiveLevelNum() + "\n" + _stateMachine.currentState;
#endif

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            frameCounter++;
#endif
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (_stateMachine.currentState != 0) {
                _levelManager.Draw(_spriteBatch);
                _entityManager.Draw(gameTime, _spriteBatch);
            }

            _uiManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
