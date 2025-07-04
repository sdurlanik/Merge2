using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{
    public class Cell : MonoBehaviour
    {
        public Item OccupiedItem { get; private set; }
        public bool IsEmpty => OccupiedItem == null;
        
        public Vector2Int GridPos { get; private set; }
        
        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;
        }
        public void PlaceItem(Item item)
        {
            OccupiedItem = item;
            item.transform.SetParent(transform);
            item.AnimateMoveTo(transform.position); 
            item.SetCurrentCell(this);
        }

        public void ClearItem()
        {
            OccupiedItem = null;
        }

        public void DestroyItem()
        {
            if (OccupiedItem == null) return;
            
            var poolTag = OccupiedItem.ItemDataSO.ItemPrefab.name;
            ServiceLocator.Get<ObjectPooler>().ReturnObjectToPool(poolTag, OccupiedItem.gameObject);
                
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
            OccupiedItem = null;
        }
        
        public void ConsumeItemWithAnimation()
        {
            if (IsEmpty) return;

            OccupiedItem.AnimateConsumption();
            
            ClearItem();
            
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }
    }
}