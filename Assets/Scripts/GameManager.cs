using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] public GameConfig _config;
    public TextMesh DebugText;
    public Field Field;
    public ArrowKeysDetector inputDetector;
    private TextMesh _textMesh;
    private GameConfig Config => _config;

    private void Start () {
        inputDetector = GetComponent<ArrowKeysDetector> ();
    }

    private void Update () {
        var value = inputDetector.DetectInputDirection ();
        if (value.HasValue) {
            DebugDisplay ();
            Debug.Log (value);
            RunIterator (value);
            DebugDisplay ();
        }
    }

    private IEnumerable<Point> RunIterator (InputDirection? value) {
        var iterator = FieldIterator.IterateForward (Config.fieldSize);
        List<FieldCellMovement> movementDetails = new List<FieldCellMovement>();
        if (Input.GetKeyDown ("escape")) {
            Application.Quit ();
        } else if (value == InputDirection.Left) {
            iterator = FieldIterator.IterateForward (Config.fieldSize);
            movementDetails = Field.MoveHorizontal(iterator, value);

        } else if (value == InputDirection.Right) {
            iterator = FieldIterator.IterateBackward (Config.fieldSize);
                        movementDetails = Field.MoveHorizontal(iterator, value);

        } else if (value == InputDirection.Bottom) {
            iterator = FieldIterator.IterateDownward (Config.fieldSize);
           movementDetails = Field.MoveVertical(iterator, value);
        } else if (value == InputDirection.Top) {
            iterator = FieldIterator.IterateUpward (Config.fieldSize);
            movementDetails = Field.MoveVertical(iterator, value);
        }
        if (movementDetails.Count > 0)
        {
            StartCoroutine(Field.AnimateItems(movementDetails));
        }
        Field.CreateRandomCells(1);
        DebugDisplay();
        return iterator;
    }

    private void DebugDisplay () {
        var content = "";
        content = ShowMatrixOnConsole (Field.cells);
        DebugText.text = content;
        Debug.Log (content);
    }

    public string ShowMatrixOnConsole (FieldCell[, ] matrix) {
        var size = Config.fieldSize;
        var x = string.Empty;
        var rowX = string.Empty;
        int column = 0,
            row = 0;
        for (row = size - 1; row >= 0; row--) {
            for (column = 0; column < size; column++) {
                if (matrix[column, row] != null) {
                    x += matrix[column, row].cellValue + "|";
                } else {
                    x += "X" + "|";
                }
            }
            x += "\n";
        }
        // Debug.Log(x);
        return x;
    }
    
}