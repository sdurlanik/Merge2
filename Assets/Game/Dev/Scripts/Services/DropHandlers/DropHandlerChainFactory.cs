using System.Collections.Generic;

namespace Sdurlanik.Merge2.Services.DropHandlers
{
    public static class DropHandlerChainFactory
    {
        public static DropHandlerBase CreateDefaultChain()
        {
            var handlers = new List<DropHandlerBase>
            {
                new MoveHandler(),
                new MergeHandler(),
                new SwapHandler()
            };

            for (int i = 0; i < handlers.Count - 1; i++)
            {
                handlers[i].SetNext(handlers[i + 1]);
            }

            return handlers[0];
        }
    }
}