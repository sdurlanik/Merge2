using System;
using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem.CellStates;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.GridSystem
{
    public class Cell : MonoBehaviour
    {
        public Item OccupiedItem { get; private set; }
        public bool IsEmpty => OccupiedItem == null;
        public Vector2Int GridPos { get; private set; }
        public ICellState CurrentState => _currentState;
        
        [SerializeField] private GameObject _orderHighlight;
        [SerializeField] private SpriteRenderer _lockedOverlay;
        [SerializeField] private SpriteRenderer _lockedRevealedOverlay;
        
        private ICellState _currentState;
        public static readonly ICellState LockedHidden = new CellLockedHiddenState();
        public static readonly ICellState LockedRevealed = new CellLockedRevealedState();
        public static readonly ICellState Unlocked = new CellUnlockedState();
        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;
        }
        public void TransitionTo(ICellState newState)
        {
            _currentState?.OnExit(this);
            _currentState = newState;
            _currentState.OnEnter(this);
            
            UpdateItemVisibility();
        }
        public void PlaceItem(Item item)
        {
            OccupiedItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;
            item.SetCurrentCell(this);
            
            UpdateItemVisibility();
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

  
        public void ShowUnlockedVisuals(float duration = 0.3f)
        {
            _lockedOverlay.DOFade(0, duration);
            _lockedRevealedOverlay.DOFade(0, duration);
            ShowItem(duration);
        }

        public void ShowLockedHiddenVisuals(float duration = 0.3f)
        {
            _lockedOverlay.DOFade(1, duration);
            _lockedRevealedOverlay.DOFade(0, duration);
            HideItem(duration);
        }

        public void ShowLockedRevealedVisuals(float duration = 0.3f)
        {
            _lockedOverlay.DOFade(0, duration);
            _lockedRevealedOverlay.DOFade(1, duration);
            ShowItem(duration);
        }
        
        private void ShowItem(float duration)
        {
            if (IsEmpty) return;
            OccupiedItem.gameObject.SetActive(true);
            OccupiedItem.GetComponent<SpriteRenderer>()?.DOFade(1, duration);
        }

        private void HideItem(float duration)
        {
            if (IsEmpty) return;
            OccupiedItem.GetComponent<SpriteRenderer>()?.DOFade(0, duration)
                .OnComplete(() => OccupiedItem.gameObject.SetActive(false));
        }
        
        public bool OnItemDropped(Item sourceItem)
        {
            return _currentState.OnItemDropped(sourceItem, this);
        }
        
        private void UpdateItemVisibility()
        {
            if (IsEmpty) return;

            bool shouldBeVisible = CurrentState == Unlocked || CurrentState == LockedRevealed;
            
            OccupiedItem.gameObject.SetActive(shouldBeVisible);
        }
  
    }
}