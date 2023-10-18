using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Pong;

public class Player : IDrawable, IUpdateable, IGameComponent, ICollidable
{
    public Texture2D Texture { get { return _texture; } }
    private readonly Texture2D _texture;
    private readonly SpriteBatch _spriteBatch;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private readonly float _playerSpeed = 200f;
    private readonly bool _isPlayer2;
    private Vector2 _position;
    private RectangleF _borderBox;

    public Player(SpriteBatch spriteBatch, Texture2D texture, int screenWidth, int screenHeight, bool isPlayer2 = false)
    {
        _isPlayer2 = isPlayer2;
        _texture = texture;
        _spriteBatch = spriteBatch;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public int DrawOrder => 0;

    public bool Visible => true;

    public bool Enabled => true;

    public int UpdateOrder => 0;

    Vector2 ICollidable.Position => _position;

    public RectangleF BorderBox => _borderBox;

    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    public void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(texture: _texture,
            position: _position,
            sourceRectangle: null,
            color: Color.White,
            rotation: 0,
            origin: new Vector2(_texture.Width / 2, _texture.Height / 2),
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0);
        _spriteBatch.End();
    }

    public void Initialize()
    {
        _position = new Vector2(_isPlayer2 == true ? _screenWidth - _texture.Width / 2 : Texture.Width / 2, _screenHeight / 2);
        _borderBox = new RectangleF(x: _position.X - (float)_texture.Width / 2, y: _position.Y - (float)_texture.Height / 2, width: _texture.Width, height: _texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        CalculatePosition(gameTime);
        _borderBox.X = _position.X - ((float)_texture.Width / 2);
        _borderBox.Y = _position.Y - ((float)_texture.Height / 2);
    }

    public void CalculatePosition(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if ((_isPlayer2 == true ? keyboardState.IsKeyDown(Keys.S) : keyboardState.IsKeyDown(Keys.Down)) && !(_position.Y + Texture.Height / 2 >= 480))
        {
            _position.Y += _playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if ((_isPlayer2 == true ? keyboardState.IsKeyDown(Keys.W) : keyboardState.IsKeyDown(Keys.Up)) && !(_position.Y - Texture.Height / 2 <= 0))
        {
            _position.Y -= _playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
