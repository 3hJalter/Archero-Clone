using System.Collections.Generic;
using UnityEngine;

public abstract class Cache
{
    private static readonly Dictionary<Collider, Entity> Entities = new();
    private static readonly Dictionary<Collider, IInteractWallObject> InteractWallObjects = new(); 
    
    public static IInteractWallObject GetInteractWallObject(Collider collider)
    {
        if (!InteractWallObjects.ContainsKey(collider)) InteractWallObjects.Add(collider, collider.GetComponent<IInteractWallObject>());
        return InteractWallObjects[collider];
    }

    public static Entity GetEntity(Collider collider)
    {
        if (!Entities.ContainsKey(collider)) Entities.Add(collider, collider.GetComponent<Entity>());
        return Entities[collider];
    }

    public static Player GetPlayer(Collider collider)
    {
        if (!Entities.ContainsKey(collider)) Entities.Add(collider, collider.GetComponent<Entity>());
        return (Player)Entities[collider];
    }

    public static Enemy GetEnemy(Collider collider)
    {
        if (!Entities.ContainsKey(collider)) Entities.Add(collider, collider.GetComponent<Entity>());
        return (Enemy)Entities[collider];
    }
}
