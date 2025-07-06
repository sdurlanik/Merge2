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
        
        public void OnEnter(Cell cell)
        {
            cell.UpdateVisuals(Color.white, true);
        }

        public void OnExit(Cell cell) { }
        
        public bool OnItemDropped(Item sourceItem, Cell targetCell)
        {
            return _dropHandlerChain.Handle(sourceItem, targetCell);
        }
    }
}