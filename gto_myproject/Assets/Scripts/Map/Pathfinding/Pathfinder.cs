using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Map _map;
    private Grid _grid;

    private void Awake()
    {
        _map = GameObject.Find("Map").GetComponent<Map>();
        _grid = new Grid(_map.GetCellArray(), _map.Width, _map.Height);
    }

    public List<Node> FindPath(Cell startCell, Cell endCell)
    {
        Node startNode = _grid.GetNode(startCell.Position);
        Node endNode = _grid.GetNode(endCell.Position);
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        //Loop through entire openset
        while (openSet.Count > 0)
        {
            var currentNode = openSet[0];

            //Get the node with the smallest FCost.
            for (var i = 1; i < openSet.Count; i++)
                if (openSet[i].FCost < currentNode.FCost ||
                    openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    currentNode = openSet[i];
            
            //Remove current from open and add to closed
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            //If we are at the target, we're done
            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            //Loop through all neighbours
            foreach (var neighbour in _grid.GetNeighbours(currentNode))
            {
                //Skip if you can't walk on it or if we already checked it
                if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                //Calculate costs
                var newGCostToNeighbour = GuessDistance(currentNode, neighbour);
                if (newGCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newGCostToNeighbour;
                    neighbour.HCost = GuessDistance(neighbour, endNode);
                    neighbour.Parent = currentNode;
                    
                    openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        return path;
    }

    private int GuessDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int distY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }
}
