using System.Collections;

public class Node
{
    public Point Position;
    public bool Walkable
    {
        get { return WorldCell.CanMove(); }
    }
    public Cell WorldCell;

    public Node Parent;

    public int GCost;    //The cost from the start to this node
    public int HCost;    //The cost from this node to the end
    
    public int FCost    //Combined cost value
    {
        get { return GCost + HCost; }
    }

    public Node(Point position, Cell worldCell)
    {
        Position = position;
        WorldCell = worldCell;
    }
}
