using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Items
{
    public interface IDraggable : IInteractable
    {
        void OnBeginDrag();
        void OnDrag(Vector2 position);
        void OnEndDrag(DropZone successDropZone);
    }
}