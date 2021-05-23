using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UpgradePlatformer.Input;
using UpgradePlatformer.Levels;
using UpgradePlatformer.UI;
using UpgradePlatformer.Entities;

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
        private LevelManager _levelManager;
        private SpriteFont _font;

        private EntityManager enMan;
#if DEBUG
        private UIText FpsMeter;
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
            _levelManager = new LevelManager(_spriteSheetTexture);
            UIButton b = new UIButton(_spriteSheetTexture, new Rectangle(250, 10, 40, 40));
            b.onClick = new UIAction(() => _levelManager.Next());
            _uiManager.Add(b);
#if DEBUG
            FpsMeter = new UIText(_font, new Rectangle(0, 0, 0, 0));
            _uiManager.Add(FpsMeter);
#endif

            _graphics.PreferredBackBufferHeight = 700;
            _graphics.PreferredBackBufferWidth = 700;

            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _font = Content.Load<SpriteFont>("Fonts/Poland");

            enMan = new EntityManager();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _inputManager.Update(gameTime);
            _uiManager.Update(gameTime, _inputManager);
#if DEBUG
        if (gameTime.ElapsedGameTime.TotalSeconds > 0.0)
            {
                frameRate = (double)frameCounter / gameTime.ElapsedGameTime.TotalSeconds;
            }
            frameCounter = 0;
            FpsMeter.Text = frameRate.ToString("F2");
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

            _uiManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
