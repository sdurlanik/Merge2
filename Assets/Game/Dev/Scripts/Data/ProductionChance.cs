using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Data
{
    [System.Serializable]
    public struct ProductionChance
    {
        public ItemSO ItemToProduce;
        [Range(0, 100)] public float Chance;
    }
}