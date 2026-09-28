using UnityEngine;

public struct Goal
{
    public ObjectType Type;
    public Transform Transform;
    public Vector2 Position => Transform.position;

    public Goal(ObjectType type, Transform transform)
    {
        Type = type;
        Transform = transform;
    }
}
