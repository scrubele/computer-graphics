using UnityEngine;
using UnityEngine.Serialization;

public class FieldCell : Cell
{
    [FormerlySerializedAs("CellValue")] private int _cellValue;

    private readonly TextMesh _text;
    public bool WasJustDuplicated { get; set; }

    public string text
    {
        get => _text.text;
        set => _text.text = value;
    }
    
    public int cellValue
    {
        get => _cellValue;
        set => _cellValue = value;
    }
}