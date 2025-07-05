using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Managers;

namespace Sdurlanik.Merge2.Services
{
    public static class ItemActionService
    {
        public static bool CanMerge(Item a, Item b, out ItemSO nextSO)
        {
            nextSO = null;
            if (a == null || b == null) return false;

            if (a.ItemDataSO.Family == b.ItemDataSO.Family && a.ItemDataSO.Level == b.ItemDataSO.Level)
            {
                nextSO = ServiceLocator.Get<DataBank>().GetSO(a.ItemDataSO.Family, a.ItemDataSO.Level + 1);
                return nextSO != null;
            }
            return false;
        }
        
        public static void Swap(Item a, Item b)
        {
            var cellA = a.CurrentCell;
            var cellB = b.CurrentCell;
            if (cellA == null || cellB == null) return;

            cellA.ClearItem();
            cellB.ClearItem();
            cellA.PlaceItem(b);
            cellB.PlaceItem(a);
            
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }

        public static void Move(Item itemMono, Cell targetCell)
        {
            var origin = itemMono.CurrentCell;
            if (origin == null || !targetCell.IsEmpty) return;

            origin.ClearItem();
            targetCell.PlaceItem(itemMono);
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }

        public static void Merge(Item a, Item b, ItemSO resultSO, Cell targetCell)
        {
            var juiceManager = ServiceLocator.Get<AnimationManager>();
            var objectPooler = ServiceLocator.Get<ObjectPooler>();

            Action onMergeAnimationComplete = () =>
            {
                objectPooler.ReturnObjectToPool(a.ItemDataSO.ItemPrefab.name, a.gameObject);
                objectPooler.ReturnObjectToPool(b.ItemDataSO.ItemPrefab.name, b.gameObject);
                ItemFactory.Create(resultSO, targetCell);
            };
        
            a.CurrentCell.ClearItem();
            b.CurrentCell.ClearItem();
            juiceManager.PlayTwoItemMergeAnimation(a, b, targetCell.transform.position, onMergeAnimationComplete);
        }
    }

}