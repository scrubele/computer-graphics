using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private IconfigProvider _configProvider;
    private Point _position;
    // Start is called before the first frame update
    private void Awake(){
        _configProvider = GetComponentInParent<IconfigProvider>();
    }

    public Point Position{
        get => _position;
        set{
            if (_position !=value){
                _position = value;
                transform.localPosition = value * _configProvider.Config.CellSize;
            }
        }
    }
    
}