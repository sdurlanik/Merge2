using System.Collections.Generic;
using Sdurlanik.Merge2.Data;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class WeightedChanceService
    {
        public static ItemSO GetRandomItem(IReadOnlyList<ProductionChance> chances)
        {
            var totalChance = 0f;
            for (var index = 0; index < chances.Count; index++)
            {
                var chance = chances[index];
                totalChance += chance.Chance;
            }

            var randomPoint = Random.Range(0, totalChance);
            
            foreach (var chance in chances)
            {
                if (randomPoint < chance.Chance)
                {
                    return chance.ItemToProduce;
                }
                randomPoint -= chance.Chance;
            }
            return null;
        }
    }
}