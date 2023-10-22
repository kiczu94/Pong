using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class GameEndedEventArgs : EventArgs
    {
        private readonly bool _player1Won;
        public bool Player1Won { get { return _player1Won; }}
        public GameEndedEventArgs(bool player1Won)
        {
            _player1Won = player1Won;
        }
    }
}
