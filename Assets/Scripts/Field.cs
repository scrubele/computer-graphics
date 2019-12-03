using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Field : MonoBehaviour, IConfigProvider {
    private FieldCell[, ] _cells;
    private Transform _cellsContainer;
    public FieldCell FieldCell2, FieldCell4, FieldCell8, FieldCell16, FieldCell32, FieldCell64, FieldCell128, FieldCell256, FieldCell512, FieldCell1024, blankFieldCell;
    [SerializeField] public GameConfig config;
    private int ZIndex = 0, score = 0;
    public TextMesh ScoreText;

    private readonly Random _cellValueRandom = new Random ();

    public FieldCell[, ] cells {
        get => _cells;
        set => cells = _cells;
    }

    public FieldCell this [int row, int column] {
        get {
            return _cells[row, column];
        }
        set {
            _cells[row, column] = value;
        }
    }

    public GameConfig Config => config;

    private void Awake () {
        _cells = new FieldCell[Config.fieldSize, Config.fieldSize];
        _cellsContainer = transform.Find ("Cells");
        // ScoreText = transform.Find("Score").GetComponent<TextMesh>();
        CreateRandomCells (2);
        UpdateScore(0);
    }

    public void CreateRandomCells (int randomCellsCount) {
        var x = 0;
        while (x < randomCellsCount) {
            var cell = GetFreeRandomPosition ();
            CreateCell (cell, 2, true);
            x++;
        }
        ShowCellsOnConsole (_cells);
    }

    private Point GetFreeRandomPosition () {
        var result = GetRandomPosition ();
        while (_cells[result.X, result.Y] != null) result = GetRandomPosition ();
        return result;
    }

    private Point GetRandomPosition () {
        return new Point (UnityEngine.Random.Range (0, Config.fieldSize), UnityEngine.Random.Range (0, Config.fieldSize));
    }

    private void CreateCell (Point position, int cellValue = 2, bool isRandom = false) {
        if (isRandom) {
            cellValue = _cellValueRandom.Next (2, 20) % 5 == 0 ? 4 : 2;
        }
        var cell = Instantiate (GetFieldCellBasedOnCellValue (cellValue), _cellsContainer);
        cell.cellValue = cellValue;
        cell.transform.scaleTo (Config.animationDuration, new Vector3 (1.0f, 1.0f, 1.0f));
        cell.position = position;
        _cells[cell.position.X, cell.position.Y] = cell;
    }

    public List<FieldCellMovement> MoveVertical (IEnumerable<Point> iterator, InputDirection? horizontalMovement) {
        ResetDuplicatedValues ();

        var movementDetails = new List<FieldCellMovement> ();
        int horizontalDirection = horizontalMovement == InputDirection.Bottom ? -1 : 1;
        foreach (Point point in iterator) {
            int row = point.X;
            int column = point.Y;
            if (_cells[row, column] == null) continue;
            FieldCellMovement imd = AreTheseTwoFieldCellsSame (row, column, row, column + horizontalDirection);
            if (imd != null) {
                movementDetails.Add (imd);
                continue;
            }
            int columnFirstNullItem;
            bool emptyItemFound;
            (columnFirstNullItem, emptyItemFound) = findNullColumn (iterator, horizontalMovement, row, column);
            if (!emptyItemFound) {
                continue;
            }
            FieldCellMovement newImd =
                MoveFieldCellToNull (row, row, row, column, columnFirstNullItem, columnFirstNullItem + horizontalDirection);

            movementDetails.Add (newImd);
        }
        return movementDetails;
    }

    public List<FieldCellMovement> MoveHorizontal (IEnumerable<Point> iterator, InputDirection? horizontalMovement) {
        ResetDuplicatedValues ();

        var movementDetails = new List<FieldCellMovement> ();
        int horizontalDirection = horizontalMovement == InputDirection.Left ? -1 : 1;
        foreach (Point point in iterator) {
            int row = point.X;
            int column = point.Y;

            if (_cells[row, column] == null) continue;
            FieldCellMovement imd = AreTheseTwoFieldCellsSame (row, column, row + horizontalDirection, column);
            if (imd != null) {
                movementDetails.Add (imd);
                continue;
            }

            int columnFirstNullItem;
            bool emptyItemFound;
            (columnFirstNullItem, emptyItemFound) = findNullRow (iterator, horizontalMovement, row, column);

            if (!emptyItemFound) {
                continue;
            }

            FieldCellMovement newImd =
                MoveFieldCellToNull (row, columnFirstNullItem, columnFirstNullItem + horizontalDirection, column, column, column);

            movementDetails.Add (newImd);

        }
        return movementDetails;
    }

    private void ResetDuplicatedValues () {
        for (int row = 0; row < Config.fieldSize; row++)
            for (int column = 0; column < Config.fieldSize; column++) {
                if (_cells[row, column] != null && _cells[row, column].WasJustDuplicated)
                    _cells[row, column].WasJustDuplicated = false;
            }
    }

    private (int, bool) findNullColumn (IEnumerable<Point> iterator, InputDirection? horizontalMovement, int row, int column) {
        int columnFirstNullItem = -1;
        int numberOfItemsToTake = horizontalMovement == InputDirection.Bottom ? column : Config.fieldSize - column;
        bool emptyItemFound = false;
        foreach (var tempPoint in iterator.Take (numberOfItemsToTake)) {
            var tempColumnFirstNullItem = tempPoint.Y;
            columnFirstNullItem = tempColumnFirstNullItem;
            if (_cells[row, columnFirstNullItem] == null) {
                emptyItemFound = true;
                break;
            }
        }
        return (columnFirstNullItem, emptyItemFound);
    }
    private (int, bool) findNullRow (IEnumerable<Point> iterator, InputDirection? verticalMovement, int row, int column) {
        int rowFirstNullItem = -1;

        int numberOfItemsToTake = verticalMovement == InputDirection.Left ?
            row : Config.fieldSize - row;

        bool emptyItemFound = false;

        foreach (var tempPoint in iterator.Take (numberOfItemsToTake)) {
            var tempRowFirstNullItem = tempPoint.Y;
            rowFirstNullItem = tempRowFirstNullItem;
            if (_cells[rowFirstNullItem, column] == null) {
                emptyItemFound = true;
                break;
            }
        }
        return (rowFirstNullItem, emptyItemFound);
    }

    private FieldCellMovement MoveFieldCellToNull
        (int oldRow, int newRow, int itemToCheckRow, int oldColumn, int newColumn, int itemToCheckColumn) {

            _cells[newRow, newColumn] = _cells[oldRow, oldColumn];
            _cells[oldRow, oldColumn] = null;
            FieldCellMovement imd2 = AreTheseTwoFieldCellsSame (newRow, newColumn, itemToCheckRow, itemToCheckColumn);
            if (imd2 != null) {
                return imd2;
            } else {
                return new FieldCellMovement (newRow, newColumn, _cells[newRow, newColumn], null);

            }
        }

    private FieldCellMovement AreTheseTwoFieldCellsSame (
        int originalRow, int originalColumn, int toCheckRow, int toCheckColumn) {
        if (toCheckRow < 0 || toCheckColumn < 0 || toCheckRow >= Config.fieldSize || toCheckColumn >= Config.fieldSize)
            return null;

        if (_cells[originalRow, originalColumn] != null && _cells[toCheckRow, toCheckColumn] != null &&
            _cells[originalRow, originalColumn].cellValue == _cells[toCheckRow, toCheckColumn].cellValue &&
            !_cells[toCheckRow, toCheckColumn].WasJustDuplicated) {

            _cells[toCheckRow, toCheckColumn].cellValue *= 2;
            _cells[toCheckRow, toCheckColumn].WasJustDuplicated = true;

            var FieldCellToAnimateScaleCopy = _cells[originalRow, originalColumn];
            _cells[originalRow, originalColumn] = null;
            return new FieldCellMovement (toCheckRow, toCheckColumn, _cells[toCheckRow, toCheckColumn], FieldCellToAnimateScaleCopy);
        } else {
            return null;
        }
    }
    public IEnumerator AnimateItems (IEnumerable<FieldCellMovement> movementDetails) {
        List<FieldCell> objectsToDestroy = new List<FieldCell> ();
        foreach (var item in movementDetails) {
            var newGoPosition = new Vector3 (item.newRow * 3, item.newColumn * 3, -1);
            var tween = item.NewFieldCell.transform.positionTo (Config.animationDuration, newGoPosition);
            tween.autoRemoveOnComplete = true;
            if (item.OriginalFieldCell != null) {
                var duplicatedItem = _cells[item.newRow, item.newColumn];
                CreateCell (new Point (item.newRow, item.newColumn), duplicatedItem.cellValue);
                UpdateScore(duplicatedItem.cellValue);

                var moveTween = new GoTween (item.OriginalFieldCell.transform, Config.animationDuration, new GoTweenConfig ().position (newGoPosition));
                var scaleTween = new GoTween (item.OriginalFieldCell.transform, Config.animationDuration, new GoTweenConfig ().scale (0.1f));

                var chain = new GoTweenChain ();
                chain.autoRemoveOnComplete = true;
                chain.append (moveTween).appendDelay (Config.animationDuration).append (scaleTween);
                chain.play ();

                objectsToDestroy.Add (item.OriginalFieldCell);
                objectsToDestroy.Add (item.NewFieldCell);
            }
        }

        yield return new WaitForSeconds (Config.animationDuration * movementDetails.Count () * 3);
        foreach (var go in objectsToDestroy)
            Destroy (go.gameObject);
    }

    private void UpdateScore(int toAdd)
    {
        score += toAdd;
        ScoreText.text = score+"";
    }
    public string ShowCellsOnConsole (FieldCell[, ] _cells) {
        var size = Config.fieldSize;
        var x = string.Empty;
        var rowX = string.Empty;
        int column = 0,
            row = 0;
        for (row = size - 1; row >= 0; row--) {
            for (column = 0; column < size; column++) {
                if (_cells[row, column] != null) {
                    x += _cells[row, column].cellValue + "|";
                } else {
                    x += "X" + "|";
                }
            }
            x += "\n";
        }
        // Debug.Log(x);
        return x;
    }

    private FieldCell GetFieldCellBasedOnCellValue (int value) {
        FieldCell newGo = null;
        switch (value) {
            case 2:
                newGo = FieldCell2;
                break;
            case 4:
                newGo = FieldCell4;
                break;
            case 8:
                newGo = FieldCell8;
                break;
            case 16:
                newGo = FieldCell16;
                break;
            case 32:
                newGo = FieldCell32;
                break;
            case 64:
                newGo = FieldCell64;
                break;
            case 128:
                newGo = FieldCell128;
                break;
            case 256:
                newGo = FieldCell256;
                break;
            case 512:
                newGo = FieldCell512;
                break;
            case 1024:
                newGo = FieldCell1024;
                break;
            default:
                throw new System.Exception ("Uknown value:" + value);
        }
        return newGo;
    }
}