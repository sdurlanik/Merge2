using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Items.Behaviours
{
    public class ProducerBehavior
    {
        private readonly Item _item;
        private readonly ItemSO _itemData;

        public ProducerBehavior(Item item)
        {
            _item = item;
            _itemData = item.ItemDataSO;
        }

        public void OnTap()
        {
            if (!_itemData.CanProduce) return;

            var productToSpawn = WeightedChanceService.GetRandomItem(_itemData.ProductionChances);
            if (productToSpawn == null) return;

            if (GridManager.Instance.TryGetEmptyCell(out var emptyCell))
            {
                ItemFactory.Create(productToSpawn, emptyCell);
            }
        }
    }
}