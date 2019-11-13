using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldIterator
 {
    public static IEnumerable < Point> IterateForward (int size) {
        return Enumerate (0, size, 1);
    }
    public static IEnumerable <Point> IterateBackward (int size) {
        return Enumerate (size - 1, -1, 1);
    }

    public static IEnumerable<Point> Enumerate (int min, int max, int direction) {
        for (int x = min; x != max; x++) {
            for (int y = min; y != max; y++) {
                yield return new Point (x, y);
            }
        }
    }
}