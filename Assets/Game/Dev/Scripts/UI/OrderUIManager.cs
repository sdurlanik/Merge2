using System;
using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.UI
{
    public class OrderUIManager : MonoBehaviour
    {
        [SerializeField] private Transform _ordersContainer;
        [SerializeField] private OrderUIEntry _orderEntryPrefab;

        [SerializeField] private OrderSettingsSO _orderSettings;
        private readonly List<OrderUIEntry> _uiEntries = new();

        private void Awake()
        {
            for (int i = 0; i < _orderSettings.MaxActiveOrders; i++)
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

        private void Start()
        {
            var orderManager = ServiceLocator.Get<OrderManager>();
            if (orderManager != null)
            {
                HandleActiveOrdersUpdated(new ActiveOrdersUpdatedEvent { ActiveOrders = orderManager.ActiveOrders });
            }
        }

        private void OnDisable()
        {
            EventBus<ActiveOrdersUpdatedEvent>.OnEvent -= HandleActiveOrdersUpdated;
        }

        private void HandleActiveOrdersUpdated(ActiveOrdersUpdatedEvent e)
        {
            var newOrders = e.ActiveOrders;
            var newOrderIds = new HashSet<Guid>(newOrders.Select(o => o.Id));

            var ordersThatNeedASlot = new Queue<Order>(newOrders);

            var availableSlots = new Queue<OrderUIEntry>();

            foreach (var uiEntry in _uiEntries)
            {
                if (uiEntry.CurrentOrder != null)
                {
                    if (newOrderIds.Contains(uiEntry.CurrentOrder.Id))
                    {
                        var updatedOrderData = newOrders.First(o => o.Id == uiEntry.CurrentOrder.Id);
                        uiEntry.SetData(updatedOrderData, _uiEntries.IndexOf(uiEntry));

                        ordersThatNeedASlot =
                            new Queue<Order>(ordersThatNeedASlot.Where(o => o.Id != uiEntry.CurrentOrder.Id));
                    }
                    else
                    {
                        availableSlots.Enqueue(uiEntry);
                    }
                }
                else
                {
                    availableSlots.Enqueue(uiEntry);
                }
            }

            while (ordersThatNeedASlot.Count > 0)
            {
                if (availableSlots.Count > 0)
                {
                    var slot = availableSlots.Dequeue();
                    var orderToDisplay = ordersThatNeedASlot.Dequeue();
                    slot.SetData(orderToDisplay, _uiEntries.IndexOf(slot));
                }
                else
                {
                    break;
                }
            }

            foreach (var slot in availableSlots)
            {
                slot.Hide();
            }
        }
    }
}