using Sdurlanik.Merge2.Services;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public class CellLockedRevealedState : ICellState
    {
        public CellState StateType => CellState.LockedRevealed;

        public void OnEnter(Cell cell)
        {
            cell.ShowLockedRevealedVisuals();
        }
        public void OnExit(Cell cell) { }

        public bool OnItemDropped(Item sourceItem, Cell targetCell)
        {
            if (targetCell.IsEmpty) return false;
            
            if (ItemActionService.CanMerge(sourceItem, targetCell.OccupiedItem, out var resultSO))
            {
                ItemActionService.Merge(sourceItem, targetCell.OccupiedItem, resultSO, targetCell, () =>
                {
                    targetCell.TransitionTo(Cell.Unlocked);
                });
                return true;
            }
            return false;
        }
    }
}