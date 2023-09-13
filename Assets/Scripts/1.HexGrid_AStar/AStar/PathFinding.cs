using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static List<HexTile> FindPath(HexTile origin, HexTile destination)
    {
        Dictionary<HexTile, PathNode> openNodes = new Dictionary<HexTile, PathNode>();
        Dictionary<HexTile, PathNode> closedNodes = new Dictionary<HexTile, PathNode>();

        PathNode startNode = new PathNode(origin, origin, destination, 0);
        openNodes.Add(origin, startNode);

        bool pathFound = EvaluateNextNode(openNodes, closedNodes, origin, destination, out List<HexTile> path);

        while (!pathFound)
        {
            pathFound = EvaluateNextNode(openNodes, closedNodes, origin, destination, out path);
        }

        return path;
    }

    private static bool EvaluateNextNode(Dictionary<HexTile, PathNode> openNodes, Dictionary<HexTile, PathNode> closedNodes, HexTile origin, HexTile destination, out List<HexTile> path)
    {
        PathNode currentNode = GetCheapestNode(openNodes.Values.ToArray());

        //if (currentNode == null)
        //{
        //    path = new List<HexTile>();
        //    return false;
        //}

        openNodes.Remove(currentNode.target);
        closedNodes.Add(currentNode.target, currentNode);

        path = new List<HexTile>();

        if (currentNode.target == destination)
        {
            path.Add(currentNode.target);

            while (currentNode.target != origin)
            {
                path.Add(currentNode.target);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            return true;
        }

        List<PathNode> neighbours = new List<PathNode>();
        foreach (HexTile tile in currentNode.target.neighbours)
        {
            PathNode node = new PathNode(tile, origin, destination, currentNode.GetCost());

            if (tile.TILE_STATUS == HexTileStatus.INVALID)
            {
                node.baseCost = 9999999;
            }

            neighbours.Add(node);
        }

        foreach (PathNode neighbour in neighbours)
        {
            if (closedNodes.Keys.Contains(neighbour.target)) { continue; }

            if (neighbour.GetCost() < currentNode.GetCost() || !openNodes.Keys.Contains(neighbour.target))
            {
                neighbour.SetParent(currentNode);

                if (!openNodes.Keys.Contains(neighbour.target))
                {
                    openNodes.Add(neighbour.target, neighbour);
                }
            }
        }

        return false;
    }

    private static PathNode GetCheapestNode(PathNode[] openNodes)
    {
        if (openNodes.Length == 0) { return null; }

        PathNode selectedNode = openNodes[0];

        for (int i = 0; i < openNodes.Length; i++)
        {
            var currentNode = openNodes[i];

            if (currentNode.GetCost() < selectedNode.GetCost())
            {
                selectedNode = currentNode;
            }
            else if (currentNode.GetCost() == selectedNode.GetCost() && currentNode.costToDestination < selectedNode.costToDestination)
            {
                selectedNode = currentNode;
            }
        }

        return selectedNode;
    }
}
