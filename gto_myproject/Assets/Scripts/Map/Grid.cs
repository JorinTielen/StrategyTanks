using System.Collections.Generic;

public class Grid
{
    private int _width, _height;
    private Node[,] _grid;

    public Grid(Cell[,] cellMap, int width, int height)
    {
        _width = width;
        _height = height;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cell c = cellMap[x, y];
                Node n = new Node(new Point(x, y), c.CanMove(), c);

                _grid[x, y] = n;
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        var neighbors = new List<Node>();
        var pos = node.Position;
		
        if (pos.x + 1 < _width)		neighbors.Add(_grid[pos.x + 1, pos.y]);
        if (pos.x - 1 >= 0) 		neighbors.Add(_grid[pos.x - 1, pos.y]);
        if (pos.y + 1 < _height) 	neighbors.Add(_grid[pos.x, 	pos.y + 1]);
        if (pos.y - 1 >= 0)			neighbors.Add(_grid[pos.x, 	pos.y - 1]);

        return neighbors;
    }

    public Node[,] GetNodeArray()
    {
        return _grid;
    }

    public Node GetNode(Point point)
    {
        return GetNode(point.x, point.y);
    }

    public Node GetNode(int x, int y)
    {
        if (x > _width || y > _height)
        {
            return null;
        }
        
        return _grid[x, y];
    }
}
