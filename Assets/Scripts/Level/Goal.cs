using UnityEngine;

public struct Goal
{
    public ObjectType Type;
    public Vector2 Position;

    public Goal(ObjectType type, Vector2 position)
    {
        Type = type;
        Position = position;
    }
}
