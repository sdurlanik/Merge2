using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public class MergeHandler : DropHandlerBase
    {
        public override bool Handle(Item sourceItem, Cell targetCell)
        {
            var targetItem = targetCell.OccupiedItem;
            if (ItemActionService.CanMerge(sourceItem, targetItem, out var resultSO))
            {
                ItemActionService.Merge(sourceItem, targetItem, resultSO, targetCell);
                return true;
            }
            
            return nextHandler?.Handle(sourceItem, targetCell) ?? false;
        }
    }
}