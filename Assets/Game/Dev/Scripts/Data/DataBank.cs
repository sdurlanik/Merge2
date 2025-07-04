using UnityEngine;
using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Data
{
    public class DataBank : Singleton<DataBank>
    {
        public List<ItemSO> AllItems;
        
        public ItemSO GetSO(ItemFamily fam, int level)
            => AllItems.Find(x=>x.Family==fam && x.Level==level);
    }

}