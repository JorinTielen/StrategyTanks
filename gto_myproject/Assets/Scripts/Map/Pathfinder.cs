using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Map _map;
    private Grid _grid;

    private void Awake()
    {
        _map = GetComponent<Map>();
        _grid = new Grid(_map.GetCellArray(), _map.Width, _map.Height);
    }

    void FindPath(Cell startCell, Cell endCell)
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
                return;
            }

            foreach (var neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;
                
                
            }
        }
    }
}
