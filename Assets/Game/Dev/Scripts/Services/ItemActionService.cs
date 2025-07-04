using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;

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
        }

        public static void Move(Item itemMono, Cell targetCell)
        {
            var origin = itemMono.CurrentCell;
            if (origin == null || !targetCell.IsEmpty) return;

            origin.ClearItem();
            targetCell.PlaceItem(itemMono);
        }

        public static void Merge(Item firstItem, Item secondItem, ItemSO resultSO, Cell targetCell)
        {
            var animationsCompleted = 0;

            var cellA = firstItem.CurrentCell;
            var cellB = secondItem.CurrentCell;
        
            cellA.ClearItem();
            cellB.ClearItem();

            firstItem.AnimateMerge(targetCell.transform.position, () =>
            {
               ServiceLocator.Get<ObjectPooler>().ReturnObjectToPool(firstItem.ItemDataSO.ItemPrefab.name, firstItem.gameObject);
                OnOneAnimationComplete();
            });
        
            secondItem.AnimateMerge(targetCell.transform.position, () =>
            {
                ServiceLocator.Get<ObjectPooler>().ReturnObjectToPool(secondItem.ItemDataSO.ItemPrefab.name, secondItem.gameObject);
                OnOneAnimationComplete();
            });
            return;

            void OnOneAnimationComplete()
            {
                animationsCompleted++;
                if (animationsCompleted == 2)
                {
                    ItemFactory.Create(resultSO, targetCell);
                }
            }
        }
    }

}