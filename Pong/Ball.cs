using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace Pong;

public class Ball : IDrawable, IUpdateable, IGameComponent, ICollidable
{
    public Texture2D Texture => _texture;
    public bool Enabled => true;
    public int UpdateOrder => 0;
    public int DrawOrder => 0;
    public bool Visible => true;
    public RectangleF BorderBox => _borderBox;
    public Vector2 Position => _position;
    private bool _isGameEnded = false;
    private readonly Texture2D _texture;
    private readonly SpriteBatch _spriteBatch;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private float _ballSpeed = 100f;
    private float _ballSpeedX = 0f;
    private float _ballSpeedY = 0f;
    private Vector2 _position;
    private RectangleF _borderBox;

    public Ball(SpriteBatch spriteBatch, Texture2D texture, int screenWidth, int screenHeight)
    {
        _texture = texture;
        _spriteBatch = spriteBatch;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public event EventHandler<PointGrantedEventArgs> PointGranted;

    public void Draw(GameTime gameTime)
    {
        if (!_isGameEnded)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: _texture,
                position: _position,
                sourceRectangle: null,
                Color.White,
                rotation: 0,
                origin: new Vector2(_texture.Height / 2, _texture.Width / 2),
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: 0);
            _spriteBatch.End();
        }
    }

    public void Update(GameTime gameTime)
    {
        if (!_isGameEnded)
        {
            if (CheckPositionX())
            {
                ResetBall();
                return;
            }
            UpdatePositionX(gameTime);
            UpdatePositionY(gameTime);
            UpdateBorderBoxes();
        }
    }

    public void Initialize()
    {
        _position = new Vector2(_screenWidth / 2, _screenHeight / 2);
        _borderBox = new RectangleF(x: _position.X - (float)_texture.Width / 2,
            y: _position.Y - (float)_texture.Height / 2,
            width: _texture.Width,
            height: _texture.Height);
        _ballSpeedX = _ballSpeed * RandomGenerator.GetFromCollection(new[] { -1, 1 });
        _ballSpeedY = _ballSpeed * RandomGenerator.GetFromCollection(new[] { -1, 1 });
    }

    public void OnCollision(object sender, CollisionEventArgs eventArgs)
    {
        if (eventArgs.ObjectHashes.Any(x => x == GetHashCode()))
            CalculateSpeed();
    }

    public void OnGameEnded(object sender, GameEndedEventArgs eventArgs)
    {
        _isGameEnded = true;
        _ballSpeedX = 0;
        _ballSpeedY = 0;
    }

    public void OnGameRestarted(object sender, EventArgs eventArgs)
    {
        _isGameEnded = false;
        ResetBall();
    }

    private bool CheckPositionX()
    {
        if (_position.X <= 0)
        {
            GrantPoint(false); return true;
        }
        if (_position.X >= 800)
        {
            GrantPoint(true); return true;
        }
        return false;
    }

    private void UpdateBorderBoxes()
    {
        _borderBox.X = _position.X - ((float)_texture.Width / 2);
        _borderBox.Y = _position.Y - ((float)_texture.Height / 2);
    }

    private void CalculateSpeed()
    {
        if (_ballSpeedX >= 300f)
        {
            _ballSpeedX *= -1;
            return;
        }
        _ballSpeedX *= -1.2f;
    }

    private void UpdatePositionY(GameTime gameTime)
    {
        if (_position.Y + Texture.Height / 2 >= 480 || _position.Y - Texture.Height / 2 <= 0)
        {
            _ballSpeedY *= -1;
        }
        _position.Y += _ballSpeedY * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void UpdatePositionX(GameTime gameTime)
    {
        _position.X += _ballSpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void GrantPoint(bool playerOneGetPoint)
    {
        PointGranted?.Invoke(this, new PointGrantedEventArgs(playerOneGetPoint));
    }

    private void ResetBall()
    {
        _position.X = _screenWidth / 2;
        _position.Y = _screenHeight / 2;
        _ballSpeedX = _ballSpeed * RandomGenerator.GetFromCollection(new[] { -1, 1 });
        _ballSpeedY = _ballSpeed * RandomGenerator.GetFromCollection(new[] { -1, 1 });
        UpdateBorderBoxes();
    }
}
