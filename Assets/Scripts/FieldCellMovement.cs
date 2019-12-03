using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FieldCellMovement
    {
        public FieldCell OriginalFieldCell { get; set; }
        public FieldCell NewFieldCell { get; set; }

        public int newRow { get; set; }
        public int newColumn { get; set; }


        public FieldCellMovement(int newRow, int newColumn, FieldCell newFieldCell, FieldCell originalFieldCell)
        {
            this.newRow = newRow;
            this.newColumn = newColumn;
            NewFieldCell = newFieldCell;
            OriginalFieldCell = originalFieldCell;
        }

    }



