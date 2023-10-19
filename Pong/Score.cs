using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pong;

public class Score : IGameComponent, IDrawable, IUpdateable
{

    public bool Enabled => true;
    public int UpdateOrder => 0;
    public int DrawOrder => 0;
    public bool Visible => true;
    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;

    private readonly SpriteBatch _spriteBatch;
    private readonly Dictionary<int, Texture2D> _scoreTextures;
    private int _player1Score;
    private int _player2Score;

    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;

    public Score(SpriteBatch spriteBatch, Dictionary<int, Texture2D> scoreTextures)
    {
        _spriteBatch = spriteBatch;
        _scoreTextures = scoreTextures;
    }

    public void Draw(GameTime gameTime)
    {
        DrawPlayerScore(true);
        DrawPlayerScore(false);
    }

    public void Initialize()
    {
        _player1Score = 0;
        _player2Score = 0;
    }

    public void Update(GameTime gameTime)
    {

    }

    private void DrawPlayerScore(bool playerOne)
    {
        Texture2D scoreTexture;
        if (_scoreTextures.TryGetValue(playerOne ? _player1Score : _player2Score, out scoreTexture))
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: scoreTexture,
                position: new Vector2(playerOne? 380: 420, 50),
                sourceRectangle: null,
                Color.White,
                rotation: 0,
                origin: new Vector2(scoreTexture.Width / 2, scoreTexture.Height / 2),
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: 0);
            _spriteBatch.End();
        }
    }

    public void OnScoreChange(object sender, PointGrantedEventArgs eventArgs)
    {
        if (eventArgs.PlayerOneGetPoint)
            _player1Score++;
        if (!eventArgs.PlayerOneGetPoint)
            _player2Score++;
    }
}
