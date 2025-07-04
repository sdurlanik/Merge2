using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.Events
{
    public struct ItemTappedEvent : IEvent
    {
        public Item TappedItem;
    }

    public struct ItemDragBeganEvent : IEvent
    {
        public Item DraggedItem;
    }

    public struct InputDragPerformedEvent : IEvent
    {
        public Vector2 MousePosition;
    }

    public struct ItemDragEndedEvent : IEvent
    {
        public DropZone TargetZone;
    }
}