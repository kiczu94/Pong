using System;
using System.Collections.Generic;
using System.Linq;

namespace Pong;

public class CollisionService
{
    public ICollection<ICollidable> objects = new List<ICollidable>();

    public event EventHandler<CollisionEventArgs> OnCollisionDetected;

    public void CheckCollisions()
    {
        var objectsArray = objects.ToList();
        for (int i = 0; i < objectsArray.Count; i++)
        {
            var comparableArray = objectsArray.Take(new Range(i + 1, objectsArray.Count)).ToList();
            CheckCollision(objectsArray[i], comparableArray);
        }
    }

    public void AddObject(ICollidable texture)
    {
        objects.Add(texture);
    }

    private void CheckCollision(ICollidable mainObject, ICollection<ICollidable> textures)
    {
        foreach (var texture in textures)
        {
            if (Checkborders(mainObject, texture))
            {
                OnCollisionDetected?.Invoke(this, new CollisionEventArgs(new[] { mainObject.GetHashCode(), texture.GetHashCode() }));
            }

        }
    }

    private bool Checkborders(ICollidable mainObject, ICollidable secondObject)
    {
        return mainObject.BorderBox.IntersectsWith(secondObject.BorderBox);
    }
}
