using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.GridSystem.CellStates
{
    public enum CellState
    {
        LockedHidden,
        LockedRevealed,
        Unlocked
    }
    public interface ICellState
    {
        CellState StateType { get; }
        void OnEnter(Cell cell);
        
        void OnExit(Cell cell);

        bool OnItemDropped(Item sourceItem, Cell targetCell);
    }
}