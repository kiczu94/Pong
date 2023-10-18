using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private Ball _ball;
        private CollisionService _collisionService;
        private GraphicsDeviceManager _graphics;
        private Player _player1;
        private Player _player2;
        private SpriteBatch _spriteBatch;
        private Texture2D _ballTexture;
        private Texture2D _playerBarTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _collisionService = new CollisionService();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _player1 = new Player(_spriteBatch, _playerBarTexture, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _player2 = new Player(_spriteBatch, _playerBarTexture, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, isPlayer2: true);
            _ball = new Ball(_spriteBatch, _ballTexture, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _collisionService = new CollisionService();
            _player1.Initialize();
            _player2.Initialize();
            _ball.Initialize();
            _collisionService.AddObject(_player1);
            _collisionService.AddObject(_player2);
            _collisionService.AddObject(_ball);
            _collisionService.OnCollisionDetected += _ball.OnCollision;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _ballTexture = Content.Load<Texture2D>("pixil-frame-0(2)");
            _playerBarTexture = Content.Load<Texture2D>("pixil-frame-0");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
            _collisionService.CheckCollisions();
            _player1.Update(gameTime);
            _player2.Update(gameTime);
            _ball.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _player1.Draw(gameTime);
            _player2.Draw(gameTime);
            _ball.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}