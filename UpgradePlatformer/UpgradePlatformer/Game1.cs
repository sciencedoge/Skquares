﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.UI;
using UpgradePlatformer.Entities;
using UpgradePlatformer.FSM;
using UpgradePlatformer.Graphics;
using UpgradePlatformer.Upgrade_Stuff;

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
        private UIGroup mainMenu, pauseMenu, deathMenu, topHud;
        private UpgradeManager upgradeManager;
        private UpgradeStructure structure;

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
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.PreferredBackBufferWidth = 660;
            _graphics.ApplyChanges();

            upgradeManager = new UpgradeManager();
            structure = new UpgradeStructure(upgradeManager);

            // setup manager scripts 
            _uiManager = new UIManager();
            _inputManager = new InputManager();
            _eventManager = new EventManager();
            _levelManager = new LevelManager(_spriteSheetTexture, _graphics);
            _entityManager = new EntityManager(_spriteSheetTexture, _graphics, _levelManager, upgradeManager);

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

            // create state machinez
            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn});
            
            // create ui elements
            int ButtonWidth = 200;
            UIButton playButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            playButton.onClick = new UIAction(() =>
            {
                _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                _entityManager.RespawnPlayer();
            });
            playButton.Text.Text = "Play";

            UIButton closeButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 350, ButtonWidth, 40));
            closeButton.onClick = new UIAction(() =>
            {
                _eventManager.Push(new Event("QUIT", 0, new Point(0, 0)));
                _entityManager.RespawnPlayer();
            });
            closeButton.Text.Text = "Quit";
            
            UIButton continueButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            continueButton.onClick = new UIAction(() =>
            {
                _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
            });
            continueButton.Text.Text = "Continue";

            UIText TitleText = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 2, Color.White);
            TitleText.Text = "platformergamething";
            TitleText.Centered = true;

            UIText PauseText = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 2, Color.White);
            PauseText.Text = "paused";
            PauseText.Centered = true;

            UIText HpText = new UIText(_font, new Rectangle(0, 0, 0, 0), 2, Color.White);
            HpText.Text = "[          ]";
            HpText.Centered = false;
            HpText.update = new UITextUpdate(() => {
                string result = "";

                for (int i = 0; i < 10; i ++) {
                    if (i < _entityManager.GetPlayerHp()) result += "=";
                    else result += " ";
                }

                return $"[{result}]X1 ${_entityManager.PlayerMoney}";
            });

            // initialize uiGroups
            Rectangle bounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            mainMenu = new UIGroup(new List<UIElement>{ TitleText, playButton, closeButton}, bounds);
            pauseMenu = new UIGroup(new List<UIElement> { PauseText, playButton, closeButton }, bounds);
            deathMenu = new UIGroup(new List<UIElement> { playButton, closeButton }, bounds);
            topHud = new UIGroup(new List<UIElement> { HpText }, bounds);

            // add uiGroups to uiManager
            _uiManager.Add(mainMenu);
            _uiManager.Add(pauseMenu);
            _uiManager.Add(deathMenu);
            _uiManager.Add(topHud);

            // create event listeners
            EventAction Action_Button_Press = new EventAction((uint e) =>
            {
                if (e == 0)
                    _eventManager.Push(new Event("WORLD_SHOW", 1, new Point(0, 0)));
                _stateMachine.SetFlag((int)e);
                return true;
            });
            _eventManager.AddListener(Action_Button_Press, "STATE_MACHINE");

            EventAction Action_State_Machine = new EventAction((uint e) =>
            {
                if (e == (uint)Keys.Escape) _eventManager.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
                else if (e == (uint)Keys.Enter && (_stateMachine.currentState != 1)) _eventManager.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
#if DEBUG
                else if (_stateMachine.currentState == 0) return false;
                else if (e == (uint)Keys.Q) _levelManager.Prev();
                else if (e == (uint)Keys.E) _levelManager.Next();
#endif
                else return false;
                return true;
            });
            _eventManager.AddListener(Action_State_Machine, "KEY_DOWN");

            EventAction Action_World_Show = new EventAction((uint e) =>
            {
                _levelManager.SetWorld((int)e);
                return true;
            });
            _eventManager.AddListener(Action_World_Show, "WORLD_SHOW");

            EventAction Action_Level_Show = new EventAction((uint e) =>
            {
                _levelManager.SetLevel((int)e);
                return true;
            });
            _eventManager.AddListener(Action_Level_Show, "LEVEL_SHOW");

            EventAction Action_Quit_Game = new EventAction((uint e) =>
            {
                Exit();
                return true;
            });

            _eventManager.AddListener(Action_Quit_Game, "QUIT");

#if DEBUG
            Stats = new UIText(_font, new Rectangle(0, 40, 0, 0), 1, Color.White);
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
            if (_stateMachine.currentState == 1) 
                _entityManager.Update(gameTime, _eventManager, _inputManager, _levelManager);
            _uiManager.Update(gameTime, _eventManager);
            _levelManager.Update(_entityManager, _spriteSheetTexture, _graphics);

            // StateMachine related Updates
            mainMenu.IsActive  = _stateMachine.currentState == 0;
            topHud.IsActive    = _stateMachine.currentState == 1;
            pauseMenu.IsActive = _stateMachine.currentState == 2;
            deathMenu.IsActive = _stateMachine.currentState == 3;

#if DEBUG
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
            frameCounter = 0;
            Stats.Text = frameRate.ToString("F2") + "\nWRLD:" + _levelManager.ActiveWorldNum() + "\nLVL:" + _levelManager.ActiveLevelNum() + "\nHP:" + _entityManager.GetPlayerHp()
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
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
