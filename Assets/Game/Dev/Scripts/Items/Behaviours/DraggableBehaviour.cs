using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.Items.Behaviours
{
   public class DraggableBehavior
   {
       private readonly InteractionSettingsSO _settings;
       private readonly Item _item;
       private readonly Transform _transform;
       private readonly SpriteRenderer _spriteRenderer;
       private readonly Camera _mainCamera;

       private Transform _originalParent;
       private Vector3 _offset;
    
       private float _originalZ; 
       private string _originalSortingLayerName;
       private int _originalSortingOrder;
   
       public DraggableBehavior(Item item)
       {
           _item = item;
           _settings = item.InteractionSettings;
           _transform = item.transform;
           _spriteRenderer = item.GetComponent<SpriteRenderer>();
           _mainCamera = Camera.main;
       }
   
       public void OnBeginDrag()
       {
           _transform.DOKill();
           _originalParent = _transform.parent;
           _transform.SetParent(null); 
           
           _originalZ = _transform.position.z;
           
           var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
           _offset = _transform.position - mouseWorldPos;
           
           _originalSortingLayerName = _spriteRenderer.sortingLayerName;
           _originalSortingOrder = _spriteRenderer.sortingOrder;
           
           _spriteRenderer.sortingLayerName = _settings.DraggingSortingLayerName;
           _spriteRenderer.sortingOrder = 100;
       }
   
       public void OnDrag(Vector2 position)
       {
           var newPosition = new Vector3(position.x, position.y, 0) + _offset;
           newPosition.z = _originalZ;
           _transform.position = newPosition;
       }
   
       public void OnEndDrag(DropZone successDropZone)
       {
           _spriteRenderer.sortingLayerName = _originalSortingLayerName;
           _spriteRenderer.sortingOrder = _originalSortingOrder;
        
           var actionWasTaken = false;

           if (successDropZone != null)
           {
               actionWasTaken = successDropZone.HandleDrop(_item);
           }

           if (!actionWasTaken)
           {
               _transform.SetParent(_originalParent);
               ServiceLocator.Get<AnimationManager>().PlayItemSnapBackAnimation(_transform);
           }
       }
   }
}