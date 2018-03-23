public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Point zero()
    {
        return new Point(0, 0);
    }

    public bool Equals(Point other)
    {
        return x == other.x && y == other.y;
    }

    public override string ToString()
    {
        return "[x: " + x + "] [y: " + y + "]";
    }
}
