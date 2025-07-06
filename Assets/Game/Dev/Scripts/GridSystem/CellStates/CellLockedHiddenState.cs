using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public class CellLockedHiddenState : ICellState
    {
        public CellState StateType => CellState.LockedHidden;

        public void OnEnter(Cell cell)
        {
            cell.ShowLockedHiddenVisuals();
            cell.SetHighlight(false);
        }
        public void OnExit(Cell cell) { }
        public bool OnItemDropped(Item sourceItem, Cell targetCell) => false;
    }
}