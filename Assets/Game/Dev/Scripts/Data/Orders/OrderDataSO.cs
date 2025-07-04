using System.Collections.Generic;
using UnityEngine;

namespace Sdurlanik.Merge2.Data.Orders
{
    [CreateAssetMenu(fileName = "NewOrderData", menuName = "Merge2/Orders/Order Data")]
    public class OrderDataSO : ScriptableObject
    {
        public string OrderName;
        public List<OrderRequirement> RequiredItems;
        
        [Header("Reward Calculation")]
        public int BaseCoinReward;
        public float CoinRewardPerLevelMultiplier;
        
        [Header("Generation Settings")]
        public int RequiredGeneratorMinLevel = 5;
        public int RequiredGeneratorMaxLevel = 10;
    }
}