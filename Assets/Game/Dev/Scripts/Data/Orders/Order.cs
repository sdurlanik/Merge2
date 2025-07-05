using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.Data.Orders
{
   public enum OrderStatus
    {
        Active,          
        ReadyToComplete,
        Completed
    }

    public class Order
    {
        public OrderDataSO OrderData { get; }
        public List<OrderRequirement> Requirements { get; }
        public int CalculatedReward { get; }
        public OrderStatus Status { get; private set; }
        public Sprite AvatarSprite { get; private set; }

        private readonly Dictionary<ItemSO, int> _fulfilledAmount = new Dictionary<ItemSO, int>();

        public Order(OrderDataSO orderData)
        {
            OrderData = orderData;
            Requirements = new List<OrderRequirement>(orderData.RequiredItems);
            Status = OrderStatus.Active;
            AvatarSprite = ServiceLocator.Get<OrderManager>().GetRandomAvatarSprite();
            
            var totalLevel = 0;
            for (var index = 0; index < Requirements.Count; index++)
            {
                var req = Requirements[index];
                totalLevel += req.RequiredItem.Level;
            }

            CalculatedReward = orderData.BaseCoinReward + Mathf.RoundToInt(totalLevel * orderData.CoinRewardPerLevelMultiplier);

            foreach (var req in Requirements)
            {
                _fulfilledAmount[req.RequiredItem] = 0;
            }
        }
        
        public void UpdateStatus(Dictionary<ItemSO, int> itemsOnBoard)
        {
            if (Status == OrderStatus.Completed) return;

            var allRequirementsMet = true;
            foreach (var req in Requirements)
            {
                if (!itemsOnBoard.TryGetValue(req.RequiredItem, out var countOnBoard) || countOnBoard < 1)
                {
                    allRequirementsMet = false;
                    break;
                }
            }
            
            Status = allRequirementsMet ? OrderStatus.ReadyToComplete : OrderStatus.Active;
        }
        
        public void MarkAsCompleted()
        {
            Status = OrderStatus.Completed;
        }
    }
}