using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public class CellLockedHiddenState : ICellState
    {
        public void OnEnter(Cell cell)
        {
            cell.UpdateVisuals(new Color(0.1f, 0.1f, 0.1f, 1f), false);
            cell.SetHighlight(false);
        }

        public void OnExit(Cell cell) { }
        
        public bool OnItemDropped(Item sourceItem, Cell targetCell) => false;
    }
}