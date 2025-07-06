using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Services;
using Sdurlanik.Merge2.Services.DropHandlers;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{

    [RequireComponent(typeof(Collider2D),(typeof(Cell)))]
    public class DropZone : MonoBehaviour
    {
        private Cell _targetCell;
        private DropHandlerBase _rootHandler;

        private void Awake()
        {
            _targetCell = GetComponent<Cell>();
            _rootHandler = DropHandlerChainFactory.CreateDefaultChain();
        }

        public bool HandleDrop(Item sourceItem)
        {
            return _targetCell.OnItemDropped(sourceItem);
        }

        public void NotifyHoverEnter()
        {
            //TODO: i maybe add a visual effect here as the original game has
        }

        public void NotifyHoverExit()
        {
            
        }
    }
}