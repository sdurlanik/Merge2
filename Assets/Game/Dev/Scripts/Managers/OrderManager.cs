using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderSettingsSO _orderSettings;
        private readonly List<Order> _activeOrders = new();
        public IReadOnlyList<Order> ActiveOrders => _activeOrders;

        private void OnEnable()
        {
            EventBus<BoardStateChangedEvent>.OnEvent += OnBoardStateChanged;
        }

        private void OnDisable()
        {
            EventBus<BoardStateChangedEvent>.OnEvent -= OnBoardStateChanged;
        }

        private void OnBoardStateChanged(BoardStateChangedEvent e)
        {
            CheckAllOrdersStatus();
        }

        public void CheckAllOrdersStatus(bool forcePublish = false)
        {
            var itemsOnBoard = ServiceLocator.Get<GridManager>().GetCurrentItemCountsOnBoard();
            var statusChanged = false;

            foreach (var order in _activeOrders)
            {
                var originalStatus = order.Status;
                order.UpdateStatus(itemsOnBoard);
                if (originalStatus != order.Status)
                {
                    statusChanged = true;
                }
            }
            
            if (statusChanged || forcePublish)
            {
                EventBus<ActiveOrdersUpdatedEvent>.Publish(new ActiveOrdersUpdatedEvent { ActiveOrders = _activeOrders });
            }
        }

        public void TryToGenerateNewOrders()
        {
            
            while (_activeOrders.Count < _orderSettings.MaxActiveOrders)
            {
                var newOrder = OrderGeneratorService.GenerateOrder();
                if (newOrder != null)
                {
                    _activeOrders.Add(newOrder);
                }
                else
                {
                    Debug.LogError("Could not generate a new order. No more orders available.");
                    break;
                }
            }
        }

        public void CompleteOrder(Order orderToComplete)
        {
            if (orderToComplete == null || !_activeOrders.Contains(orderToComplete) || orderToComplete.Status != OrderStatus.ReadyToComplete)
            {
                Debug.LogError("Order completion failed: Order is null, not active, or not ready to complete.");
                return;
            }

            ServiceLocator.Get<GridManager>().ConsumeItems(orderToComplete.Requirements);
            
            EventBus<GrantRewardEvent>.Publish(new GrantRewardEvent { CoinAmount = orderToComplete.CalculatedReward });

            orderToComplete.MarkAsCompleted();
            _activeOrders.Remove(orderToComplete);
            
            Debug.Log($"Order completed: {orderToComplete.OrderData.OrderName}. Reward: {orderToComplete.CalculatedReward}x coin.");

            TryToGenerateNewOrders();
    
            CheckAllOrdersStatus(forcePublish: true);
        }
        
        public List<string> GetOrdersForSaving()
        {
            return _activeOrders.Select(order => order.OrderData.name).ToList();
        }

        public void LoadOrdersFromSave(List<string> savedOrderNames)
        {
            _activeOrders.Clear();
    
            foreach (var soName in savedOrderNames)
            {
                var orderSO = ServiceLocator.Get<DataBank>().AllOrderTemplates.Find(so => so.name == soName);
                if (orderSO != null)
                {
                    _activeOrders.Add(new Order(orderSO));
                }
            }

            CheckAllOrdersStatus();
            EventBus<ActiveOrdersUpdatedEvent>.Publish(new ActiveOrdersUpdatedEvent { ActiveOrders = _activeOrders });
        }
        
        public Sprite GetRandomAvatarSprite()
        {
            var avatarList = _orderSettings.CustomerAvatars;
            var randomIndex = Random.Range(0, avatarList.Count);
            return avatarList[randomIndex];
        }
    }
}