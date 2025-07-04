using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items.Behaviours;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Events;
using UnityEngine;

namespace Sdurlanik.Merge2.Items
{

    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Item : MonoBehaviour
    {
        public InteractionSettingsSO InteractionSettings => _interactionSettings;
        public ItemSO ItemDataSO { get; private set; }
        public Cell CurrentCell { get; private set; }
        
        [SerializeField] private InteractionSettingsSO _interactionSettings;
        private ProducerBehavior _producerBehavior;
        private DraggableBehavior _draggableBehavior;
        private bool _isBeingDragged;

        private void OnEnable()
        {
            EventBus<ItemTappedEvent>.OnEvent += HandleTap;
            EventBus<ItemDragBeganEvent>.OnEvent += HandleDragBegan;
            EventBus<InputDragPerformedEvent>.OnEvent += HandleDragPerformed;
            EventBus<ItemDragEndedEvent>.OnEvent += HandleDragEnded;
        }
        
        private void OnDisable()
        {
            EventBus<ItemTappedEvent>.OnEvent -= HandleTap;
            EventBus<ItemDragBeganEvent>.OnEvent -= HandleDragBegan;
            EventBus<InputDragPerformedEvent>.OnEvent -= HandleDragPerformed;
            EventBus<ItemDragEndedEvent>.OnEvent -= HandleDragEnded;
        }
        
        private void HandleTap(ItemTappedEvent e)
        {
            if (e.TappedItem != this) return;
            
            _producerBehavior?.OnTap();
        }

        private void HandleDragBegan(ItemDragBeganEvent e)
        {
            if (e.DraggedItem != this) return;
            
            _isBeingDragged = true;
            _draggableBehavior.OnBeginDrag();
        }

        private void HandleDragPerformed(InputDragPerformedEvent e)
        {
            if (!_isBeingDragged) return;
            
            _draggableBehavior.OnDrag(e.MousePosition);
        }

        private void HandleDragEnded(ItemDragEndedEvent e)
        {
            if (!_isBeingDragged) return;
            
            _isBeingDragged = false;
            _draggableBehavior.OnEndDrag(e.TargetZone);
        }

        public void Init(ItemSO so)
        {
            ItemDataSO = so;
            GetComponent<SpriteRenderer>().sprite = so.Icon;

            if (ItemDataSO is GeneratorSO { CanProduce: true } generatorData)
            {
                _producerBehavior = new ProducerBehavior(this, generatorData);
            }
            _draggableBehavior = new DraggableBehavior(this);
        }
    
        public void SetCurrentCell(Cell cell)
        {
            CurrentCell = cell;
        }
    }
}