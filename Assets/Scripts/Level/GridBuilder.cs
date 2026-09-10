using System;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private float nodeSize;
    [SerializeField] private LayerMask unwalkableLayerMask;

    private const float gridOffset = 0.25f; // Offset to adjust the grid height
    private Vector2 minPosition;
    private Vector2 maxPosition;

    private Node[,] grid;

    public Node[,] Grid => grid;

    public void BuildGrid(Bounds spawnAreaBounds)
    {
        // Implementation for building the grid
        minPosition = spawnAreaBounds.min;
        maxPosition = spawnAreaBounds.max;

        int gridWidth = Mathf.FloorToInt((maxPosition.x - minPosition.x) / nodeSize);
        int gridHeight = Mathf.FloorToInt((maxPosition.y - minPosition.y) / nodeSize);

        grid = new Node[gridWidth, gridHeight];

        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                // Use grid offset, because the grid is not starting from 0,0, but from the offset position
                Vector2 nodePosition = new Vector2(minPosition.x + i * nodeSize + nodeSize / 2, gridOffset + j * nodeSize + nodeSize / 2);
                Vector2Int gridPosition = new Vector2Int(i, j);
                bool isWalkable = CheckIfWalkable(nodePosition); // You can add logic to determine if the node is walkable

                grid[i, j] = new Node(nodePosition, gridPosition, isWalkable);
            }
        }
    }

    public void UpdateGrid()
    {
        if (grid == null) return;

        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                Vector2 nodePosition = grid[i, j].position;
                bool isWalkable = CheckIfWalkable(nodePosition);
                grid[i, j] = new Node(nodePosition, new Vector2Int(i, j), isWalkable);
            }
        }
    }

    private bool CheckIfWalkable(Vector2 nodePosition)
    {
        // Implementation for checking if a node is walkable
        if(Physics2D.OverlapCircle(nodePosition, nodeSize / 2, unwalkableLayerMask) != null)
        {
            return false; // Node is not walkable if it overlaps with any collider
        }

        return true; // Placeholder implementation
    }

    private void OnDrawGizmos()
    {
        if (minPosition == Vector2.zero || maxPosition == Vector2.zero) return;

        int gridWidth = Mathf.FloorToInt((maxPosition.x - minPosition.x) / nodeSize);
        int gridHeight = Mathf.FloorToInt((maxPosition.y - minPosition.y) / nodeSize);

        Gizmos.color = Color.green;

        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                Vector2 nodePosition = new Vector2(minPosition.x + i * nodeSize + nodeSize / 2, gridOffset + j * nodeSize + nodeSize / 2);

                if(CheckIfWalkable(nodePosition))
                {
                    Gizmos.color = Color.green; // Walkable nodes are green
                }
                else
                {
                    Gizmos.color = Color.red; // Non-walkable nodes are red
                }

                Gizmos.DrawWireCube(nodePosition, new Vector3(nodeSize * 0.75f, nodeSize * 0.75f, 0));
            }
        }
    }
}
