using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;

namespace Sdurlanik.Merge2.Events
{
    public struct BoardStateChangedEvent : IEvent { }
    
    public struct ActiveOrdersUpdatedEvent : IEvent
    {
        public IReadOnlyList<Order> ActiveOrders;
    }
}