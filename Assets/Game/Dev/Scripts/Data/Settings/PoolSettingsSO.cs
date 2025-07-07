using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "PoolSettings", menuName = "Merge2/Settings/PoolSettings", order = 1)]
    public class PoolSettingsSO : ScriptableObject
    {
        public GameObject DefaultItemPrefab;
        public int InitialPoolSize = 25;
    }
}