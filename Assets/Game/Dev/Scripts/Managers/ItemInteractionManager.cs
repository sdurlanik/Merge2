using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
   public class ItemInteractionManager : MonoBehaviour
   {
       [SerializeField] private InteractionSettingsSO _settings;
       private Camera _mainCamera;
       
       private IInteractable _clickedInteractable;
       private DropZone _lastDetectedZone;
       
       private Vector2 _startMousePosition;
       private bool _isDragging;
       private Item _lastMergeableTarget; 
       
       private readonly Collider2D[] _dropZoneResults = new Collider2D[1];
       private InputDragPerformedEvent _reusableDragEvent;

       private void Awake()
       {
           _mainCamera = Camera.main;
       }
   
       private void Update()
       {
           var mousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition);

           if (Input.GetMouseButtonDown(0))
           {
               IInteractable interactable = GetInteractableAt(mousePosition);

               if (interactable is Item clickedItem && clickedItem.CurrentCell.CurrentState == Cell.Unlocked)
               {
                   _clickedInteractable = clickedItem;
                   _startMousePosition = mousePosition;
               }
           }
    
           if (Input.GetMouseButton(0))
           {
               if (_clickedInteractable != null && !_isDragging)
               {
                   if (Vector2.Distance(mousePosition, _startMousePosition) > _settings.DragThreshold)
                   {
                       if (_clickedInteractable is IDraggable)
                       {
                           _isDragging = true;
                           EventBus<ItemDragBeganEvent>.Publish(new ItemDragBeganEvent { DraggedItem = (Item)_clickedInteractable });
                       }
                   }
               }
        
               if (_isDragging)
               {
                   _reusableDragEvent.MousePosition = mousePosition; 
                   EventBus<InputDragPerformedEvent>.Publish(_reusableDragEvent);
                   DetectDropZone((_clickedInteractable as MonoBehaviour).transform.position);
            
               }
           }

           if (Input.GetMouseButtonUp(0))
           {
               if (_isDragging)
               {
                   EventBus<ItemDragEndedEvent>.Publish(new ItemDragEndedEvent { TargetZone = _lastDetectedZone });
               }
               else if (_clickedInteractable != null)
               {
                   if (_clickedInteractable is ITappable)
                   {
                       EventBus<ItemTappedEvent>.Publish(new ItemTappedEvent { TappedItem = (Item)_clickedInteractable });
                   }
               }
        
               ResetInteractionState();
           }
       }
       
       private void ResetInteractionState()
       {
           _isDragging = false;
           _clickedInteractable = null;
           _lastDetectedZone = null;
           
           if (_lastMergeableTarget != null)
           {
               ServiceLocator.Get<AnimationManager>().PlayMergePreviewShrinkAnimation(_lastMergeableTarget.transform);
               _lastMergeableTarget = null;
           }
       }
       
       private void HandleMergePreview()
       {
           var draggedItem = _clickedInteractable as Item;
           var targetItem = _lastDetectedZone?.GetComponent<Cell>()?.OccupiedItem;

           if (draggedItem == null || targetItem == null)
           {
               if (_lastMergeableTarget != null)
               {
                   ServiceLocator.Get<AnimationManager>().PlayMergePreviewShrinkAnimation(_lastMergeableTarget.transform);
                   _lastMergeableTarget = null;
               }
               return;
           }

           var canMerge = ItemActionService.CanMerge(draggedItem, targetItem, out _);

           if (canMerge && draggedItem != targetItem && targetItem != _lastMergeableTarget)
           {
               if(_lastMergeableTarget != null)
                   ServiceLocator.Get<AnimationManager>().PlayMergePreviewShrinkAnimation(_lastMergeableTarget.transform);
        
               ServiceLocator.Get<AnimationManager>().PlayMergePreviewGrowAnimation(targetItem.transform);
               _lastMergeableTarget = targetItem;
           }
           else if ((!canMerge || draggedItem == targetItem) && _lastMergeableTarget != null)
           {
               ServiceLocator.Get<AnimationManager>().PlayMergePreviewShrinkAnimation(_lastMergeableTarget.transform);
               _lastMergeableTarget = null;
           }
       }
       
       private IInteractable GetInteractableAt(Vector2 position)
       {
           var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, _settings.InteractableLayer);
           return hit.collider?.GetComponent<IInteractable>();
       }
   
       private void DetectDropZone(Vector2 position)
       {
           DropZone detectedZone = null;
           var hitCount = Physics2D.OverlapCircleNonAlloc(position, _settings.DropDetectionRadius, _dropZoneResults, _settings.DropZoneLayer);
           if (hitCount > 0)
           {
               detectedZone = _dropZoneResults[0].GetComponent<DropZone>();
           }
           if (detectedZone != _lastDetectedZone)
           {
                _lastDetectedZone?.NotifyHoverExit();
                detectedZone?.NotifyHoverEnter();
               _lastDetectedZone = detectedZone;
           }
       }
   }
}