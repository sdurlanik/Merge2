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
        
        public ItemSO GetSO(ItemFamily fam, int level)
            => AllItems.Find(x=>x.Family==fam && x.Level==level);
    }

}