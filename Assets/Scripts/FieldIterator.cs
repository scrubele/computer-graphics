using System.Collections.Generic;

public static class FieldIterator
{
    public static IEnumerable<Point> IterateForward(int size)
    {
        return Enumerate(size, 0, 0);
    }

    public static IEnumerable<Point> IterateBackward(int size)
    {
        return Enumerate(size, 0, 1);
    }

    public static IEnumerable<Point> IterateDownward(int size)
    {
        return Enumerate(size, 1, 0);
    }

    public static IEnumerable<Point> IterateUpward(int size)
    {
        return Enumerate(size, 1, 1);
    }

    public static IEnumerable<Point> Enumerate(int size, int isXGrowing, int isYGrowing)
    {
        var startXIndex = isXGrowing * (size - 1);
        var endXIndex = (isXGrowing ^ 1) * size == 0 ? -1 : (isXGrowing ^ 1) * size;
        var xDirection = GetDirection(isXGrowing);

        var startYIndex = isYGrowing * (size - 1);
        var endYIndex = (isYGrowing ^ 1) * size == 0 ? -1 : (isYGrowing ^ 1) * size;
        var yDirection = GetDirection(isYGrowing);

        for (var x = startXIndex; x != endXIndex; x += xDirection)
        for (var y = startYIndex; y != endYIndex; y += yDirection)
            yield
                return new Point(x, y);
    }

    private static int GetDirection(int value)
    {
        if (value > 0)
            return -1;
        return 1;
    }
}