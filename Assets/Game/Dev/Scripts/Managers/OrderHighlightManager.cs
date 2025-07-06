// Konum: Sdurlanik.Merge2/Scripts/Managers/OrderHighlightManager.cs (YENİ DOSYA)

using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
    public class OrderHighlightManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus<BoardStateChangedEvent>.OnEvent += HandleBoardStateChanged;
            EventBus<ActiveOrdersUpdatedEvent>.OnEvent += HandleOrdersUpdated;
        }

        private void OnDisable()
        {
            EventBus<BoardStateChangedEvent>.OnEvent += HandleBoardStateChanged;
            EventBus<ActiveOrdersUpdatedEvent>.OnEvent -= HandleOrdersUpdated;
        }

        private void HandleBoardStateChanged(BoardStateChangedEvent e)
        {
            UpdateAllCellHighlights();
        }

        private void HandleOrdersUpdated(ActiveOrdersUpdatedEvent e)
        {
            UpdateAllCellHighlights();
        }


        private void UpdateAllCellHighlights()
        {
            var orderManager = ServiceLocator.Get<OrderManager>();
            var gridManager = ServiceLocator.Get<GridManager>();

            var requiredItemsForCompletableOrders = new HashSet<ItemSO>();

            foreach (var order in orderManager.ActiveOrders)
            {
                if (order.Status == OrderStatus.ReadyToComplete)
                {
                    foreach (var requirement in order.Requirements)
                    {
                        requiredItemsForCompletableOrders.Add(requirement.RequiredItem);
                    }
                }
            }

            var allCells = gridManager.GetAllCells();
            foreach (var cell in allCells)
            {
                var shouldHighlight = 
                    !cell.IsEmpty && 
                    cell.CurrentState == Cell.Unlocked &&
                    requiredItemsForCompletableOrders.Contains(cell.OccupiedItem.ItemDataSO);
                
                cell.SetHighlight(shouldHighlight);
            }
        }
    }
}