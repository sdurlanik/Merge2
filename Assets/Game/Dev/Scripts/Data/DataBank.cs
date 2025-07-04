using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Data
{
    public class DataBank : MonoBehaviour
    {
        public static DataBank Instance;
        public List<ItemSO> AllItems;

        void Awake() { Instance = this; }

        public ItemSO GetSO(ItemFamily fam, int level)
            => AllItems.Find(x=>x.Family==fam && x.Level==level);
    }

}