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
using System;

namespace UpgradePlatformer
{

    //HEADER=========================================================
    //Authors: Sami Chamberlain, Preston Precourt
    //Date: 5/19/2021
    //===============================================================
    public class Game1 : Game
    {
        
        private const string ASSET_NAME_SPRITESHEET = "SpriteSheet";

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private FiniteStateMachine _stateMachine;
        private UIGroup mainMenu, pauseMenu, deathMenu, topHud, options;
        private SaveManager Save;
        private RenderTarget2D _lightTarget, _mainTarget;


#if DEBUG
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

        int joycooldown = 20;
        protected override void Initialize()
        {
            base.Initialize();
            // setup resolution
            _graphics.PreferredBackBufferWidth = 630;
            _graphics.PreferredBackBufferHeight = 670;
            Window.AllowUserResizing = true;
            //_graphics.IsFullScreen = true;
            //_graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();
            
            Sprite.texture = _spriteSheetTexture;
            Sprite.graphics = _graphics;

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
            Flag menuPressPause = new Flag(5, 0);

            // create state machine states
            StateMachineState Menu = new StateMachineState(new List<Flag> { playButtonPressMenu, optionsButtonPressMenu });                                     // 0
            StateMachineState Game = new StateMachineState(new List<Flag> { escapeKeyPressGame, PlayerDeathGame });                                             // 1
            StateMachineState Escape = new StateMachineState(new List<Flag> { escapeKeyPressEscape, optionsButtonPressGame, backPressEscape, menuPressPause }); // 2
            StateMachineState Respawn = new StateMachineState(new List<Flag> { playButtonPressRespawn, menuPressPause });                                       // 3
            StateMachineState Options = new StateMachineState(new List<Flag> { backPressOptions });                                                             // 4
            StateMachineState OptionsGame = new StateMachineState(new List<Flag> { backPressOptionsGame });                                                     // 5

            // create state machine
            _stateMachine = new FiniteStateMachine(new List<StateMachineState>{Menu, Game, Escape, Respawn, Options, OptionsGame});

            // start menu music
            SoundManager.Instance.PlayMusic("menu");
            #endregion

            #region  UIELEMENTS
            // define a const with the default screen dims only for ui stuff dont change
            const int DEF_SIZE = 630;

            // create ui elements

            UpgradeStructure.InitStructure();
            UpgradeStructure.panel = new UIPanel(_font, new Rectangle(5, DEF_SIZE - 55, DEF_SIZE - 10, 90))
            {
                cond = new ShowCond(() => {
                    return _stateMachine.currentState == 1;
                })
            };
            //UpgradeStructure.panel.Show(0);
            new LevelManager();
            int ButtonWidth = 200;
            UIButton playButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 300, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("LOAD", 0, new Point(0, 0)));
                    EventManager.Instance.Push(new Event("WORLD_SHOW", Save.Data.lastWorld, new Point(0, 0)));
                    LevelManager.Instance.SetLevel(1);
                    SoundManager.Instance.PlaySFX("button");
                    SoundManager.Instance.PlayMusic("game");
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                    EntityManager.Instance.RespawnPlayer();
                })
            };
            playButton.Text.Text = "Play";
            playButton.Disabled = !Save.Data.valid;

            UIButton newButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 350, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    foreach (World w in LevelManager.Instance.Worlds)
                        foreach (Level l in w.Levels)
                            l.Collected = new List<LevelCollectedEntity>();
                    EntityManager.Instance.PlayerMoney = 0;
                    EventManager.Instance.Push(new Event("WORLD_SHOW", 1, new Point(0, 0)));
                    SoundManager.Instance.PlaySFX("button");
                    Save.Data.valid = true;
                    Save.Data.lastWorld = 1;
                    Save.Save();
                    SoundManager.Instance.PlayMusic("game");
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                    EntityManager.Instance.RespawnPlayer();
                })
            };
            newButton.Text.Text = "New Game";

            UIButton closeButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 500, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("QUIT", 0, new Point(0, 0)));
                })
            };
            closeButton.Text.Text = "Quit";

            UIButton menuButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 350, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("SAVE", 0, new Point(0, 0)));
                    LevelManager.Instance.SetWorld(0);
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 5, new Point(0, 0)));
                })
            };
            menuButton.Text.Text = "Menu";

            UIButton continueButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 300, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
                })
            };
            continueButton.Text.Text = "Continue";

            UIButton OptionsButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 400, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 3, new Point(0, 0)));
                })
            };
            OptionsButton.Text.Text = "Options";

            UIButton backButton = new UIButton(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 500, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    EventManager.Instance.Push(new Event("STATE_MACHINE", 4, new Point(0, 0)));
                })
            };
            backButton.Text.Text = "Back";

            UIToggle muteToggle = new UIToggle(_font, new Rectangle((DEF_SIZE - ButtonWidth) / 2, 300, ButtonWidth, 40))
            {
                onClick = new UIAction((i) =>
                {
                    SoundManager.Instance.Muted = i == 1;
                    EventManager.Instance.Push(new Event("SAVE", 0, new Point(0, 0)));
                })
            };
            muteToggle.Text.update = new UITextUpdate(() =>
            {
                return SoundManager.Instance.Muted ? "Muted: x" : "Muted: -";
            });
            muteToggle.toggled = SoundManager.Instance.Muted;

            // TODO: Add Fullscreen
            //UIToggle fullscreenToggle = new UIToggle(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 350, ButtonWidth, 40));
            //fullscreenToggle.onClick = new UIAction((i) =>
            //{
            //     = i == 1;
            //    Save.Data.fullscreen = SoundManager.Instance.Muted;
            //    Save.Save();
            //});
            //fullscreenToggle.Text.update = new UITextUpdate(() =>
            //{
            //    return SoundManager.Instance.Muted ? "Muted: x" : "Muted: -";
            //});
            //fullscreenToggle.toggled = SoundManager.Instance.Muted;

            UIText TitleText = new UIText(_font, new Rectangle(0, 100, DEF_SIZE, 0), 2, Color.Black)
            {
                Text = "platformergamething",
                Centered = true
            };

            UIText PauseText = new UIText(_font, new Rectangle(0, 100, DEF_SIZE, 0), 2, Color.White)
            {
                Text = "paused",
                Centered = true
            };

            UIText HpText = new UIText(_font, new Rectangle(0, 0, 0, 0), 2, Color.White)
            {
                Text = "[          ]",
                Centered = false,
                update = new UITextUpdate(() =>
                {
                    string result = "";
                    string end = "";
                    result += "[";
                    if (EntityManager.Instance.GetPlayerHp() == -1) return "";
                    int cap = 0;
                    if (EntityManager.Instance.MaxPlayerHP() < 10)
                    {
                        for (int i = 0; i < EntityManager.Instance.MaxPlayerHP(); i++)
                        {
                            if (i < EntityManager.Instance.GetPlayerHp()) result += "=";
                            else result += " ";
                            if (cap++ > 10) break;
                        }
#if DEBUG
                        end = "F: " + frameRate.ToString("F2") + "\nE: " + EntityManager.Instance.Count();
#endif
                        return $"{result}]X1 ${EntityManager.Instance.PlayerMoney}\n{end}";
                    }
                    for (float i = 0; i < EntityManager.Instance.MaxPlayerHP(); i += EntityManager.Instance.MaxPlayerHP() / 10)
                    {
                        if (i < EntityManager.Instance.GetPlayerHp()) result += "=";
                        else result += " ";
                        if (cap++ > 10) break;
                    }

#if DEBUG
                    end = "F: " + frameRate.ToString("F2") + "\nE: " + EntityManager.Instance.Count();
#endif
                    return $"{result}]X1 ${EntityManager.Instance.PlayerMoney}\n{end}";
                })
            };
