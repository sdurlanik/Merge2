using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Managers
{
   public class ItemInteractionManager : MonoBehaviour
   {
       [SerializeField] private InteractionSettingsSO _settings;
       private Camera _mainCamera;
       private IInteractable _currentTarget;
       private IDraggable _draggingTarget;
       private DropZone _lastDetectedZone;
       
       private Vector2 _startMousePosition;
       private bool _isPotentialDrag;
       
       private readonly Collider2D[] _dropZoneResults = new Collider2D[1];
       private void Awake()
       {
           _mainCamera = Camera.main;
       }
   
       private void Update()
       {
           var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
   
           if (Input.GetMouseButtonDown(0))
           {
               _currentTarget = GetInteractableAt(mousePosition);
               if (_currentTarget != null)
               {
                   _isPotentialDrag = true;
                   _startMousePosition = mousePosition;
               }
           }
           
           if (Input.GetMouseButton(0))
           {
               if (_isPotentialDrag && _draggingTarget == null)
               {
                   if (Vector2.Distance(mousePosition, _startMousePosition) > _settings.DragThreshold)
                   {
                       _isPotentialDrag = false;
                       if (_currentTarget is IDraggable draggable)
                       {
                           _draggingTarget = draggable;
                           _draggingTarget.OnBeginDrag();
                       }
                   }
               }
               
               if (_draggingTarget != null)
               {
                   _draggingTarget.OnDrag(mousePosition);
                   DetectDropZone(((MonoBehaviour)_draggingTarget).transform.position);
               }
           }
   
           if (Input.GetMouseButtonUp(0))
           {
               if (_draggingTarget != null)
               {
                   _draggingTarget.OnEndDrag(_lastDetectedZone);
               }
               else if (_isPotentialDrag)
               {
                   if (_currentTarget is ITappable tappable)
                   {
                       tappable.OnTap();
                   }
               }
               
               ResetInteractionState();
           }
       }
       
       private void ResetInteractionState()
       {
           _isPotentialDrag = false;
           _currentTarget = null;
           _draggingTarget = null;
           _lastDetectedZone = null;
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