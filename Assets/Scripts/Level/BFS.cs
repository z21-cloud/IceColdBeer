using System;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
     private List<Vector2Int> _directions = new List<Vector2Int>
    {
        new Vector2Int(0, 1),   // Up
        new Vector2Int(1, 0),   // Right
        new Vector2Int(0, -1),  // Down
        new Vector2Int(-1, 0),   // Left
    };

    public bool FindPath(Node startNode, Node targetNode, Node[,] nodes)
    {
        // Implementation for BFS pathfinding

        if(startNode.gridPosition == targetNode.gridPosition)
        {
            return true; // Return true if the start and target positions are the same
        }
        

        // Структура данных visitedNodes
        HashSet<Node> visitedNodes = new HashSet<Node>();
        // Структура данных toVisit
        Queue<Node> toVisit = new Queue<Node>();

        // Цикл while toVisit > 0
        // Берем первый элемент из toVisit и добавляем его в visitedNodes
        // Проверяем соседей текущего узла, если сосед не посещен и можно пройти, добавляем в ToVisit
        toVisit.Enqueue(startNode);
        visitedNodes.Add(startNode);

        while(toVisit.Count > 0)
        {
            Node currentNode = toVisit.Dequeue();

            if(currentNode.gridPosition == targetNode.gridPosition) return true; // Return true if the target node is found

            foreach(Vector2Int direction in _directions)
            {
                Vector2Int neighborPosition = currentNode.gridPosition + direction;
                // Check if the neighbor position is valid and not visited
                // If valid and not visited, add it to toVisit

                if(neighborPosition.x >= 0 && neighborPosition.x < nodes.GetLength(0) &&
                   neighborPosition.y >= 0 && neighborPosition.y < nodes.GetLength(1))
                {
                    Node neighborNode = nodes[neighborPosition.x, neighborPosition.y];
                    if(!visitedNodes.Contains(neighborNode) && neighborNode.isWalkable)
                    {
                        visitedNodes.Add(neighborNode);
                        toVisit.Enqueue(neighborNode);
                    }
                }
            }
        }


        return false; // Return false if a path is not found, true otherwise
    }
}
