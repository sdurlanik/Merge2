using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{
    public class Cell : MonoBehaviour
    {
        public Item OccupiedItem { get; private set; }
        public bool IsEmpty => OccupiedItem == null;
        public Vector2Int GridPos { get; private set; }
        
        [SerializeField] private GameObject _orderHighlight;

        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;
        }
        public void PlaceItem(Item item)
        {
            OccupiedItem = item;
            item.transform.SetParent(transform);
            ServiceLocator.Get<AnimationManager>().PlayItemMoveAnimation(item.transform, transform.position);
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

            ServiceLocator.Get<AnimationManager>().PlayItemConsumptionAnimation(OccupiedItem);
            
            ClearItem();
            
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }
        
        public void SetHighlight(bool isActive)
        {
            if (_orderHighlight != null)
            {
                _orderHighlight.SetActive(isActive);
            }
        }
    }
}