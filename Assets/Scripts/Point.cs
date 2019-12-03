using UnityEngine;

public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }


    public static Vector2 operator *(Point point, float multiplier)
    {
        return new Vector2(point.X * multiplier, point.Y * multiplier);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"{X} : {Y}";
    }
}