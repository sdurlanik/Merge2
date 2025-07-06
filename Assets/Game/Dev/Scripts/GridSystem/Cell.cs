using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem.CellStates;
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
        public ICellState CurrentState => _currentState;
        
        [SerializeField] private GameObject _orderHighlight;
        [SerializeField] private SpriteRenderer _background;
        
        private ICellState _currentState;
        public static readonly ICellState LockedHidden = new CellLockedHiddenState();
        public static readonly ICellState LockedRevealed = new CellLockedRevealedState();
        public static readonly ICellState Unlocked = new CellUnlockedState();
        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;
            TransitionTo(LockedHidden);
        }
        public void TransitionTo(ICellState newState)
        {
            _currentState?.OnExit(this);
            _currentState = newState;
            _currentState.OnEnter(this);
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
        
        public bool OnItemDropped(Item sourceItem)
        {
            return _currentState.OnItemDropped(sourceItem, this);
        }
        
        public void UpdateVisuals(Color backgroundColor, bool showItem)
        {
            if (_background != null)
            {
                _background.color = backgroundColor;
            }

            if (!IsEmpty)
            {
                OccupiedItem.gameObject.SetActive(showItem);
            }
        }
    }
}