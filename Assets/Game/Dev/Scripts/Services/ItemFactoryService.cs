using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class ItemFactoryService
    {
        public static Item Create(ItemSO so, Cell targetCell, bool animate = true, bool publishEvents = true)
        {
            var newItemObject = ServiceLocator.Get<ObjectPooler>().GetObjectFromPool(so.ItemPrefab.name);
            if (newItemObject == null) return null;

            var newItem = newItemObject.GetComponent<Item>();
            newItem.Init(so);
            targetCell.PlaceItem(newItem);
            

            if (animate)
            {
                ServiceLocator.Get<AnimationManager>().PlayItemAppearAnimation(newItem.transform);
            }
            else
            {
                newItem.gameObject.SetActive(true);
                newItem.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (publishEvents)
            {
                EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
            }

            return newItem;
        }
    }
}