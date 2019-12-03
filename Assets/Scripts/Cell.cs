using UnityEngine;

public class Cell : MonoBehaviour
{
    private IConfigProvider _configProvider;
    private Point _position;

    public Point position
    {
        get => _position;
        set
        {
            if (_position == value) return;
            _position = value;
            transform.localPosition = value * _configProvider.Config.cellSize;
        }
    }


    private void Awake()
    {
        _configProvider = GetComponentInParent<IConfigProvider>();
    }
}