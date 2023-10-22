using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Pong;

public class Score : IGameComponent, IDrawable, IUpdateable
{
    public bool _isGameEnded = false;
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
    private SpriteFont _font;

    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    public event EventHandler<GameEndedEventArgs> GameEnded;

    public Score(SpriteBatch spriteBatch, Dictionary<int, Texture2D> scoreTextures, SpriteFont font)
    {
        _spriteBatch = spriteBatch;
        _scoreTextures = scoreTextures;
        _font = font;
    }



    public void Draw(GameTime gameTime)
    {
        if (_isGameEnded)
        {
            DrawWhoWon(_player1Score >= 10 ? "1" : "2");
            DrawInformationAboutRestart();
            return;
        }
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
        if (CheckIfGameEnded())
        {
            _isGameEnded = true;
            GameEnded?.Invoke(this, new GameEndedEventArgs(Player1Score >= 10 ? true : false));
        }
    }

    private void DrawPlayerScore(bool playerOne)
    {
        if (_scoreTextures.TryGetValue(playerOne ? _player1Score : _player2Score, out Texture2D scoreTexture))
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: scoreTexture,
                position: new Vector2(playerOne ? 380 : 420, 50),
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

    public void OnGameRestarted(object sender, EventArgs eventArgs)
    {
        _isGameEnded = false;
        _player1Score = 0;
        _player2Score = 0;
    }

    private void DrawWhoWon(string playerNumber)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, $"Player {playerNumber} won", new Vector2(100,100), Color.Black);
        _spriteBatch.End();
    }

    private void DrawInformationAboutRestart()
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, $"Press Enter to restart", new Vector2(100, 150), Color.Black);
        _spriteBatch.End();
    }

    private bool CheckIfGameEnded() => (_player1Score >= 10 || _player2Score >= 10) ? true : false;

}
