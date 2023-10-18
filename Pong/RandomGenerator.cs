using System;
using System.Collections.Generic;
using System.Linq;

namespace Pong;

public static class RandomGenerator
{
    public static T GetFromCollection<T>(ICollection<T> collection)
    {
        var index = new Random().Next(collection.Count);
        return collection.ToArray()[index];
    }
}
