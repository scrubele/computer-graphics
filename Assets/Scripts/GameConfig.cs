using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "Config", menuName = "Config/Create Game Config", order = 1)]
    public class GameConfig : ScriptableObject {
        public int FieldSize = 4;
        public float CellSize = 3;

        
    }
