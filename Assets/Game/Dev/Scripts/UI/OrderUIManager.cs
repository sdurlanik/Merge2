using System.Collections.Generic;
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

        [Header("Prefabs")]
        [SerializeField] private OrderUIEntry _orderEntryPrefab;

        private readonly List<OrderUIEntry> _uiEntries = new();

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
            var activeOrders = e.ActiveOrders;

            while (_uiEntries.Count < activeOrders.Count)
            {
                var newEntry = Instantiate(_orderEntryPrefab, _ordersContainer);
                _uiEntries.Add(newEntry);
            }

            for (int i = 0; i < _uiEntries.Count; i++)
            {
                if (i < activeOrders.Count)
                {
                    _uiEntries[i].Populate(activeOrders[i]);
                    _uiEntries[i].gameObject.SetActive(true);
                }
                else
                {
                    _uiEntries[i].gameObject.SetActive(false);
                }
            }
        }
    }
}