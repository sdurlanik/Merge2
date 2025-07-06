using Sdurlanik.Merge2.Services.DropHandlers;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public class CellUnlockedState : ICellState
    {
        private readonly DropHandlerBase _dropHandlerChain;

        public CellUnlockedState()
        {
            _dropHandlerChain = DropHandlerChainFactory.CreateDefaultChain();
        }

        public CellState StateType => CellState.Unlocked;
        public void OnEnter(Cell cell)
        {
            cell.ShowUnlockedVisuals();
        }

        public void OnExit(Cell cell) { }
        
        public bool OnItemDropped(Item sourceItem, Cell targetCell)
        {
            return _dropHandlerChain.Handle(sourceItem, targetCell);
        }
    }
}