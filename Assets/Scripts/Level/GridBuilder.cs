using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder
{
    private readonly float nodeSize;
    private readonly LayerMask unwalkableLayerMask;

    private const float gridOffset = 0.25f; // Offset to adjust the Grid height

    private Vector2 minPosition;
    private Vector2 maxPosition;
    private int gridWidth;
    private int gridHeight;

    public Node[,] Grid {get; private set;}

    public GridBuilder(float nodeSize, LayerMask unwalkableLayerMask)
    {
        this.nodeSize = nodeSize;
        this.unwalkableLayerMask = unwalkableLayerMask;
    }

    public void BuildGrid(Bounds spawnAreaBounds)
    {
        // Implementation for building the Grid
        minPosition = spawnAreaBounds.min;
        maxPosition = spawnAreaBounds.max;

        gridWidth = Mathf.FloorToInt((maxPosition.x - minPosition.x) / nodeSize);
        gridHeight = Mathf.FloorToInt((maxPosition.y - minPosition.y) / nodeSize);

        Grid = new Node[gridWidth, gridHeight];

        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                // Use Grid offset, because the Grid is not starting from 0,0, but from the offset position
                Vector2 nodePosition = new Vector2(minPosition.x + i * nodeSize + nodeSize / 2, gridOffset + j * nodeSize + nodeSize / 2);
                Vector2Int gridPosition = new Vector2Int(i, j);
                bool isWalkable = CheckIfWalkable(nodePosition); // You can add logic to determine if the node is walkable

                Grid[i, j] = new Node(nodePosition, gridPosition, isWalkable);
            }
        }
    }

    public bool TryGetNodePosition(Vector2 position, out Node node)
    {
        // Implementation for getting a node at a specific position
        int x = Mathf.FloorToInt((position.x - minPosition.x) / nodeSize);
        int y = Mathf.FloorToInt((position.y - gridOffset) / nodeSize);

        if(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            //Debug.Log($"[GridBuilder] GetNodeAtPosition - x: {x}, y: {y}, position: {position}");
            node = Grid[x, y];
            return true;
        }

        Debug.LogError($"[GridBuilder] GetNodeAtPosition - Position {position} is out of bounds!");
        // Return the default node if the position is out of bounds
        node = default;
        return false; 
    }

    private bool CheckIfWalkable(Vector2 nodePosition)
    {
        // Implementation for checking if a node is walkable
        if(Physics2D.OverlapCircle(nodePosition, nodeSize * 0.35f, unwalkableLayerMask) != null)
        {
            return false; // Node is not walkable if it overlaps with any collider
        }

        return true; // Placeholder implementation
    }
}