using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class ItemFactory
    {
        public static Item Create(ItemSO so, Cell targetCell)
        {
            var newItem = Object.Instantiate(so.ItemPrefab);
            newItem.Init(so);
            targetCell.PlaceItem(newItem);
            return newItem;
        }
    }
}