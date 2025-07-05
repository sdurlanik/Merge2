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
        [Header("UI References")]
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

            var oldOrders = _uiEntries
                .Where(entry => entry.CurrentOrder != null && entry.gameObject.activeSelf)
                .Select(entry => entry.CurrentOrder)
                .ToList();

            var addedOrder = newOrders.Except(oldOrders).FirstOrDefault();
            
            var removedOrder = oldOrders.Except(newOrders).FirstOrDefault();

            for (var i = 0; i < _uiEntries.Count; i++)
            {
                var uiEntry = _uiEntries[i];
                if (uiEntry.CurrentOrder != null && uiEntry.CurrentOrder != removedOrder)
                {
                    uiEntry.UpdateDisplay(uiEntry.CurrentOrder);
                }
            }

            if (removedOrder != null && addedOrder != null)
            {
                var entryToReplace = _uiEntries.FirstOrDefault(entry => entry.CurrentOrder == removedOrder);
                if (entryToReplace != null)
                {
                    entryToReplace.AnimateOutAndRepopulate(addedOrder);
                }
            }
            else if (oldOrders.Count == 0 && newOrders.Count > 0)
            {
                for (int i = 0; i < newOrders.Count; i++)
                {
                    if (i < _uiEntries.Count)
                    {
                        _uiEntries[i].InitializeAndAnimateIn(newOrders[i], i * 0.1f);
                    }
                }
            }
        }
    }
}