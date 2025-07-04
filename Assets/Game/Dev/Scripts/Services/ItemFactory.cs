using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class ItemFactory
    {
            public static Item Create(ItemSO so, Cell targetCell)
            {
                var newItemObject = ServiceLocator.Get<ObjectPooler>().GetObjectFromPool(so.ItemPrefab.name);
                if (newItemObject == null) return null;

                var newItem = newItemObject.GetComponent<Item>();
            
                newItem.Init(so);
                newItemObject.transform.position = targetCell.transform.position;
                targetCell.PlaceItem(newItem);
                newItemObject.SetActive(true);
                
                newItem.AnimateAppear();
                EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());

                return newItem;
            }
    }
}