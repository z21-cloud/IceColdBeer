using UnityEngine;

public struct Goal
{
    public ObjectType Type;
    public Transform Transform;
    public Vector2 Position => Transform.position;
    public int Index;

    // index -1 = WinHole

    public Goal(ObjectType type, Transform transform, int index = -1)
    {
        Type = type;
        Transform = transform;
        Index = index;
    }
}
