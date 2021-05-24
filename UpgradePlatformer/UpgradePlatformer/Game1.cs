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
            Flag playButtonPressRespawn = new Flag(1, 0);

            StateMachineState Menu = new StateMachineState(new List<Flag> {playButtonPressMenu});                  // 0
            StateMachineState Game = new StateMachineState(new List<Flag> {escapeKeyPressGame, PlayerDeathGame});  // 1
            StateMachineState Escape = new StateMachineState(new List<Flag> { escapeKeyPressEscape });             // 2
            StateMachineState Respawn = new StateMachineState(new List<Flag> { playButtonPressRespawn });          // 3

            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn});
            UIButton playButton = new UIButton(_spriteSheetTexture, new Rectangle(400, 10, 40, 40));
            playButton.onClick = new UIAction(() => {
                _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                _entityManager.RespawnPlayer();
            });
            UIButton menuButton = new UIButton(_spriteSheetTexture, new Rectangle(450, 10, 40, 40));
            menuButton.onClick = new UIAction(() => _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0))));

            _uiManager.Add(playButton);
            _uiManager.Add(menuButton);

            EventAction a = new EventAction((uint e) =>
            {
                _stateMachine.SetFlag((int)e);
                return true;
            });

            _eventManager.AddListener(a, "STATE_MACHINE");

#if DEBUG
            UIButton b = new UIButton(_spriteSheetTexture, new Rectangle(250, 10, 40, 40));
            UIButton c = new UIButton(_spriteSheetTexture, new Rectangle(300, 10, 40, 40));
            b.onClick = new UIAction(() => _levelManager.Prev());
            c.onClick = new UIAction(() => _levelManager.Next());
            _uiManager.Add(b);
            _uiManager.Add(c);
            Stats = new UIText(_font, new Rectangle(0, 0, 0, 0), Color.White);
            _uiManager.Add(Stats);
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _font = Content.Load<SpriteFont>("Fonts/Poland");

            //player = new Player(10, 2, 
            //    new Rectangle(new Point(10, 10), new Point(50, 50)), _spriteSheetTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _inputManager.Update(_eventManager);
            _entityManager.Update(gameTime, _eventManager, _inputManager);
            _uiManager.Update(gameTime, _eventManager);
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

            _levelManager.Draw(_spriteBatch);

            _entityManager.Draw(gameTime, _spriteBatch);

            _uiManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
