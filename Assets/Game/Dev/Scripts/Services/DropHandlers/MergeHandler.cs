using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public class MergeHandler : DropHandlerBase
    {
        public override bool Handle(Item sourceItem, Cell targetCell)
        {
            var targetItem = targetCell.OccupiedItem;

            if (targetItem == null || sourceItem == targetItem)
            {
                return nextHandler?.Handle(sourceItem, targetCell) ?? false;
            }

            if (ItemActionService.CanMerge(sourceItem, targetItem, out var resultSO))
            {
                ItemActionService.Merge(sourceItem, targetItem, resultSO, targetCell, () =>
                {
                    targetCell.TransitionTo(Cell.Unlocked);
                });
                return true;
            }
            
            return nextHandler?.Handle(sourceItem, targetCell) ?? false;
        }
    }
}