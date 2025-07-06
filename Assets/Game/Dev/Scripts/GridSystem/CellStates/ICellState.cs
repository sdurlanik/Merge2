using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public interface ICellState
    {
        void OnEnter(Cell cell);
        
        void OnExit(Cell cell);

        bool OnItemDropped(Item sourceItem, Cell targetCell);
    }
}