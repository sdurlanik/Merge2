using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class OrderGeneratorService
    {
        public static Order GenerateOrder()
        {
            var maxGeneratorLevel = ServiceLocator.Get<GridManager>().GetHighestGeneratorLevelOnBoard();
            
            var allTemplates = ServiceLocator.Get<DataBank>().AllOrderTemplates;
            if (allTemplates == null || allTemplates.Count == 0)
            {
                Debug.LogError("There are no order templates exist in the DataBank.");
                return null;
            }

            var suitableTemplates = allTemplates
                .Where(t => maxGeneratorLevel >= t.RequiredGeneratorMinLevel && maxGeneratorLevel <= t.RequiredGeneratorMaxLevel)
                .ToList();

            if (suitableTemplates.Count == 0)
            {
                Debug.LogWarning($"No suitable order templates found for generator level: {maxGeneratorLevel}.");
                return null;
            }

            var randomIndex = Random.Range(0, suitableTemplates.Count);
            var selectedTemplate = suitableTemplates[randomIndex];
            
            var newOrder = new Order(selectedTemplate);
            
            Debug.Log($"Generated new order from template: {selectedTemplate.OrderName} for generator level: {maxGeneratorLevel}.");
            return newOrder;
        }
    }
}