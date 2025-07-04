using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{

    [RequireComponent(typeof(Collider2D),(typeof(Cell)))]
    public class DropZone : MonoBehaviour
    {
        private Cell _targetCell;

        private void Awake()
        {
            _targetCell = GetComponent<Cell>();
        }

        public bool HandleDrop(Item sourceItem) //TODO i will probably change this method with using chain of responsibility pattern
        {
            var sourceCell = sourceItem.CurrentCell;
            
            if (sourceCell == _targetCell) return false;
        
            var targetItem = _targetCell.OccupiedItem;
        
            if (targetItem == null) //Empty cell
            {
                ItemActionService.Move(sourceItem, _targetCell);
                return true;
            }
            else if (ItemActionService.CanMerge(sourceItem, targetItem, out var resultSO))
            {
                ItemActionService.Merge(sourceItem, targetItem, resultSO, _targetCell);
                return true;
            }
            else
            {
                ItemActionService.Swap(sourceItem, targetItem);
                return true;
            }
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