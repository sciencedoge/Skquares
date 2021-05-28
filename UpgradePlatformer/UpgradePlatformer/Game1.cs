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
        private UIGroup mainMenu, pauseMenu, deathMenu, topHud;
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
            
            Sprite.texture = _spriteSheetTexture;
            Sprite.graphics = _graphics;
            
            structure = new UpgradeStructure();
            new LevelManager();

            // setup sound manager
            SoundManager.Instance.content = Content;
            SoundManager.Instance.LoadContent();

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
            SoundManager.Instance.PlayMusic("menu");
            
            // create ui elements
            int ButtonWidth = 200;
            UIButton playButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            playButton.onClick = new UIAction(() =>
            {
                SoundManager.Instance.PlaySFX("button");
                SoundManager.Instance.PlayMusic("game");
                EventManager.Instance.Push(new Event("STATE_MACHINE", 0, new Point(0, 0)));
                EntityManager.Instance.RespawnPlayer();
            });
            playButton.Text.Text = "Play";

            UIButton closeButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 350, ButtonWidth, 40));
            closeButton.onClick = new UIAction(() =>
            {
                SoundManager.Instance.PlaySFX("button");
                EventManager.Instance.Push(new Event("QUIT", 0, new Point(0, 0)));
                EntityManager.Instance.RespawnPlayer();
            });
            closeButton.Text.Text = "Quit";
            
            UIButton continueButton = new UIButton(_spriteSheetTexture, _font, new Rectangle((_graphics.PreferredBackBufferWidth - ButtonWidth) / 2, 300, ButtonWidth, 40));
            continueButton.onClick = new UIAction(() =>
            {
                EventManager.Instance.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
            });
            continueButton.Text.Text = "Continue";

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

                int cap = 0;
                for (float i = 0; i < EntityManager.Instance.MaxPlayerHP(); i += EntityManager.Instance.MaxPlayerHP() / 10) {
                    if (i < EntityManager.Instance.GetPlayerHp()) result += "=";
                    else result += " ";
                    if (cap++ > 10) break;
                }

                return $"[{result}]X1 ${EntityManager.Instance.PlayerMoney}";
            });

            // initialize uiGroups
            Rectangle bounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            mainMenu = new UIGroup(new List<UIElement>{ TitleText, playButton, closeButton}, bounds);
            pauseMenu = new UIGroup(new List<UIElement> { PauseText, continueButton, closeButton }, bounds);
            deathMenu = new UIGroup(new List<UIElement> { playButton, closeButton }, bounds);
            topHud = new UIGroup(new List<UIElement> { HpText }, bounds);

            // add uiGroups to uiManager
            UIManager.Instance.Add(mainMenu);
            UIManager.Instance.Add(pauseMenu);
            UIManager.Instance.Add(deathMenu);
            UIManager.Instance.Add(topHud);

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
                if (e.Data == (uint)Keys.Escape) EventManager.Instance.Push(new Event("STATE_MACHINE", 1, new Point(0, 0)));
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

            EventAction Action_Level_Show = new EventAction((Event e) =>
            {
                LevelManager.Instance.SetLevel((int)e.Data);
                return true;
            });
            EventManager.Instance.AddListener(Action_Level_Show, "LEVEL_SHOW");

            EventAction Action_Quit_Game = new EventAction((Event e) =>
            {
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

#if DEBUG
            Stats = new UIText(_font, new Rectangle(0, 40, 0, 0), 1, Color.White);
            UIManager.Instance.Add(Stats);
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
            Sprite.Dim         = _stateMachine.currentState == 2
                              || _stateMachine.currentState == 3;

#if DEBUG
            if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
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

            if (_stateMachine.currentState != 0) {
                EntityManager.Instance.Draw(gameTime, _spriteBatch);
            }

            UIManager.Instance.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
