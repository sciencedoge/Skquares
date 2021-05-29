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
using UpgradePlatformer.Upgrade_Stuff;
using Microsoft.Xna.Framework.Content;
using UpgradePlatformer.Music;
using UpgradePlatformer.Saves;

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
        private SpriteFont _font;
        private FiniteStateMachine _stateMachine;
        private UIGroup mainMenu, pauseMenu, deathMenu, topHud, options;
        private UpgradeStructure structure;
        private SaveManager Save;

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
            
            Sprite.texture = _spriteSheetTexture;
            Sprite.graphics = _graphics;
            
            structure = new UpgradeStructure();
            new LevelManager();

            // setup sound manager
            SoundManager.Instance.content = Content;
            SoundManager.Instance.LoadContent();

#region  STATEMACHINE
            // create connections in state machine
            Flag playButtonPressMenu = new Flag(0, 1);
            Flag escapeKeyPressGame = new Flag(1, 2);
            Flag escapeKeyPressEscape = new Flag(1, 1);
            Flag backPressEscape = new Flag(4, 1);
            Flag PlayerDeathGame = new Flag(2, 3);
            Flag playButtonPressRespawn = new Flag(0, 1);
            Flag optionsButtonPressGame = new Flag(3, 5);
            Flag optionsButtonPressMenu = new Flag(3, 4);
            Flag backPressOptions = new Flag(4, 0);
            Flag backPressOptionsGame = new Flag(4, 2);

            // create state machine states
            StateMachineState Menu = new StateMachineState(new List<Flag> { playButtonPressMenu, optionsButtonPressMenu });                     // 0
            StateMachineState Game = new StateMachineState(new List<Flag> { escapeKeyPressGame, PlayerDeathGame });                             // 1
            StateMachineState Escape = new StateMachineState(new List<Flag> { escapeKeyPressEscape, optionsButtonPressGame, backPressEscape }); // 2
            StateMachineState Respawn = new StateMachineState(new List<Flag> { playButtonPressRespawn });                                       // 3
            StateMachineState Options = new StateMachineState(new List<Flag> { backPressOptions });                                             // 4
            StateMachineState OptionsGame = new StateMachineState(new List<Flag> { backPressOptionsGame });                                     // 5

            // create state machine
            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn, Options, OptionsGame});

            // start menu music
            SoundManager.Instance.PlayMusic("menu");
#endregion

#region  UIELEMENTS
            // create ui elements
            int ButtonWidth = 200;
            UIButton playButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            playButton.onClick = new UIAction((i) =>
            {
                SoundManager.Instance.PlaySFX("button");
                SoundManager.Instance.PlayMusic("game");
                EventManager.Instance.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                EntityManager.Instance.RespawnPlayer();
            });
            playButton.Text.Text = "Play";

            UIButton closeButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 500, ButtonWidth, 40));
            closeButton.onClick = new UIAction((i) =>
            {
                EventManager.Instance.Push(new Event("QUIT", 0, new Point(0, 0)));
            });
            closeButton.Text.Text = "Quit";

            UIButton continueButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            continueButton.onClick = new UIAction((i) =>
            {
                EventManager.Instance.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
            });
            continueButton.Text.Text = "Continue";

            UIButton OptionsButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 350, ButtonWidth, 40));
            OptionsButton.onClick = new UIAction((i) =>
            {
                EventManager.Instance.Push(new Event("STATE_MACHINE", 3, new Point(0, 0)));
            });
            OptionsButton.Text.Text = "Options";

            UIButton backButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 500, ButtonWidth, 40));
            backButton.onClick = new UIAction((i) =>
            {
                EventManager.Instance.Push(new Event("STATE_MACHINE", 4, new Point(0, 0)));
            });
            backButton.Text.Text = "Back";

            UIToggle muteToggle = new UIToggle(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            muteToggle.onClick = new UIAction((i) => 
            {
                SoundManager.Instance.Muted = i == 1;
                Save.Data.muted = SoundManager.Instance.Muted;
                Save.Save();
            });
            muteToggle.Text.update = new UITextUpdate(() =>
            {
                return SoundManager.Instance.Muted ? "Muted: x" : "Muted: -";
            });
            muteToggle.toggled = SoundManager.Instance.Muted;

            UIText TitleText = new UIText(_font, new Rectangle(0, 100, _graphics.PreferredBackBufferWidth, 0), 2, Color.Black);
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
                if (EntityManager.Instance.GetPlayerHp() == -1) return "";
                int cap = 0;
                if (EntityManager.Instance.MaxPlayerHP() < 10) {
                    for (int i = 0; i < EntityManager.Instance.MaxPlayerHP(); i ++)
                    {
                        if (i < EntityManager.Instance.GetPlayerHp()) result += "=";
                        else result += " ";
                        if (cap++ > 10) break;
                    }
                    return $"[{result}]X1 ${EntityManager.Instance.PlayerMoney}";
                }
                for (float i = 0; i < EntityManager.Instance.MaxPlayerHP(); i += EntityManager.Instance.MaxPlayerHP() / 10) {
                    if (i < EntityManager.Instance.GetPlayerHp()) result += "=";
                    else result += " ";
                    if (cap++ > 10) break;
                }

                return $"[{result}]X1 ${EntityManager.Instance.PlayerMoney}";
            });
            #endregion

