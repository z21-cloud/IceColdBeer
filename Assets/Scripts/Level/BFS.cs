using System.Collections.Generic;
using UnityEngine;

public class BFS
{
     // Направления перемещения: вверх, вправо, вниз, влево
    private List<Vector2Int> _directions = new List<Vector2Int>
    {
        new Vector2Int(0, 1),   // Вверх
        new Vector2Int(1, 0),   // Вправо
        new Vector2Int(0, -1),  // Вниз
        new Vector2Int(-1, 0),  // Влево
    };

    public bool FindPath(Node startNode, Node targetNode, Node[,] nodes)
    {
        if(nodes == null) return false;
        
        // Если старт уже совпадает с целью, путь найден сразу
        if(startNode.gridPosition == targetNode.gridPosition)
        {
            return true;
        }

        // visitedNodes — все клетки, которые уже были проверены или добавлены в очередь
        HashSet<Node> visitedNodes = new HashSet<Node>();

        // toVisit — очередь клеток, которые нужно проверить
        Queue<Node> toVisit = new Queue<Node>();

        // Добавляем стартовую клетку в очередь и помечаем её как посещённую
        toVisit.Enqueue(startNode);
        visitedNodes.Add(startNode);

        // Пока есть клетки для проверки, продолжаем обход
        while(toVisit.Count > 0)
        {
            // Берём первую клетку из очереди
            Node currentNode = toVisit.Dequeue();

            // Если эта клетка — цель, значит путь найден
            if(currentNode.gridPosition == targetNode.gridPosition)
            {
                return true;
            }

            // Проверяем всех соседей текущей клетки
            foreach(Vector2Int direction in _directions)
            {
                // Соседняя позиция относительно текущей клетки
                Vector2Int neighborPosition = currentNode.gridPosition + direction;

                // Проверяем, что сосед внутри границ сетки
                if(neighborPosition.x >= 0 && neighborPosition.x < nodes.GetLength(0) &&
                   neighborPosition.y >= 0 && neighborPosition.y < nodes.GetLength(1))
                {
                    // Получаем соседнюю клетку из сетки
                    Node neighborNode = nodes[neighborPosition.x, neighborPosition.y];

                    // Добавляем в очередь только непосещённые и проходимые клетки
                    if(!visitedNodes.Contains(neighborNode) && neighborNode.isWalkable)
                    {
                        visitedNodes.Add(neighborNode);
                        toVisit.Enqueue(neighborNode);
                    }
                }
            }
        }

        // Если очередь опустела, путь не найден
        return false;
    }
}
