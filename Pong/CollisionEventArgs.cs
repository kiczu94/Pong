using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong;

public class CollisionEventArgs : EventArgs
{
    IReadOnlyCollection<int> _objectHash;

    public IReadOnlyCollection<int> ObjectHashes { get { return _objectHash; } }

    public CollisionEventArgs(IReadOnlyCollection<int> objectHash)
    {
        _objectHash = objectHash;
    }
}