#region UILAYOUT
            // initialize uiGroups
            Rectangle bounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            mainMenu = new UIGroup(new List<UIElement>{ TitleText, playButton, closeButton, OptionsButton}, bounds);
            pauseMenu = new UIGroup(new List<UIElement> { PauseText, continueButton, closeButton, OptionsButton }, bounds);
            deathMenu = new UIGroup(new List<UIElement> { playButton, closeButton}, bounds);
            options = new UIGroup(new List<UIElement> { backButton, muteToggle }, bounds);
            topHud = new UIGroup(new List<UIElement> { HpText }, bounds);

            // add uiGroups to uiManager
            UIManager.Instance.Add(mainMenu);   
            UIManager.Instance.Add(pauseMenu);
            UIManager.Instance.Add(deathMenu);
            UIManager.Instance.Add(topHud);
            UIManager.Instance.Add(options);
            UIManager.SetupFocusLoop(new List<UIElement> { playButton, continueButton, OptionsButton, closeButton, backButton });
            #endregion

#region EVENTS
            // create event listeners
            EventAction Action_Button_Press = new EventAction((Event e) =>
            {
                if (e.Data == 0)
                    EventManager.Instance.Push(new Event("WORLD_SHOW", 1, new Point(0, 0)));
                _stateMachine.SetFlag((int)e.Data);
                return true;
            });
            EventManager.Instance.AddListener(Action_Button_Press, "STATE_MACHINE");

            EventAction Action_State_Machine = new EventAction((Event e) =>
            {
                if (e.Data == (uint)Keys.Escape)
                {
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
                    if (_stateMachine.currentState == 2) SoundManager.Instance.PlayMusic("pause");
                    else SoundManager.Instance.PlayMusic("continue");
                }
                else if (e.Data == (uint)Keys.Enter && (_stateMachine.currentState != 1)) EventManager.Instance.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
#if DEBUG
                else if (_stateMachine.currentState == 0) return false;
                else if (e.Data == (uint)Keys.Q) LevelManager.Instance.Prev();
                else if (e.Data == (uint)Keys.E) LevelManager.Instance.Next();
#endif
                else return false;
                return true;
            });
            EventManager.Instance.AddListener(Action_State_Machine, "KEY_DOWN");

            EventAction Action_World_Show = new EventAction((Event e) =>
            {
                LevelManager.Instance.SetWorld((int)e.Data);
                return true;
            });
            EventManager.Instance.AddListener(Action_World_Show, "WORLD_SHOW");

            EventAction Action_Nav = new EventAction((Event e) =>
            {
                UIManager.Instance.Nav(e.Data == 0);
                return true;
            });
            EventManager.Instance.AddListener(Action_Nav, "NAV");

            EventAction Action_Select = new EventAction((Event e) =>
            {
                UIManager.Instance.Select(e.Data == 1);
                return true;
            });
            EventManager.Instance.AddListener(Action_Select, "SELECT");

            EventAction Action_Level_Show = new EventAction((Event e) =>
            {
                LevelManager.Instance.SetLevel((int)e.Data);
                return true;
            });
            EventManager.Instance.AddListener(Action_Level_Show, "LEVEL_SHOW");

            EventAction Action_Quit_Game = new EventAction((Event e) =>
            {
                Save.Save();
                Exit();
                return true;
            });

            EventManager.Instance.AddListener(Action_Quit_Game, "QUIT");

            EventAction Action_Mouse_Move = new EventAction((Event e) =>
            {
                UIManager.Instance.MouseMove(e.MousePosition);
                return true;
            });

            EventManager.Instance.AddListener(Action_Mouse_Move, "MOUSE_MOVE");
#endregion
            UIManager.Instance.focused = playButton;
            
#if DEBUG
            Stats = new UIText(_font, new Rectangle(0, 40, 0, 0), 1, Color.Gray);
            UIManager.Instance.Add(Stats);
#endif
        }
        protected override void LoadContent()
        {
            Save = new IsolatedStorageSaveManager("UpgradePlatformer", "options.dat");
            Save.Load();
            SoundManager.Instance.Muted = Save.Data.muted;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _font = Content.Load<SpriteFont>("Fonts/Poland");
        }

        protected override void Update(GameTime gameTime)
        {
            // update managers
            InputManager.Instance.Update();
            if (_stateMachine.currentState == 1) 
                EntityManager.Instance.Update(gameTime);
            UIManager.Instance.Update(gameTime);
            LevelManager.Instance.Update();

            // StateMachine related Updates
            mainMenu.IsActive  = _stateMachine.currentState == 0;
            topHud.IsActive    = _stateMachine.currentState == 1;
            pauseMenu.IsActive = _stateMachine.currentState == 2;
            deathMenu.IsActive = _stateMachine.currentState == 3;
            options.IsActive   = _stateMachine.currentState == 4
                              || _stateMachine.currentState == 5;
            Sprite.Dim         = _stateMachine.currentState == 2
                              || _stateMachine.currentState == 3
                              || _stateMachine.currentState == 5;

#if DEBUG
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
            frameCounter = 0;
            frameCounter = 0;
            Stats.Text = frameRate.ToString("F2") + "\nWRLD:" + LevelManager.Instance.ActiveWorldNum() + "\nLVL:" + LevelManager.Instance.ActiveLevelNum() + "\nHP:" + EntityManager.Instance.GetPlayerHp()
            + "\nMoney:" + EntityManager.Instance.PlayerMoney;
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

            LevelManager.Instance.Draw(_spriteBatch);
            if (_stateMachine.currentState != 0) EntityManager.Instance.Draw(gameTime, _spriteBatch);
            UIManager.Instance.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
