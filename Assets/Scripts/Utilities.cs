using System;
using UnityEngine;

public class Utilities
{
    [SerializeField] private static GameConfig _config;

    private static GameConfig Config => _config;

    public static string ShowMatrixOnConsole(FieldCell[,] cells)
    {
        var size = Config.fieldSize;
        var x = string.Empty;
        for (var row = size - 1; row >= 0; row--)
        {
            for (var column = 0; column < size; column++)
                if (cells[row, column] != null)
                    x += cells[row, column].cellValue + "|";
                else
                    x += "X" + "|";
            x += Environment.NewLine;
        }

        Debug.Log(x);
        return x;
    }
}