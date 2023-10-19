using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong;

public class PointGrantedEventArgs : EventArgs
{
    private bool _playerOneGetPoint;
    public bool PlayerOneGetPoint { get { return _playerOneGetPoint; } }

    public PointGrantedEventArgs(bool playerOneGetPoint = true)
    {
        _playerOneGetPoint = playerOneGetPoint;
    }
}
