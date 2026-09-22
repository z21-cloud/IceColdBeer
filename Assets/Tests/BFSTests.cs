using NUnit.Framework;
using UnityEngine;

public class BFSTests
{
    private BFS _bfs;

    [SetUp]
    public void SetUp()
    {
        _bfs = new BFS();
    }

    [Test]
    public void FindPath_StartNodeEqualsTargetNode_ReturnsTrue()
    {
        Node node = CreateNode(0, 0);

        bool pathExists = _bfs.FindPath(node, node, null);

        Assert.IsTrue(pathExists);
    }

    [Test]
    public void FindPath_NodesNull_ReturnsFalse()
    {
        Node startNode = CreateNode(0, 0);
        Node targetNode = CreateNode(1, 0);

        bool pathExists = _bfs.FindPath(startNode, targetNode, null);

        Assert.IsFalse(pathExists);
    }

    [Test]
    public void FindPath_NoPathFromStartNode_ReturnsFalse()
    {
        Node[,] nodes =
        {
            { CreateNode(0, 0), CreateNode(0, 1, false) },
            { CreateNode(1, 0, false), CreateNode(1, 1) }
        };

        bool pathExists = _bfs.FindPath(nodes[0, 0], nodes[1, 1], nodes);

        Assert.IsFalse(pathExists);
    }

    [Test]
    public void FindPath_PathFromStartNodeExists_ReturnsTrue()
    {
        Node[,] nodes =
        {
            { CreateNode(0, 0), CreateNode(0, 1) },
            { CreateNode(1, 0, false), CreateNode(1, 1) }
        };

        bool pathExists = _bfs.FindPath(nodes[0, 0], nodes[1, 1], nodes);

        Assert.IsTrue(pathExists);
    }

    private static Node CreateNode(int x, int y, bool isWalkable = true)
    {
        return new Node(Vector2.zero, new Vector2Int(x, y), isWalkable);
    }
}
