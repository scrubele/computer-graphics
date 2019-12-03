using UnityEngine;

public class GameBackgroundItem : MonoBehaviour
{
    private IConfigProvider _configProvider;
    private Vector3 _position;

    public Vector3 position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                transform.localPosition = value * _configProvider.Config.cellSize;
            }
        }
    }


    private void Awake()
    {
        _configProvider = GetComponentInParent<IConfigProvider>();
    }
}