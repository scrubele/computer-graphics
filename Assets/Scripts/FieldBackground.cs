using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBackground : MonoBehaviour
{
    [SerializeField]
    public GameConfig _config;
    private IconfigProvider _configProvider;

    public Cell BackgroundCellPrefab;
    
    public GameConfig Config => _config;

    public void Awake(){
        _configProvider = GetComponentInParent<IconfigProvider>();
        for (int x=0; x<Config.FieldSize; x++){
            for (int y=0; y< Config.FieldSize; y++ ){
                var cell = Instantiate(BackgroundCellPrefab, transform);
                cell.Position = new Point(x,y);
            }
        }
    }
   }
