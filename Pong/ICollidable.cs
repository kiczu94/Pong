using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Pong;

public interface ICollidable
{
    public Vector2 Position { get; }
    public Texture2D Texture { get; }
    public RectangleF BorderBox { get; }
}
