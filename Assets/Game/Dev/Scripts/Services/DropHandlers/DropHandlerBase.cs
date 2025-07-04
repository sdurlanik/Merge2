using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public abstract class DropHandlerBase
    {
        protected DropHandlerBase nextHandler;

        public DropHandlerBase SetNext(DropHandlerBase nextHandler)
        {
            this.nextHandler = nextHandler;
            return nextHandler;
        }

        public abstract bool Handle(Item sourceItem, Cell targetCell);
    }
}