using UnityEngine;
using UnityEngine.Serialization;

public class FieldBackground : MonoBehaviour
{
    [FormerlySerializedAs("_config")] [SerializeField] public GameConfig config;

    private IConfigProvider _configProvider;

    [FormerlySerializedAs("BackgroundCellPrefab")] public Cell backgroundCellPrefab;
    private const int Offset = 0;

    private GameConfig Config => config;

    public void Awake()
    {
        _configProvider = GetComponentInParent<IConfigProvider>();
        for (var x = 0; x < Config.fieldSize; x++)
        for (var y = Offset; y < Config.fieldSize + Offset; y++)
        {
            var cell = Instantiate(backgroundCellPrefab, transform);
            cell.position = new Point(x, y);
        }
    }
}