#endregion

#region UILAYOUT
            // initialize uiGroups
            Rectangle bounds = new Rectangle(0, 0, DEF_SIZE, DEF_SIZE);

            mainMenu = new UIGroup(new List<UIElement>{ TitleText, playButton, newButton, closeButton, OptionsButton}, bounds);
            pauseMenu = new UIGroup(new List<UIElement> { PauseText, continueButton, menuButton, OptionsButton, closeButton}, bounds);
            deathMenu = new UIGroup(new List<UIElement> { playButton, menuButton, closeButton}, bounds);
            options = new UIGroup(new List<UIElement> { backButton, muteToggle }, bounds);
            topHud = new UIGroup(new List<UIElement> { HpText }, bounds);

            // add uiGroups to uiManager
            UIManager.Instance.Add(mainMenu);   
            UIManager.Instance.Add(pauseMenu);
            UIManager.Instance.Add(deathMenu);
            UIManager.Instance.Add(topHud);
            UIManager.Instance.Add(options);
            UIManager.Instance.Add(UpgradeStructure.panel);

            UIManager.SetupFocusLoop(new List<UIElement> { muteToggle, playButton, newButton, continueButton, menuButton, OptionsButton, closeButton, backButton });
#endregion

#region EVENTS
            // create event listeners
            EventAction Action_Button_Press = new EventAction((Event e) =>
            {
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
                if (EntityManager.Instance.Player() != null)
                    EntityManager.Instance.Player().weapon.Clean();
                EventManager.Instance.Push(new Event("SAVE", 0, new Point(0, 0)));
                return true;
            });
            EventManager.Instance.AddListener(Action_World_Show, "WORLD_SHOW");

            EventAction Action_Nav = new EventAction((Event e) =>
            {
                UIManager.Instance.Nav(e.Data == 0);
                return true;
            });
            EventManager.Instance.AddListener(Action_Nav, "NAV");

            EventAction Action_Joystick = new EventAction((Event e) =>
            {
                if (joycooldown > 0) return false;
                if (e.MousePosition.Y > .5)
                {
                    UIManager.Instance.Nav(true);
                    joycooldown = 10;
                }
                else if (e.MousePosition.Y < -.5)
                {
                    UIManager.Instance.Nav(false);
                    joycooldown = 10;
                }
                return false;
            });
            EventManager.Instance.AddListener(Action_Joystick, "GAME_PAD_JOYSTICK");

            EventAction Action_Select = new EventAction((Event e) =>
            {
                UIManager.Instance.Select(e.Data == 1);
                return true;
            });
            EventManager.Instance.AddListener(Action_Select, "SELECT");

            EventAction Action_Level_Show = new EventAction((Event e) =>
            {
                LevelManager.Instance.SetLevel((int)e.Data);
                EntityManager.Instance.Player().weapon.Clean();
                return true;
            });
            EventManager.Instance.AddListener(Action_Level_Show, "LEVEL_SHOW");

            EventAction Action_Quit_Game = new EventAction((Event e) =>
            {
                EventManager.Instance.Push(new Event("SAVE", 0, new Point(0, 0)));
                Exit();
                return true;
            });

            EventManager.Instance.AddListener(Action_Quit_Game, "QUIT");

            EventAction Action_Mouse_Move = new EventAction((Event e) =>
            {
                UIManager.Instance.MouseMove(e.MousePosition);
                return false;
            });

            EventManager.Instance.AddListener(Action_Mouse_Move, "MOUSE_MOVE");

            EventAction Action_Save = new EventAction((Event e) =>
            {
                if (_stateMachine.currentState == 0) return true;
                List<LevelCollectedEntity> lces = new List<LevelCollectedEntity>();
                foreach (World w in LevelManager.Instance.Worlds)
                    foreach (Level l in w.Levels)
                        foreach (LevelCollectedEntity obj in l.Collected)
                            lces.Add(obj);
                Save.Data.collectedEntities = lces;
                Save.Data.muted = SoundManager.Instance.Muted;
                Save.Data.lastWorld = (uint)LevelManager.Instance.ActiveWorldNum();
                Save.Data.money = EntityManager.Instance.PlayerMoney;
                Save.Save();
                return true;
            });
            EventManager.Instance.AddListener(Action_Save, "SAVE");

            EventAction Action_Load = new EventAction((Event e) =>
            {
                foreach (World w in LevelManager.Instance.Worlds)
                    foreach (Level l in w.Levels)
                        l.Collected = new List<LevelCollectedEntity>();
                if (Save.Data.collectedEntities != null)
                    foreach (LevelCollectedEntity obj in Save.Data.collectedEntities)
                        LevelManager.Instance.Worlds[obj.World].Levels[obj.Level].Collected.Add(obj);
                SoundManager.Instance.Muted = Save.Data.muted;
                EntityManager.Instance.PlayerMoney = Save.Data.money;
                return true;
            });
            EventManager.Instance.AddListener(Action_Load, "LOAD");
