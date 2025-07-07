using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Items.Behaviours
{
    public class ProducerBehavior
    {
        private readonly Item _item;
        private readonly GeneratorSO _generatorData;

        public ProducerBehavior(Item item, GeneratorSO generatorData)
        {
            _item = item;
            _generatorData = generatorData;
        }

        public void OnTap()
        {
            if (!_generatorData.CanProduce) return;

            var productToSpawn = WeightedChanceService.GetRandomItem(_generatorData.ProductionChances);
            if (productToSpawn == null) return;

            if (ServiceLocator.Get<GridManager>().TryGetEmptyCell(out var emptyCell))
            {
                ItemFactoryService.Create(productToSpawn, emptyCell);
            }
        }
    }
}