using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public class MoveHandler : DropHandlerBase
    {
        public override bool Handle(Item sourceItem, Cell targetCell)
        {
            if (targetCell.IsEmpty)
            {
                ItemActionService.Move(sourceItem, targetCell);
                return true;
            }
            
            return nextHandler?.Handle(sourceItem, targetCell) ?? false;
        }
    }
}