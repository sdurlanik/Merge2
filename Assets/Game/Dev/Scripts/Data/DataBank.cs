using UnityEngine;
using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;

namespace Sdurlanik.Merge2.Data
{
    public class DataBank : Singleton<DataBank>
    {
        [Header("Item Data")]
        public List<ItemSO> AllItems;
        
        [Header("Order Data")]
        public List<OrderDataSO> AllOrderTemplates;
        
        private Dictionary<string, ItemSO> _itemSODictionary;
        
        protected override void Awake()
        {
            base.Awake();
            FillItemSODictionary();
          
        }
        
        private void FillItemSODictionary()
        {
            _itemSODictionary = new Dictionary<string, ItemSO>();
            for (var i = 0; i < AllItems.Count; i++)
            {
                var itemSO = AllItems[i];
                if (itemSO != null)
                {
                    _itemSODictionary.TryAdd(itemSO.name, itemSO);
                }
            }
        }
        
        public ItemSO GetSO(ItemFamily fam, int level)
        {
            return AllItems.Find(x=>x.Family==fam && x.Level==level);
        }
        
        public ItemSO GetSOByName(string soName)
        {
            _itemSODictionary.TryGetValue(soName, out var itemSO);
            return itemSO;
        }
    }
}