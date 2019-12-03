using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Config", menuName = "Config/Create Game Config", order = 1)]
public class GameConfig : ScriptableObject
{
    private readonly int _fieldSize = 4;
    [FormerlySerializedAs("CellSize")] public float cellSize = 3;
    private static readonly float _animationDuration =  0.05f;

    public int fieldSize
    {
        get => _fieldSize;
        private set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            fieldSize = value;
        }
    }
    
    public float animationDuration
    {
        get => _animationDuration;
    }
}