#endregion
            UIManager.Instance.focused = playButton;
            _lightTarget = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _mainTarget = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            EventManager.Instance.Push(new Event("LOAD", 0, new Point(0, 0)));
        }
        Texture2D _rectangle;
        protected override void LoadContent()
        {
            Save = new IsolatedStorageSaveManager("UpgradePlatformer", "options.dat");
            Save.Load();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _font = Content.Load<SpriteFont>("Fonts/Poland");
            Sprite.Shaders = new List<Effect>();
            Sprite.Shaders.Add(Content.Load<Effect>("Shaders/ShaderLava"));
            //Filled

            _rectangle = new Texture2D(GraphicsDevice, 1, 1);
            _rectangle.SetData(new Color[] { Color.Black });
        }

        protected override void Update(GameTime gameTime)
        {
            if (_graphics.PreferredBackBufferWidth != Window.ClientBounds.Width || _graphics.PreferredBackBufferHeight != Window.ClientBounds.Height)
            {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                _lightTarget = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _mainTarget = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _graphics.ApplyChanges();
            }
            joycooldown--;
            joycooldown = Math.Max(0, joycooldown);
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
#endif
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            frameCounter++;
#endif
            GraphicsDevice.SetRenderTarget(_lightTarget);
            GraphicsDevice.Clear(Color.White);
            if (Sprite.Dim)
            {
                GraphicsDevice.Clear(Color.Gray);
            }
            Sprite.Light = true;
            if (LevelManager.Instance.Light && false) {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, null);

                LevelManager.Instance.DrawLightMap(_spriteBatch);

                _spriteBatch.End();
            }
            Sprite.Light = false;
            GraphicsDevice.SetRenderTarget(_mainTarget);
            GraphicsDevice.Clear(Color.LightBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            LevelManager.Instance.Draw(_spriteBatch);
            if (_stateMachine.currentState != 0 && _stateMachine.currentState != 4) EntityManager.Instance.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            Sprite.Shaders[0].Parameters["MaskTexture"].SetValue(_lightTarget);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Sprite.Shaders[0], null);
            
            _spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.White);

            Rectangle Bounds = Sprite.GetRect();
            Bounds.Location += new Point(0, 40);

            _spriteBatch.Draw(_rectangle, new Rectangle(0, 0, Bounds.Left, _graphics.PreferredBackBufferHeight), Color.Black);
            _spriteBatch.Draw(_rectangle, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, Bounds.Top), Color.Black);
            _spriteBatch.Draw(_rectangle, new Rectangle(Bounds.Right, 0, _graphics.PreferredBackBufferWidth - Bounds.Right, _graphics.PreferredBackBufferHeight), Color.Black);
            _spriteBatch.Draw(_rectangle, new Rectangle(0, Bounds.Bottom, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight - Bounds.Bottom), Color.Black);

            _spriteBatch.End();
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            
            UIManager.Instance.Draw(gameTime, _spriteBatch);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
