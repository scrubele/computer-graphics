using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour, IconfigProvider {
    public FieldCell FieldCell;
    [SerializeField]
    public GameConfig _config;

    public GameConfig Config => _config;

    private FieldCell[, ] _cells;

    private Transform _cellsContainer;

    private void Awake () {
        _cells = new FieldCell[Config.FieldSize, Config.FieldSize];
        _cellsContainer = transform.Find ("Cells");
        createRandomCells ();

        bool direction = true;

        var iterator = direction ? FieldIterator.IterateForward(Config.FieldSize) : FieldIterator.IterateForward(Config.FieldSize);

        foreach(var point in iterator){
            Debug.Log(point.ToString());
        }

    }

    private void createRandomCells () {
        int randomCellsCount = Random.Range (1, Config.FieldSize * Config.FieldSize/2);
        int x = 0;
        while (x < randomCellsCount) {
            Point cell;
            cell = GetFreeRandomPosition ();
            createCell (cell);
            x++;
        }
    }
    private Point GetFreeRandomPosition () {
        var result = GetRandomPosition ();
        while (_cells[result.Y, result.X] != null) {
            result = GetRandomPosition ();

        }
        return result;
    }

    private Point GetRandomPosition () {
        return new Point (Random.Range (0, Config.FieldSize), Random.Range (0, Config.FieldSize));
    }
    private FieldCell createCell (Point position) {
        var cell = Instantiate (FieldCell, _cellsContainer);
        _cells[position.X, position.Y] = cell;
        cell.Position = position;
        return cell;
    }
    // Start is called before the first frame update

}