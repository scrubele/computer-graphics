using System.Collections.Generic;
using UnityEngine;

public class GameManagerCopy : MonoBehaviour
{
    [SerializeField] public GameConfig _config;

    public TextMesh DebugText;
    public Field Field;
    public IInputDetector inputDetector;
    public GameConfig Config => _config;

    private void Start()
    {
        inputDetector = GetComponent<IInputDetector>();
    }

    private void Update()
    {
        var value = inputDetector.DetectInputDirection();
        IEnumerable<Point> iterator;
        if (value.HasValue)
        {
            DebugDisplay();
            Debug.Log(value);
            iterator = runIterator(value);
            DebugDisplay();
        }
    }

    private IEnumerable<Point> runIterator(InputDirection? value)
    {
        var iterator = FieldIterator.IterateForward(Config.fieldSize);
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
        else if (value == InputDirection.Left)
        {
            iterator = FieldIterator.IterateForward(Config.fieldSize);
            moveCellsHorizontally(iterator);
        }
        else if (value == InputDirection.Right)
        {
            iterator = FieldIterator.IterateBackward(Config.fieldSize);
            moveCellsHorizontally(iterator);
        }
        else if (value == InputDirection.Bottom)
        {
            iterator = FieldIterator.IterateDownward(Config.fieldSize);
            moveCellsVertically(iterator);
        }
        else if (value == InputDirection.Top)
        {
            iterator = FieldIterator.IterateUpward(Config.fieldSize);
            moveCellsVertically(iterator);
        }

        return iterator;
    }

    private void moveCellsVertically(IEnumerable<Point> iterator)
    {
        var enumerator = iterator.GetEnumerator();
        enumerator.MoveNext();

        var initialSecondAxis = enumerator.Current.Y;
        var initialFirstAxis = 0;
        var secondAxis = initialSecondAxis;
        var secondAxisDirection = secondAxis > 0 ? -1 : 1;
        int[] usedCellsLevel = {0, 0, 0, 0};
        foreach (var point in iterator)
        {
            var pointX = point.X;
            var pointY = point.Y;
            Debug.Log(pointX + " " + pointY + " " + initialFirstAxis + " " + usedCellsLevel[initialFirstAxis]);
            var isLocationChanged = false;
            if (Field.cells[pointX, pointY] != null &&
                (usedCellsLevel[initialFirstAxis] - pointY) * secondAxisDirection >= 0)
                while (isLocationChanged == false)
                {
                    if (Field.cells[pointX, usedCellsLevel[initialFirstAxis]] == null)
                    {
                        Debug.Log(pointX + " " + pointY + " " + initialFirstAxis + " " + secondAxis);
                        switchValue(pointX, pointY, pointX, usedCellsLevel[initialFirstAxis], 1, secondAxisDirection);
                        isLocationChanged = true;
                    }

                    DebugDisplay();
                    try
                    {
                        if (Field.cells[pointX, usedCellsLevel[initialFirstAxis] - secondAxisDirection].cellValue ==
                            Field.cells[pointX, usedCellsLevel[initialFirstAxis]].cellValue)
                        {
                            Debug.Log(point + "Yes");
                            switchValue(pointX, usedCellsLevel[initialFirstAxis], pointX,
                                usedCellsLevel[initialFirstAxis] - secondAxisDirection, 2, secondAxisDirection);
                        }
                    }
                    catch
                    {
                    }

                    usedCellsLevel[initialFirstAxis] += secondAxisDirection;
                }

            initialFirstAxis++;
            if (initialFirstAxis >= Config.fieldSize || initialFirstAxis < 0)
            {
                initialFirstAxis %= Config.fieldSize;
                secondAxis = initialSecondAxis;
            }
        }

        DebugDisplay();
        Field.CreateRandomCells(1);
    }

    private void moveCellsHorizontally(IEnumerable<Point> iterator)
    {
        var enumerator = iterator.GetEnumerator();
        enumerator.MoveNext();

        var initialFirstAxis = enumerator.Current.X;
        var initialSecondAxis = enumerator.Current.Y;
        var secondAxis = initialSecondAxis;
        var secondAxisDirection = secondAxis > 0 ? -1 : 1;
        foreach (var point in iterator)
        {
            var pointX = point.Y;
            var pointY = point.X;
            Debug.Log(point + " " + secondAxis);
            var isLocationChanged = false;
            if (Field.cells[pointX, pointY] != null)
                while (isLocationChanged == false && (secondAxis - pointY) * secondAxisDirection > 0)
                {
                    if (Field.cells[pointX, secondAxis] == null)
                    {
                        Debug.Log(point + " " + secondAxis + pointY);
                        switchValue(pointX, pointY, secondAxis, pointY, 1, secondAxisDirection);
                        isLocationChanged = true;
                    }

                    DebugDisplay();
                    try
                    {
                        if (Field.cells[secondAxis - secondAxisDirection, pointY].cellValue ==
                            Field.cells[secondAxis, pointY].cellValue)
                        {
                            Debug.Log(point + "Yes");
                            switchValue(pointX, secondAxis, pointX, secondAxis - secondAxisDirection, 2,
                                secondAxisDirection);
                        }
                    }
                    catch
                    {
                    }

                    secondAxis += secondAxisDirection;
                }

            initialFirstAxis++;
            if (initialFirstAxis > Config.fieldSize)
            {
                initialFirstAxis %= Config.fieldSize;
                secondAxis = initialSecondAxis;
            }
        }

        DebugDisplay();
        Field.CreateRandomCells(1);
    }

    private void switchValue(int firstX, int firstY, int secondX, int secondY, int multiplier, int secondAxisDirection)
    {
        if (firstY != secondY)
        {
            Field.cells[secondX, secondY] = null;
            Destroy(Field.cells[firstX, secondY]);
            Field.cells[secondX, secondY] = Field.cells[firstX, firstY];
            Field.cells[secondX, secondY].position = new Point(secondX, secondY);
            Field.cells[secondX, secondY].cellValue = Field.cells[firstX, firstY].cellValue * multiplier;
            Field.cells[secondX, secondY].GetComponentInChildren<TextMesh>().text =
                Field.cells[secondX, secondY].cellValue.ToString();
            Field.cells[firstX, firstY] = null;
            Destroy(Field.cells[firstX, firstY]);
            if (multiplier > 1) Field.cells[firstX, secondY + secondAxisDirection] = null;
        }
    }

    private void DebugDisplay()
    {
        var content = ShowMatrixOnConsole(Field.cells);
        DebugText.text = content;
        Debug.Log(content);
    }

    public string ShowMatrixOnConsole(FieldCell[,] Cells)
    {
        var size = Config.fieldSize;
        var x = string.Empty;
        var rowX = string.Empty;
        int column = 0,
            row = 0;
        for (column = size - 1; column >= 0; column -= 1)
        {
            x += column + " ";
            for (row = 0; row < size; row += 1)
            {
                rowX = "X" + "|";
                if (Cells[row, column] != null) rowX = Cells[row, column].cellValue + "|";

                x += rowX;
                //Debug.Log(rowX);
            }

            x += "\n";
        }

        return x;
    }
}