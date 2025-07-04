using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events; 
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
   public class ItemInteractionManager : MonoBehaviour
   {
       [SerializeField] private InteractionSettingsSO _settings;
       private Camera _mainCamera;
       
       private Item _clickedItem;
       private DropZone _lastDetectedZone;
       
       private Vector2 _startMousePosition;
       private bool _isDragging;
       
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
               _clickedItem = GetInteractableItemAt(mousePosition);
               if (_clickedItem != null)
               {
                   _startMousePosition = mousePosition;
               }
           }
           
           if (Input.GetMouseButton(0))
           {
               if (_clickedItem != null && !_isDragging)
               {
                   if (Vector2.Distance(mousePosition, _startMousePosition) > _settings.DragThreshold)
                   {
                       _isDragging = true;
                       EventBus<ItemDragBeganEvent>.Publish(new ItemDragBeganEvent { DraggedItem = _clickedItem });
                   }
               }
               
               if (_isDragging)
               {
                   //InputDragPerformedEvent is frequently published, so we reuse the same instance to avoid any stuttering
                   _reusableDragEvent.MousePosition = mousePosition; 
                   EventBus<InputDragPerformedEvent>.Publish(_reusableDragEvent);
                   
                   DetectDropZone(_clickedItem.transform.position);
               }
           }
   
           if (Input.GetMouseButtonUp(0))
           {
               if (_isDragging)
               {
                   EventBus<ItemDragEndedEvent>.Publish(new ItemDragEndedEvent { TargetZone = _lastDetectedZone });
               }
               else if (_clickedItem != null)
               {
                   EventBus<ItemTappedEvent>.Publish(new ItemTappedEvent { TappedItem = _clickedItem });
               }
               
               ResetInteractionState();
           }
       }
       
       private void ResetInteractionState()
       {
           _isDragging = false;
           _clickedItem = null;
           _lastDetectedZone = null;
       }
       
       private Item GetInteractableItemAt(Vector2 position)
       {
           var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, _settings.InteractableLayer);
           return hit.collider?.GetComponent<Item>();
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