using System.Collections;

public class Node
{
    public Point Position;
    public bool Walkable;
    public Cell WorldCell;

    public int GCost;
    public int HCost;
    
    public int FCost
    {
        get { return GCost + HCost; }
    }

    public Node(Point position, bool walkable, Cell worldCell)
    {
        Position = position;
        Walkable = walkable;
        WorldCell = worldCell;
    }
}
