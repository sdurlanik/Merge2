// Konum: Sdurlanik.Merge2/Scripts/UI/OrderUIManager.cs

using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Events;
using UnityEngine;

namespace Sdurlanik.Merge2.UI
{
    public class OrderUIManager : MonoBehaviour
    {
        [SerializeField] private Transform _ordersContainer;
        [SerializeField] private OrderUIEntry _orderEntryPrefab;
        [SerializeField] private int _maxOrdersToDisplay = 3;

        private readonly List<OrderUIEntry> _uiEntries = new();

        private void Awake()
        {
            for (int i = 0; i < _maxOrdersToDisplay; i++)
            {
                var entry = Instantiate(_orderEntryPrefab, _ordersContainer);
                entry.gameObject.SetActive(false);
                _uiEntries.Add(entry);
            }
        }

        private void OnEnable()
        {
            EventBus<ActiveOrdersUpdatedEvent>.OnEvent += HandleActiveOrdersUpdated;
        }

        private void OnDisable()
        {
            EventBus<ActiveOrdersUpdatedEvent>.OnEvent -= HandleActiveOrdersUpdated;
        }

        private void HandleActiveOrdersUpdated(ActiveOrdersUpdatedEvent e)
        {
            var newOrders = e.ActiveOrders.ToList();

            var survivingEntries = new List<OrderUIEntry>();
            foreach (var newOrder in newOrders)
            {
                var existingEntry = _uiEntries.FirstOrDefault(ui => ui.CurrentOrder == newOrder);
                if (existingEntry != null)
                {
                    existingEntry.UpdateDisplay(newOrder);
                    survivingEntries.Add(existingEntry);
                }
            }

            var entryToAnimateOut = _uiEntries.FirstOrDefault(ui => 
                ui.gameObject.activeSelf && !newOrders.Contains(ui.CurrentOrder));
            
            var addedOrder = newOrders.FirstOrDefault(o => _uiEntries.All(ui => ui.CurrentOrder != o));

            if (entryToAnimateOut != null)
            {
                entryToAnimateOut.AnimateOut(() =>
                {
                    if (addedOrder != null)
                    {
                        entryToAnimateOut.InitializeAndAnimateIn(addedOrder, 0.1f);
                    }
                });
            }
            else if (addedOrder != null)
            {
                var availableEntry = _uiEntries.FirstOrDefault(ui => !ui.gameObject.activeSelf);
                if (availableEntry != null)
                {
                    availableEntry.InitializeAndAnimateIn(addedOrder, 0.1f);
                }
            }
        }
    }
}