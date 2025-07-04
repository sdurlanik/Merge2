using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public class SwapHandler : DropHandlerBase
    {
        public override bool Handle(Item sourceItem, Cell targetCell)
        {
            var targetItem = targetCell.OccupiedItem;
            if(targetItem != null)
            {
                ItemActionService.Swap(sourceItem, targetItem);
                return true;
            }
            // There is no next handler because swap is a terminal operation
            return false;
        }
    }
}