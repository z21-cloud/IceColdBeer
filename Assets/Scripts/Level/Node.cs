using UnityEngine;

public struct Node
{
    public Vector2 position;
    public Vector2Int gridPosition;
    public bool isWalkable;

    public Node(Vector2 pos, Vector2Int gridPos, bool walkable)
    {
        position = pos;
        gridPosition = gridPos;
        isWalkable = walkable;
    }
}
